using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity.AudioVis
{

    //A simple component to visualize the raw audio data coming out of SoundCapture
    public class Paddles : MonoBehaviour
    {
        public SoundCapture.DataSource Target = SoundCapture.DataSource.AverageVolume;
        public int BandIndex;

        

        
        private SoundCapture _sc;
        private Quaternion paddleStartRot;
        private Quaternion rotateTo;
        
        public void Start()
        {
            
            _sc = FindObjectOfType<SoundCapture>();
            paddleStartRot = transform.rotation;
            
        }

        public void Update()
        {
            if (gameObject.tag == "PaddleLeft")
            {
                rotateTo = Quaternion.Euler(45f, paddleStartRot.y, Mathf.Clamp(paddleStartRot.z + GetValue() * 150 - 45, -45, 70));
                GetComponent<Rigidbody>().MoveRotation(rotateTo);
            }
            else if (gameObject.tag == "PaddleRight")
            {
                rotateTo = Quaternion.Euler(45f - 180, paddleStartRot.y, Mathf.Clamp(paddleStartRot.z + GetValue() * 150 - 45, -45, 70) - 180);
                
                GetComponent<Rigidbody>().MoveRotation(rotateTo);
            }

        }

        private float GetValue()
        {
            switch (Target) {
                case SoundCapture.DataSource.AverageVolume:
                    return _sc.AverageVolume;
                case SoundCapture.DataSource.PeakVolume:
                    return _sc.PeakVolume;
                case SoundCapture.DataSource.SingleBand:
                    return _sc.BarData[Mathf.Clamp(BandIndex, 0, _sc.BarData.Length)];
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

   
        private IEnumerator InitializeAfterBarData()
        {
            //waits for the sound capture FFT data to be initially populated, then creates the cubes
            _sc = FindObjectOfType<SoundCapture>();
            while (_sc.BarData.Length == 0) yield return null;

        }
    }
}