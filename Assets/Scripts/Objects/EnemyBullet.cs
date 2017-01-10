using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Manager;
using Game.Player;
//By @JavierBullrich

namespace Game.Obj {
	public class EnemyBullet : Bullet {
        float animationChange;
        SpriteRenderer spr;

        private void Start()
        {
            spr = GetComponent<SpriteRenderer>();
        }

        public override void Update()
        {
            base.Update();
            animationChange += GameManager.DeltaTime;
            if (animationChange > 0.15f)
            {
                spr.flipX = !spr.flipX;
                animationChange = 0;
            }
        }
    }
}