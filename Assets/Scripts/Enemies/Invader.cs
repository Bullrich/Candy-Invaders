using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Animation;
using Game.Grid;
//By @JavierBullrich

namespace Game.Enemies {
	public class Invader : MonoBehaviour, IGridElement {

        public Color[] colors;

        SpriteRenderer spr;
        Animator anim;
        [SerializeField]
        public AnimationSystem anims;
        bool movement;

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
            int colorIndex = Random.Range(0, colors.Length);
            return getColor(colorIndex);

        }

		void Start () {
            //print(getSpriteWidth());
		}

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
                MovementAnim();
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