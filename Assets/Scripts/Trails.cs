using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity.AudioVis
{

    
    //A simple component to visualize the raw audio data coming out of SoundCapture
    public class Trails : MonoBehaviour
    {
        public SoundCapture.DataSource Target = SoundCapture.DataSource.AverageVolume;
        public int BandIndex;

        
        private TrailRenderer trail;
        
        private SoundCapture _sc;
        
        public void Start()
        {
            
            _sc = FindObjectOfType<SoundCapture>();

            trail = GetComponent<TrailRenderer>(); 
        }

        public void Update()
        {

            trail.material.SetColor("_EmissionColor", new Color(GetValue()* 50, GetValue() * 20, GetValue() * 100));


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