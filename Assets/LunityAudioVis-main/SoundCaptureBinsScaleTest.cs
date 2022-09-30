using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lunity.AudioVis
{
    public class SoundCaptureBinsScaleTest : MonoBehaviour
    {
        private SoundCapture _sc;
        
        
        public void Start()
        {
            
            StartCoroutine(InitializeAfterBarData());
        }

        private IEnumerator InitializeAfterBarData()
        {
            //waits for the sound capture FFT data to be initially populated, then creates the cubes
            _sc = FindObjectOfType<SoundCapture>();
            while (_sc.BarData.Length == 0) yield return null;
           
        }
    }
}