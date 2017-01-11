using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Interface;
//By @JavierBullrich

namespace Game.Manager {
	public class UIManager : MonoBehaviour, IReset {
        public Text scoreTxt;

        public void UpdateScore(int newScore)
        {
            scoreTxt.text = newScore + "";
        }

        public void Respawn()
        {

        }
	}
}