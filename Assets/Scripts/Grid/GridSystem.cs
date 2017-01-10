using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemies;
using Game.Manager;
using Game.Systems;
//By @JavierBullrich

namespace Game.Grid {
	public class GridSystem : MonoBehaviour {
        public Invader invader;
        public int height, width;
        [Range(0.7f, 2)]
        public float distanceBetween = 0.9f;
        public IGridElement[,] elements;
        public ElementPosition elPos;
        public float MovementPause;
        float movementTime = .1f;
        public int PercentageToMove;
        GameObject gridContainer;
        bool goingRight = true;
        SystemCalculations calcs;
        int moves;

		void Start () {
            elements = new IGridElement[height, width];
            calcs = new SystemCalculations();
            PopulateGrid();
		}

        private void PopulateGrid()
        {
            //float distance = (invader.getSpriteWidth() / 50);
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
                        break;
                    }
                }
            }
            return elem;
        }

        public void DestroyShip(IGridElement el)
        {
            int[] shipCords = GetCords(el);
            int colorType = el.getColorType();
            int comboMultiplier = 0, lastCombo = 1;

            foreach(IGridElement elem in getAdjacentElements(shipCords[0], shipCords[1]))
            {
                if (elem != null && elem.getColorType() == colorType && elem.getGameobject().activeInHierarchy)
                    elem.ChainDestroy();

                int temp = comboMultiplier;
                comboMultiplier = lastCombo;
                lastCombo = temp + lastCombo;
            }
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
        IGridElement element;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
                print(getBorderElement(elPos).getPosition() + " " + getBorderElement(elPos).getGameobject());
            else if (Input.GetKeyDown(KeyCode.G))
                calcs.FloatToPercentage(Camera.main.WorldToScreenPoint(getBorderElement(elPos).getPosition()).x, Screen.width);
            else if (Input.GetKeyDown(KeyCode.J))
                print(calcs.PercentageToFloat(PercentageToMove, Screen.width));
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                if (element != null)
                    element.getGameobject().SetActive(false);
                element = GetRandomLastElement();
                element.getGameobject().GetComponent<SpriteRenderer>().color = Color.white;
            }

                MoveGrid();
        }

        private IGridElement getBorderElement(ElementPosition pos)
        {
            int[] deepValue = new int[2];
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
                    MovementPause -= (MovementPause / 10);
                }

                gridContainer.transform.position = Camera.main.ScreenToWorldPoint(
                    Camera.main.WorldToScreenPoint(gridContainer.transform.position) +
                    vectorMovement);
                foreach(IGridElement el in elements)
                {
                    el.ExecuteMovement();
                }

                FireToPlayer();
            }
            else
                movementTime += GameManager.DeltaTime;
        }

        void FireToPlayer()
        {
            if (moves > Random.Range(0, 3))
            {
                moves = 0;
                GameObject bullet = GameManager.instance.returnPooledObject("EnemyBullet");
                if (bullet != null)
                {
                    bullet.transform.position = GetRandomLastElement().getPosition();
                    bullet.SetActive(true);
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