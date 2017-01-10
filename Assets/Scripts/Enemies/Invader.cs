using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Animation;
using Game.Grid;
//By @JavierBullrich

namespace Game.Enemies {
    [RequireComponent(typeof(BoxCollider2D))]
	public class Invader : MonoBehaviour, IGridElement {

        public Color[] colors;

        SpriteRenderer spr;
        [SerializeField]
        public AnimationSystem anims;
        bool movement;
        GridSystem grid;
        int colorType;

        public bool testBool;

        private void Awake()
        {
            spr = GetComponent<SpriteRenderer>();
            spr.color = RandomColor();
            anims.SetUp(GetComponent<SpriteRenderer>());
        }

        public float getSpriteWidth()
        {
            if(spr == null)
                GetComponent<SpriteRenderer>();
            return spr.sprite.rect.width;
        }

        #region Interface Functions
        public bool isActive()
        {
            return gameObject.activeInHierarchy;
        }
        public Vector2 getPosition()
        {
            return transform.position;
        }
        public GameObject getGameobject()
        {
            return gameObject;
        }
        public void ExecuteMovement()
        {
            MovementAnim();
        }
        public void SetGrid(GridSystem gr)
        {
            grid = gr;
        }
        public int getColorType()
        {
            return colorType;
        }
        public void ChainDestroy()
        {
            gameObject.SetActive(false);
        }
        #endregion

        public void MovementAnim()
        {
            string currAnim = "Default";
            if(anims.GetCurrentAnim() == (currAnim + (movement ? 1 : 0)))
            {
                movement = !movement;
                anims.ChangeSprite(currAnim + (movement ? 1 : 0));
            }
        }

        Color getColor(int colorIndex)
        {
            return colors[colorIndex];
        }

        public Color RandomColor()
        {
            colorType = Random.Range(0, colors.Length);
            return getColor(colorType);
        }

        void Destroy()
        {
            grid.DestroyShip(this);
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
                MovementAnim();

            if (testBool)
            {
                testBool = false;
                Destroy();
            }
        }
    }

    [System.Serializable]
    public class ColorSystem
    {
        public Color red, blue, yellow, green;

        public Color[] getColors()
        {
            Color[] colors = { red, blue, yellow, green };
            return colors;
        }
    }
}