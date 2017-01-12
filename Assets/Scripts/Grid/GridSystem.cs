using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemies;
using Game.Manager;
using Game.Systems;
using Game.Sfx;
using Game.Interface;
//By @JavierBullrich

namespace Game.Grid {
    [RequireComponent(typeof(SoundPlayer))]
	public class GridSystem : MonoBehaviour, IReset {
        public Invader invader;
        public int height, width;
        [Range(0.7f, 2)]
        public float distanceBetween = 0.9f;
        public IGridElement[,] elements;
        public float MovementPause;
        float originalMovementPause, movementTime = .1f;
        public int PercentageToMove;
        GameObject gridContainer;
        bool goingRight = true;
        SystemCalculations calcs;
        int moves;
        SoundPlayer sfxPlayer;
        Vector2 startPos;

		void Start () {
            elements = new IGridElement[height, width];
            calcs = new SystemCalculations();
            sfxPlayer = GetComponent<SoundPlayer>();
            originalMovementPause = MovementPause;
            PopulateGrid();
		}

        public void Respawn()
        {
            Destroy(gridContainer);
            System.GC.Collect();
            MovementPause = originalMovementPause;
            goingRight = true;
            PopulateGrid();
            sfxPlayer.ChangePitch(1);
        }

        int AmountOfElementsActive()
        {
            int activeElements = 0;
            foreach(IGridElement el in elements)
            {
                if (el.isActive())
                    activeElements++;
            }
            return activeElements;
        }

        private void PopulateGrid()
        {
            Vector2 spawnPos = transform.position;
            gridContainer = new GameObject();
            gridContainer.transform.position = Vector3.zero;
            gridContainer.name = invader.name + "Container";
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    GameObject enemy = Instantiate(invader.gameObject);
                    enemy.transform.position = spawnPos + new Vector2(distanceBetween * j, 0);
                    enemy.name = invader.gameObject.name + '[' + i + ", " + j + ']';
                    enemy.transform.parent = gridContainer.transform;
                    elements[i, j] = enemy.GetComponent<IGridElement>();
                    elements[i, j].SetGrid(this);
                }
                spawnPos.y -= distanceBetween;
            }
            
        }

        public int[] GetCords(IGridElement el)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (elements[i, j] == el)
                        return new int[] { i, j };
                }
            }
            return new int[0];
        }

        public IGridElement GetRandomLastElement()
        {
            IGridElement elem = null;
            while(elem == null)
            {
                int xCord = Random.Range(0, width);
                for (int i = 0; i < height; i++)
                {
                    if (elements[i, xCord].isActive())
                        elem = elements[i, xCord];
                }
                if(elem == null)
                {
                    bool active = false;
                    foreach(IGridElement el in elements)
                    {
                        if (el.isActive())
                            active = true;
                    }
                    if (!active)
                    {
                        Debug.Log("There are no more active elements in the grid");
                        GameManager.instance.LevelCompleted();
                        return null;
                    }
                }
            }
            return elem;
        }

        /// <summary>Returns the score from the destruction of a ship</summary>
        public int DestroyShip(IGridElement el)
        {
            int[] shipCords = GetCords(el);
            int colorType = el.getColorType();
            int comboMultiplier = 1, lastCombo = 1;

            foreach(IGridElement elem in getAdjacentElements(shipCords[0], shipCords[1]))
            {
                if (elem != null && elem.getColorType() == colorType && elem.getGameobject().activeInHierarchy)
                {
                    elem.ChainDestroy();

                    int temp = comboMultiplier;
                    comboMultiplier = lastCombo;
                    lastCombo = temp + lastCombo;
                }
            }
            SpeedUp();

            return 10 * lastCombo;
        }

        private IGridElement[] getAdjacentElements(int shipY, int shipX)
        {
            IGridElement[] els = new IGridElement[4];

            if (shipY > 0)
                els[0] = elements[shipY - 1, shipX];

            if (shipY < height - 1)
            {
                els[1] = elements[shipY + 1, shipX];
            }

            if (shipX > 0)
                els[2] = elements[shipY, shipX - 1];

            if (shipX < width - 1)
                els[3] = elements[shipY, shipX + 1];

            return els;
        }

        private bool ChainDestroy(int shipX, int shipY, int colorType)
        {
            if (elements[shipX, shipY].getColorType() == colorType && elements[shipX, shipY].getGameobject().activeInHierarchy)
            {
                elements[shipX, shipY].ChainDestroy();
                return true;
            }
            return false;
        }

        private void Update()
        {
            MoveGrid();
        }

        private IGridElement getBorderElement(ElementPosition pos)
        {
            int[] deepValue = new int[2] { 0, 0 };
            if (pos == ElementPosition.Left || pos == ElementPosition.Top)
                deepValue = (pos == ElementPosition.Top ? new int[2] { height, 0 } : new int[2] { 0, width });
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (elements[i, j].isActive())
                    {
                        switch (pos)
                        {
                            case ElementPosition.Top:
                                if (i < deepValue[0])
                                    deepValue = new int[2] { i, j };
                                break;
                            case ElementPosition.Bottom:
                                if (i > deepValue[0])
                                    deepValue = new int[2] { i, j };
                                break;
                            case ElementPosition.Left:
                                if (j < deepValue[1])
                                    deepValue = new int[2] { i, j };
                                break;
                            case ElementPosition.Right:
                                if (j > deepValue[1])
                                    deepValue = new int[2] { i, j };
                                break;
                            default:
                                break;
                        }

                    }
                }
                
            }

            if (deepValue[0] >= elements.GetLength(0) || deepValue[1] >= elements.GetLength(1))
                deepValue = new int[2] { 0, 0 };

            print("[" + deepValue[0] + ", " + deepValue[1] + ']');
            return elements[deepValue[0], deepValue[1]];
        }

        private void MoveGrid()
        {
            if (movementTime > MovementPause)
            {
                movementTime = 0;
                Vector3 vectorMovement;

                if (goingRight && calcs.FloatToPercentage(Camera.main.WorldToScreenPoint(getBorderElement(ElementPosition.Right).getPosition()).x, Screen.width) < 97)
                    vectorMovement = new Vector3(calcs.PercentageToFloat(PercentageToMove, Screen.width), 0);
                else if (!goingRight && calcs.FloatToPercentage(Camera.main.WorldToScreenPoint(getBorderElement(ElementPosition.Left).getPosition()).x, Screen.width) > 3)
                    vectorMovement = new Vector3(calcs.PercentageToFloat(PercentageToMove, Screen.width) * -1, 0);
                else
                {
                    vectorMovement = new Vector3(0, -(calcs.PercentageToFloat(PercentageToMove, Screen.width)));
                    goingRight = !goingRight;
                    //SpeedUp();
                }

                gridContainer.transform.position = Camera.main.ScreenToWorldPoint(
                    Camera.main.WorldToScreenPoint(gridContainer.transform.position) +
                    vectorMovement);
                foreach(IGridElement el in elements)
                {
                    el.ExecuteMovement();
                }

                FireToPlayer();
                sfxPlayer.PlaySFX(GameManager.instance.getSoundManager().getFastInvaderSound(1));
            }
            else
                movementTime += GameManager.DeltaTime;
        }

        private void SpeedUp()
        {
            //MovementPause -= (MovementPause / 10);
            MovementPause = calcs.PercentageToFloat(calcs.FloatToPercentage(AmountOfElementsActive(), elements.Length), originalMovementPause);
            sfxPlayer.ChangePitch(1 + (1 - calcs.PercentageToFloat(calcs.FloatToPercentage(AmountOfElementsActive(), elements.Length), 1)));
        }

        void FireToPlayer()
        {
            if (moves > Random.Range(0, 3))
            {
                moves = 0;
                GameObject bullet = GameManager.instance.returnPooledObject("EnemyBullet");
                if (bullet != null)
                {
                    IGridElement spawner = GetRandomLastElement();
                    if (spawner != null)
                    {
                        bullet.transform.position = spawner.getPosition();
                        bullet.SetActive(true);
                    }
                }
            }
            else
                moves++;
        }

        public enum ElementPosition
        {
            Top, Bottom, Left, Right
        }

        private void OnDrawGizmos()
        {
            Vector3 spawnPos = transform.position;
            float size = .3f;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Vector3 spawnPosition = spawnPos + new Vector3(distanceBetween * j, 0);
                    Gizmos.DrawLine(spawnPosition - Vector3.up * size, spawnPosition + Vector3.up * size);
                    Gizmos.DrawLine(spawnPosition - Vector3.left * size, spawnPosition + Vector3.left * size);
                }
                spawnPos.y -= distanceBetween;
            }
        }
    }
}