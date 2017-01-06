using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//By @JavierBullrich

namespace Game.Enemies {
	public class Invader : MonoBehaviour {
        public enum InvaderColor
        {
            Yellow,
            Red,
            Blue,
            Green
        }
        public InvaderColor shipColor;
        SpriteRenderer spr;

        private void Awake()
        {
            spr = GetComponent<SpriteRenderer>();
            spr.color = getColor();
        }

        public float getSpriteWidth()
        {
            if(spr == null)
                GetComponent<SpriteRenderer>();
            return spr.sprite.rect.width;
        }

        Color getColor()
        {
            Color color = Color.white;
            switch (shipColor)
            {
                case InvaderColor.Yellow:
                    color = Color.yellow;
                    break;
                case InvaderColor.Red:
                    color = Color.red;
                    break;
                case InvaderColor.Blue:
                    color = Color.blue;
                    break;
                case InvaderColor.Green:
                    color = Color.green;
                    break;
                default:
                    break;
            }
            return color;
        }

		void Start () {
            //print(getSpriteWidth());
		}
	}
}