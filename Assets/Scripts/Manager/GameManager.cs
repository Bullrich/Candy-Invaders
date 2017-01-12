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
        public Player.Player player;
        public GameObject[] iReset;
        int score, lifes = 3;

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

        #region Getters
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
        #endregion

        #region Setters
        public void AddScore(int newScore)
        {
            score += newScore;
            uiManager.UpdateScore(score);
        }
        #endregion

        public void PlayerDestroyed()
        {
            lifes--;
            if (lifes > 0)
            {
                player.Respawn();
                uiManager.LostALife();
            }
            else
            {
                ResetGame();
                score = 0;
                uiManager.UpdateScore(score);
                lifes = 3;
            }
        }

        public void LevelCompleted()
        {
            ResetGame();
            uiManager.UpdateScore(score);
        }

        private void Update()
        {
            DeltaTime = Time.deltaTime;
        }
    }
}