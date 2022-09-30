using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity.AudioVis
{

    //A simple component to visualize the raw audio data coming out of SoundCapture
    public class Gates : MonoBehaviour
    {
        public SoundCapture.DataSource Target = SoundCapture.DataSource.AverageVolume;
        public int BandIndex;

        

        
        private SoundCapture _sc;
        private Vector3 pusherStartPos;
        
        private Vector3 moveTo;
        
        public void Awake()
        {
            
            _sc = FindObjectOfType<SoundCapture>();
            
            
        }

        public void Update()
        {
            if (GetValue() < 0.3f)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 0);
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, GetValue());
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