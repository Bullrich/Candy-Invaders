using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//By @JavierBullrich

namespace Game.Manager {
    [RequireComponent(typeof(PoolManager))]
	public class GameManager : MonoBehaviour {
        public static float DeltaTime;
        PoolManager pool;
        public static GameManager instance;

        private void Awake()
        {
            instance = this;
            pool = GetComponent<PoolManager>();
        }

        public GameObject returnPooledObject(string pooledObj)
        {
            return pool.GetPooledObject(pooledObj);
        }
        public GameObject returnPooledObject(object pooledObj)
        {
            return pool.GetPooledObject(pooledObj);
        }

        private void Update()
        {
            DeltaTime = Time.deltaTime;
        }
    }
}