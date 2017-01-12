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
        [Header("Elements in the game")]
        public UIManager uiManager;
        public Player.Player player;
        public Grid.GridSystem grid;
        public Obj.ProtectionContainer protection;
        int score, lifes = 3;
        bool gamePaused;

        private void Awake()
        {
            instance = this;
            pool = GetComponent<PoolManager>();
            sound = GetComponent<SoundManager>();
        }

        void ResetGame()
        {
            ResetItem(player);
            ResetItem(grid);
            ResetItem(protection);
            ResetItem(uiManager);
            ResetItem(pool);
            gamePaused = false;
            uiManager.UpdateScore(score);
        }
        
        void ResetItem(Interface.IReset resetItem)
        {
            resetItem.Respawn();
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
        public Player.Player getPlayer()
        {
            return player;
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
                EndGame();
            }
        }

        void EndGame()
        {
            score = 0;
            lifes = 3;
            uiManager.ShowEndText();
            gamePaused = true;

            Invoke("ResetGame", 3f);
        }

        public void ShipFinishedGame()
        {
            EndGame();
        }

        public void LevelCompleted()
        {
            ResetGame();
            uiManager.UpdateScore(score);
        }

        private void Update()
        {
            if (!gamePaused)
                DeltaTime = Time.deltaTime;
            else
                DeltaTime = 0;
        }
    }
}