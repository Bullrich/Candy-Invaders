using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Manager;
using Game.Obj;
using Game.Systems;
using Game.Interface;
//By @JavierBullrich

namespace Game.Player {
    [RequireComponent(typeof(BoxCollider2D))]
	public class Player : MonoBehaviour, IDamagable {
        public float speed;
        SystemCalculations calcs;

		void Start () {
            calcs = new SystemCalculations();
		}

        private void Update()
        {
            InputHandler();
            if (Input.GetKeyDown(KeyCode.X))
            {
                GameObject bullet = GameManager.instance.returnPooledObject(new PlayerBullet());
                if (bullet != null)
                {
                    bullet.transform.position = transform.position;
                    bullet.SetActive(true);
                }
            }
        }

        private void InputHandler()
        {
            Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

            Movement(directionalInput);
        }


        private void Movement(Vector2 moveDir)
        {
            Vector2 amountToMove = moveDir * GameManager.DeltaTime * speed;
            int screenPosPercentage = calcs.FloatToPercentage(Camera.main.WorldToScreenPoint(transform.position).x, Screen.width);

            if (moveDir.x < 0 && screenPosPercentage < 3)
                amountToMove.x = 0;
            else if (moveDir.x > 0 && screenPosPercentage > 97)
                amountToMove.x = 0;

            transform.Translate(amountToMove);
        }
        public void Destroy()
        {
            gameObject.SetActive(false);
        }

        public void ReceiveDamage()
        {
            Destroy();
        }
    }
}