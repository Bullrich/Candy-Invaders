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
        public UIManager uiManager;
        public GameObject[] iReset;

        private void Awake()
        {
            instance = this;
            pool = GetComponent<PoolManager>();
            sound = GetComponent<SoundManager>();
        }

        void ResetGame()
        {
            foreach(GameObject res in iReset)
            {
                res.GetComponent<Game.Interface.IReset>().Respawn();
            }
            uiManager.Respawn();
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