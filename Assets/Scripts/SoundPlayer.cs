using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//By @JavierBullrich

namespace Game.Sfx {
    [RequireComponent(typeof(AudioSource))]
	public class SoundPlayer : MonoBehaviour {
        AudioSource audioSource;

		void Start () {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
		}

        public void PlaySFX(AudioClip sfx)
        {
            audioSource.clip = sfx;
            audioSource.Play();
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