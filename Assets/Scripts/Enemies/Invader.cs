using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Animation;
using Game.Grid;
using Game.Interface;
//By @JavierBullrich

namespace Game.Enemies {
    [RequireComponent(typeof(BoxCollider2D))]
	public class Invader : MonoBehaviour, IGridElement, IDamagable {

        public Color[] colors;

        SpriteRenderer spr;
        [SerializeField]
        public AnimationSystem anims;
        bool movement, alive;
        GridSystem grid;
        int colorType;

        public bool testBool;

        private void Awake()
        {
            spr = GetComponent<SpriteRenderer>();
            anims.SetUp(GetComponent<SpriteRenderer>());
            SetUp();
        }

        void SetUp()
        {
            alive = true;
            spr.color = RandomColor();
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
            StartCoroutine(DestroyAnimation());
        }
        #endregion

        public void MovementAnim()
        {
            if (alive)
            {
                string currAnim = "Default";
                if (anims.GetCurrentAnim() == (currAnim + (movement ? 1 : 0)))
                {
                    movement = !movement;
                    anims.ChangeSprite(currAnim + (movement ? 1 : 0));
                }
            }
        }

        Color getColor(int colorIndex)
        {
            return colors[colorIndex];
        }

        IEnumerator DestroyAnimation()
        {
            alive = false;
            anims.ChangeSprite("Destroy");
            yield return new WaitForSeconds(0.2f);
            gameObject.SetActive(false);
        }

        public Color RandomColor()
        {
            colorType = Random.Range(0, colors.Length);
            return getColor(colorType);
        }

        public void Destroy()
        {
            grid.DestroyShip(this);
            StartCoroutine(DestroyAnimation());
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

        public void ReceiveDamage()
        {
            Destroy();
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