using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Manager;
using Game.Obj;
using Game.Systems;
using Game.Interface;
using Game.Sfx;
//By @JavierBullrich

namespace Game.Player
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SoundPlayer))]
    public class Player : MonoBehaviour, IDamagable, IReset {
        public float speed;
        SystemCalculations calcs;
        SoundPlayer sfxPlayer;
        Vector2 startPos;
        bool alive = true;
        Sprite shipSprite;
        public Sprite destroyedSprite;
        SpriteRenderer spr;

		void Start () {
            calcs = new SystemCalculations();
            sfxPlayer = GetComponent<SoundPlayer>();
            startPos = transform.position;
            spr = GetComponent<SpriteRenderer>();
            shipSprite = spr.sprite;
		}

        private void Update()
        {
            InputHandler();
            if (Input.GetKeyDown(KeyCode.X))
                Shoot();
        }

        public void Respawn()
        {
            transform.position = startPos;
            gameObject.SetActive(true);
            alive = true;
            spr.sprite = shipSprite;
        }

        void Shoot()
        {
            GameObject bullet = GameManager.instance.returnPooledObject(new PlayerBullet());
            if (bullet != null)
            {
                bullet.transform.position = transform.position;
                bullet.SetActive(true);
                sfxPlayer.PlaySFX(GameManager.instance.getSoundManager().getSfx(SoundManager.Sfx.shoot));
            }
        }

        private void InputHandler()
        {
            Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
            if(alive)
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
            GameManager.instance.getSoundManager().PlaySFX((GameManager.instance.getSoundManager().getSfx(Manager.SoundManager.Sfx.explosion)));
            StartCoroutine(DestroyAnim());
        }

        IEnumerator DestroyAnim()
        {
            spr.sprite = destroyedSprite;
            yield return new WaitForSeconds(0.4f);
            //gameObject.SetActive(false);
            Respawn();
        }

        public void ReceiveDamage()
        {
            Destroy();
        }
    }
}