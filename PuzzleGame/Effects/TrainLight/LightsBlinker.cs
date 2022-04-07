using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
namespace PuzzleGame
{
    public enum BlinkerCommandName { AllOn, AllOff, BlinkMild, BlinkHard, Wait}
    public class BlinkerCommand
    {
        public BlinkerCommandName name;
        public Action onFinish;
        public BlinkerCommand(BlinkerCommandName name, Action onEnd)
        {
            this.name = name;
            onFinish = onEnd;
        }
    }
    
    public class LightsBlinker : MonoBehaviour
    {
        //private TrainLightComponents _components;
        private BlinkingSettings _settings;
        private List<Light> lights = new List<Light>();
        private Coroutine _blinking;
        private Coroutine _waiting;

        private Queue<BlinkerCommand> _commands = new Queue<BlinkerCommand>();
        public float WaitTime = 1f;

        private bool _isBlinking = false;
        private Action OnBlinkingEnd;

        public void Init(BlinkingSettings settings, List<Light> lights)
        {
            this.lights = lights;
            _settings = settings;
        }


        public void AddCommand(BlinkerCommand command, bool startExecution = true)
        {
            _commands.Enqueue(command);
            if(startExecution == true)
                ExecuteNextCommand();
        }


        public void ExecuteNextCommand()
        {
            if (_commands.Count == 0) { return;}
            BlinkerCommand next = _commands.Peek();
            switch (next.name)
            {
                case BlinkerCommandName.AllOn:

                    if(_isBlinking == true)
                    {
                        OnBlinkingEnd = AllOn;
                    }
                       AllOn();
                    break;
                case BlinkerCommandName.AllOff:
                    if (_isBlinking == true)
                    {
                        OnBlinkingEnd = AllOff;
                    } else 
                        AllOff();
                    break;
                case BlinkerCommandName.BlinkMild:
                    BlinkMild(next.onFinish);
                    break;
                case BlinkerCommandName.BlinkHard:
                    BlinkHard(next.onFinish);
                    break;
                case BlinkerCommandName.Wait:
                    Wait(WaitTime, next.onFinish);
                    break;
            }
        }

        public void OnCommandEnd(Action onEnd)
        {
            if(_commands.Count > 0)
                _commands.Dequeue();
            onEnd?.Invoke();
            ExecuteNextCommand();
        }


        public void BlinkMild(Action onFinish)
        {
            Blink(_settings.MildBlinkCount, _settings.MildBlinkDelay, onFinish);
        }

        public void BlinkHard(Action onFinish)
        {
            Blink(_settings.WarningBlinkCount, _settings.WarningBlinkDelay, onFinish);
        }

        public void Blink(int count, float delay, Action onEnd)
        {
            if (_blinking != null) StopCoroutine(_blinking);
            _blinking = StartCoroutine(BlinkingHandler(lights, count, delay, onEnd));
        }

        private IEnumerator BlinkingHandler(List<Light> _lights, int count, float delay, Action onEnd)
        {
            _isBlinking = true;
            int i = 0;
            float elapsed = 0f;
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
            _isBlinking = false;
            OnBlinkingEnd?.Invoke();
            OnCommandEnd(onEnd);
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
            OnCommandEnd(onEnd);
        }


        public void AllOn()
        {
            foreach (Light l in lights)
                l.enabled = true;
            OnCommandEnd(null);
        }

        public void AllOff()
        {
            
            foreach (Light l in lights)
                l.enabled = false;
            OnCommandEnd(null);
        }

    }
}