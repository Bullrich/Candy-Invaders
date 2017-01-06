using UnityEngine;
using System.Collections;
// By @JavierBullrich
namespace Game.Camara
{
    public class ForceCameraSize : MonoBehaviour
    {

        Camera myCam;
        public int desiredWidth, desiredHeight;

        void Start()
        {
            myCam = GetComponent<Camera>();


        }

        private void Update()
        {
            myCam.aspect = (float)desiredWidth / (float)desiredHeight;

            //myCam.fieldOfView = myDesiredHorizontalFov * normalAspect / ((float)myCam.pixelWidth / myCam.pixelHeight);
        }

    }
}