using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//By @JavierBullrich

namespace Game.Sfx {
    [RequireComponent(typeof(AudioSource))]
	public class SoundPlayer : MonoBehaviour {
        AudioSource audioSource;
        public static bool canPlay = true;

		void Start () {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
		}

        public void PlaySFX(AudioClip sfx)
        {
            if (canPlay)
            {
                audioSource.clip = sfx;
                audioSource.Play();
            }
        }

        public void ChangePitch(float pitch)
        {
            audioSource.pitch = pitch;
        }
        public float getPitch()
        {
            return audioSource.pitch;
        }
	}
}