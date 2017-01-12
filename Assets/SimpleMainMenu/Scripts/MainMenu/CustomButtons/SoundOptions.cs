using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//By @JavierBullrich

namespace SimpleMainMenu
{
	public class SoundOptions : CustomButton {
        string soundStatus = "SOUND: ";
        public override void OnClickAction()
        {
            Game.Sfx.SoundPlayer.canPlay = !Game.Sfx.SoundPlayer.canPlay;
            buttonText.text = soundStatus + (Game.Sfx.SoundPlayer.canPlay ? "ON" : "OFF");
        }
    }
}