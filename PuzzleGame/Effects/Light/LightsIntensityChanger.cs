using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
namespace PuzzleGame
{
    public class LightsIntensityChanger : MonoBehaviour
    {
        private TrainLightSettings _settings;
        private List<Light> lights = new List<Light>();

        private Coroutine _intensityChange;
        public void Init(TrainLightSettings settings, List<Light> lights )
        {
            this.lights = lights;
            _settings = settings;
        }
        public void Show(Action onEnd)
        {
            if (_intensityChange != null) StopCoroutine(_intensityChange);
            AllOn();
            _intensityChange = StartCoroutine(IntensityChaning(_settings.SwitchTime, _settings.NormalIntensity, lights, onEnd));
        }
        public void Hide(Action onEnd)
        {
            if (_intensityChange != null) StopCoroutine(_intensityChange);
            _intensityChange = StartCoroutine(IntensityChaning(_settings.SwitchTime, 0, lights, onEnd));
        }

        private IEnumerator IntensityChaning(float time, float endVal, List<Light> _lights, Action onEnd)
        {
            float elapsed = 0f;
            float startVal = _lights[0].intensity;
            while (elapsed <= time)
            {
                float val = Mathf.Lerp(startVal, endVal, elapsed / time);
                SetIntensity(_lights, val);
                elapsed += Time.deltaTime;
                yield return null;
            }
            onEnd?.Invoke();
        }


        public void ResetNormal()
        {
            foreach (Light l in lights)
                l.intensity = _settings.NormalIntensity;
        }
        private void SetIntensity(List<Light> _lights, float val)
        {
            foreach (Light l in _lights)
            {
                l.intensity = val;
            }
        }
        public void SetIntensityAll(float val)
        {
            SetIntensity(lights, val);
        }
        public void AllOn()
        {
            foreach (Light l in lights)
            {
                l.enabled = true;
                l.intensity = _settings.NormalIntensity;
            }
        }
        public void AllOff()
        {
            foreach (Light l in lights)
                l.enabled = false;
        }
    }
}