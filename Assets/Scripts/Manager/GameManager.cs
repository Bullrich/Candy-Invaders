using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//By @JavierBullrich

namespace Game {
	public class GameManager : MonoBehaviour {
        public static float DeltaTime;
        public float time;
		void Start () {
			
		}

        private void Update()
        {
            DeltaTime = Time.deltaTime;
            time = DeltaTime;
        }
    }
}