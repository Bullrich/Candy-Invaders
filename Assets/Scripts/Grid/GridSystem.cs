using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemies;
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
        bool goingRight;

		void Start () {
            elements = new IGridElement[height, width];
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
                }
                spawnPos.y -= distanceBetween;
            }
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
                print(getBorderElement(elPos).getPosition() + " " + getBorderElement(elPos).getGameobject());
            else if (Input.GetKeyDown(KeyCode.G))
                FloatToPercentage(Camera.main.WorldToScreenPoint(getBorderElement(elPos).getPosition()).x, Screen.width);
            else if (Input.GetKeyDown(KeyCode.J))
                print(PercentageToFloat(PercentageToMove, Screen.width));

                if (movementTime > MovementPause)
            {
                movementTime = 0;
                Vector3 vectorMovement;
                if (goingRight && FloatToPercentage(Camera.main.WorldToScreenPoint(getBorderElement(ElementPosition.Right).getPosition()).x, Screen.width) < 95)
                    vectorMovement = new Vector3(PercentageToFloat(PercentageToMove, Screen.width), 0);
                else if (!goingRight && FloatToPercentage(Camera.main.WorldToScreenPoint(getBorderElement(ElementPosition.Left).getPosition()).x, Screen.width) > 5)
                    vectorMovement = new Vector3(PercentageToFloat(PercentageToMove, Screen.width) * -1, 0);
                else
                {
                    vectorMovement = new Vector3(0, -(PercentageToFloat(PercentageToMove, Screen.width)));
                    goingRight = !goingRight;
                }

                gridContainer.transform.position = Camera.main.ScreenToWorldPoint(
                    Camera.main.WorldToScreenPoint(gridContainer.transform.position) +
                    vectorMovement);
            }
            else
                movementTime += GameManager.DeltaTime;
        }

        private IGridElement getBorderElement(ElementPosition pos)
        {
            int deepElement = 0;
            int[] deepValue = new int[2];
            if (pos == ElementPosition.Left || pos == ElementPosition.Top)
                deepElement = (pos == ElementPosition.Top ? height : width);
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
            return elements[deepValue[0], deepValue[1]];
        }

        void CalculateDistance()
        {
            IGridElement elementRight = getBorderElement(ElementPosition.Right);
            IGridElement elementLeft = getBorderElement(ElementPosition.Left);

            Camera cam = Camera.main;
            float cheight = 2f * cam.orthographicSize;
            float cwidth = cheight * cam.aspect;
            print(Screen.width + " | " + Screen.height);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(elementRight.getPosition());
            Debug.Log(elementRight.getPosition() + " target is " + screenPos.x + " pixels from the left");


        }

        /// <summary>Return an int giving the percentage of numPos between 0 and Max </summary>
        int FloatToPercentage(float numPos, float Max)
        {
            float HundredPercent = Max;
            float actualPoint = HundredPercent - numPos;
            float distance = HundredPercent - actualPoint;
            float limit = (distance / HundredPercent) * 100;
            int percentageCompleted = (int)limit;

            if (percentageCompleted < 0) percentageCompleted = 0;
            else if (percentageCompleted > 100) percentageCompleted = 100;

            string percentage = percentageCompleted + "%";
            print(percentage);
            return percentageCompleted;
        }

        float PercentageToFloat(int percentage, float Max)
        {
            float result = (Max / 100) * percentage;
            return result;
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