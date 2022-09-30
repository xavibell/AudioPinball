using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering.PostProcessing;
namespace Lunity.AudioVis
{
    //Post processing manipulation code taken from https://docs.unity3d.com/Packages/com.unity.postprocessing@3.0/manual/Manipulating-the-Stack.html

    //A simple component to visualize the raw audio data coming out of SoundCapture
    public class Lighting : MonoBehaviour
    {
        public SoundCapture.DataSource Target = SoundCapture.DataSource.AverageVolume;
        public int BandIndex;


        private PostProcessVolume v;
        private AmbientOcclusion ao;
        private SoundCapture _sc;
        
        
        public void Start()
        {
            
            _sc = FindObjectOfType<SoundCapture>();
            ao = ScriptableObject.CreateInstance<AmbientOcclusion>();
            ao.enabled.Override(true);
            ao.intensity.Override(1f);
            ao.color.Override(new Color(0,0,0));
            v = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, ao);

        }

        public void Update()
        {

            ao.intensity.value = GetValue() / 10;
            ao.color.value = new Color(GetValue() * 100, 0, 20);


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

        void OnDestroy()
        {
            RuntimeUtilities.DestroyVolume(v, true, true);
        }
    }
}