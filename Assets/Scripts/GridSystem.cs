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

		void Start () {
            PopulateGrid();
		}

        private void PopulateGrid()
        {
            //float distance = (invader.getSpriteWidth() / 50);
            Vector2 spawnPos = transform.position;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    GameObject enemy = Instantiate(invader.gameObject);
                    enemy.transform.position = spawnPos + new Vector2(distanceBetween * j, 0);
                }
                spawnPos.y -= distanceBetween;
            }
            
        }
	}
}