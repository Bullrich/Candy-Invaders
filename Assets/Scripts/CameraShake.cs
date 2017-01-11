using UnityEngine;
using System.Collections;
// By @JavierBullrich

namespace Game.GameCamera
{
    public class CameraShake : MonoBehaviour
    {
        // Transform of the camera to shake. Grabs the gameObject's transform
        // if null.
        public Transform camTransform;

        // How long the object should shake for.
        public float shakeDuration = 0f;

        // Amplitude of the shake. A larger value shakes the camera harder.
        public float shakeAmount = 0.7f;
        public float decreaseFactor = 1.0f;

        Vector3 originalPos;

        void Awake()
        {
            if (camTransform == null)
            {
                camTransform = transform;//GetComponent(typeof(Transform)) as Transform;
            }
        }

        void Start()
        {
            originalPos = camTransform.localPosition;
        }

        public void Shake(float shakeDur)
        {
            originalPos = camTransform.localPosition;
            shakeDuration = shakeDur;
        }

        public bool isShaking()
        {
            return !(shakeDuration == 0);
        }

        void Update()
        {
            if (shakeDuration > 0)
            {
                camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shakeDuration = 0f;
                //camTransform.localPosition = originalPos;
                //originalPos = camTransform.localPosition;
                camTransform.localPosition = originalPos;
            }
        }

    }
}