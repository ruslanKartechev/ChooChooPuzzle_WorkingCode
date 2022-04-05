using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace PuzzleGame
{
    public class LightsBlinker : MonoBehaviour
    {
        //private TrainLightComponents _components;
        private BlinkingSettings _settings;
        private List<Light> lights = new List<Light>();
        private Coroutine _blinking;
        private Coroutine _waiting;

        public void Init(BlinkingSettings settings, List<Light> lights)
        {
            this.lights = lights;
            _settings = settings;
        }
        public void BlinkMild(Action onEnd)
        {
            Blink(_settings.MildBlinkCount, _settings.MildBlinkDelay, onEnd);
        }
        public void BlinkWarning(Action onEnd)
        {
            Blink(_settings.WarningBlinkCount, _settings.WarningBlinkDelay, onEnd);
        }
        public void Blink(int count, float delay, Action onEnd)
        {
            if (_blinking != null) StopCoroutine(_blinking);
            _blinking = StartCoroutine(BlinkingHandler(lights, count, delay, onEnd));
        }

        private IEnumerator BlinkingHandler(List<Light> _lights, int count, float delay, Action onEnd)
        {
            int i = 0;
            float elapsed = 0f;
            bool offOnEnd = false;
            if (_lights[0].enabled == false)
            {
                offOnEnd = true;
            }
            AllOn();
            while (i < count)
            {
                if (elapsed >= delay)
                {
                    elapsed = 0f;
                    i++;
                    if (i % 2 == 0)
                    {
                        AllOn();
                    }
                    else
                    {
                        AllOff();
                    }
                }
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (offOnEnd == true)
                AllOff();
            onEnd?.Invoke();
        }


        public void Wait(float time, Action onEnd)
        {
            if (_waiting != null) StopCoroutine(_waiting);
            _waiting = StartCoroutine(Waiting(time, onEnd));
        }
        private IEnumerator Waiting(float time, Action onEnd)
        {
            yield return new WaitForSeconds(time);
            onEnd?.Invoke();
        }




        public void AllOn()
        {
            foreach (Light l in lights)
                l.enabled = true;
        }
        public void AllOff()
        {
            foreach (Light l in lights)
                l.enabled = false;
        }

    }
}