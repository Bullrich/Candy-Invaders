using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//By @JavierBullrich

namespace Game.Manager
{
    [RequireComponent(typeof(PoolManager))]
    [RequireComponent(typeof(SoundManager))]
    public class GameManager : MonoBehaviour {
        public static float DeltaTime;
        PoolManager pool;
        SoundManager sound;
        public static GameManager instance;

        private void Awake()
        {
            instance = this;
            pool = GetComponent<PoolManager>();
            sound = GetComponent<SoundManager>();
        }

        public GameObject returnPooledObject(string pooledObj)
        {
            return pool.GetPooledObject(pooledObj);
        }
        public GameObject returnPooledObject(object pooledObj)
        {
            return pool.GetPooledObject(pooledObj);
        }

        public SoundManager getSoundManager()
        {
            return sound;
        }

        private void Update()
        {
            DeltaTime = Time.deltaTime;
        }
    }
}