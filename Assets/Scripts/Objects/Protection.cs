using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Interface;
using System;
//By @JavierBullrich

namespace Game.Obj {
	public class Protection : MonoBehaviour, IDamagable, IReset {
        [SerializeField]
        public Animation.AnimationSystem anim;
        int life = 5;

        public void Respawn()
        {
            gameObject.SetActive(true);
            life = 4;
            anim.ChangeSprite(anim.getAnimsLength() - 1);
        }

        public void ReceiveDamage()
        {
            life--;
            if (life <= 0)
            {
                gameObject.SetActive(false);
                Manager.GameManager.instance.getSoundManager().PlaySFX((Manager.GameManager.instance.getSoundManager().getSfx(Manager.SoundManager.Sfx.explosion)));
            }
            else
                anim.ChangeSprite(life - 1);
        }

        void Start () {
            anim.SetUp(GetComponent<SpriteRenderer>());
            Respawn();
		}
    }
}