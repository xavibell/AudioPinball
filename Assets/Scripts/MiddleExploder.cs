using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity.AudioVis
{

    //A simple component to visualize the raw audio data coming out of SoundCapture
    public class MiddleExploder : MonoBehaviour
    {
        public SoundCapture.DataSource Target = SoundCapture.DataSource.AverageVolume;
        public int BandIndex;

        

        
        private SoundCapture _sc;
        private Vector3 startScale;


        public void Start()
        {
            
            _sc = FindObjectOfType<SoundCapture>();
            startScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            
        }

        public void Update()
        {

            transform.localScale = new Vector3(startScale.x + GetValue() / 2, startScale.y , startScale.z + GetValue() / 2);
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