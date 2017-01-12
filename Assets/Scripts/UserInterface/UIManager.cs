using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Interface;
//By @JavierBullrich

namespace Game.Manager {
	public class UIManager : MonoBehaviour, IReset {
        public Text scoreTxt;
        public GameObject[] lifesSprites;
        public Text GameOver;

        public void UpdateScore(int newScore)
        {
            scoreTxt.text = newScore + "";
        }

        public void LostALife()
        {
            foreach(GameObject go in lifesSprites)
            {
                if (go.activeInHierarchy)
                {
                    go.SetActive(false);
                    break;
                }
            }
        }

        public void ShowEndText()
        {
            GameOver.gameObject.SetActive(true);
        }

        public void Respawn()
        {
            foreach (GameObject go in lifesSprites)
                go.SetActive(true);
            scoreTxt.text = 0 + "";
            GameOver.gameObject.SetActive(false);
        }
	}
}