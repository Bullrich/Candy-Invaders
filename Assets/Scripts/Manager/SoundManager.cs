using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//By @JavierBullrich

namespace Game.Manager
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Sound effects")]
        public AudioClip[] fastInvader;
        public AudioClip explosion, invaderKilled, shoot;

        public enum Sfx
        {
            explosion,
            fastInvader,
            invaderKilled
              ,shoot
        }

        public AudioClip getSfx(Sfx option)
        {
            AudioClip audioSFX;
            switch (option)
            {
                case Sfx.explosion:
                    audioSFX = explosion;
                    break;
                case Sfx.invaderKilled:
                    audioSFX = invaderKilled;
                    break;
                case Sfx.shoot:
                    audioSFX = shoot;
                    break;
                default:
                    audioSFX = null;
                    break;
            }
            return audioSFX;
        }

        public AudioClip getFastInvaderSound(int index)
        {
            if (fastInvader.Length < index - 1)
                return fastInvader[index];
            else
                return null;
        }

    }
}