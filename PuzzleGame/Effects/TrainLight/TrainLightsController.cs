using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    public class TrainLightsController : MonoBehaviour
    {
        [SerializeField] private TrainLightComponents _Components;
        [SerializeField] private LightEffectExecutioner _executioner;
        private void Start()
        {
           // InitAll();
        }
        public void InitAll()
        {
            List<Light> lights = new List<Light>();
            lights.Add(_Components.left);
            lights.Add(_Components.right);
            LightColorHandler colorHandler = new LightColorHandler();
            LightsBlinker blinker = gameObject.AddComponent<LightsBlinker>();
            LightsIntensityChanger intensityHandler = gameObject.AddComponent<LightsIntensityChanger>();
            _Components.ColorHandler = colorHandler;
            _Components.BlinkingHandler = blinker;
            _Components.IntensityHandler = intensityHandler;

            colorHandler.Init(_Components._Settings._ColorSettings, lights);
            blinker.Init(_Components._Settings._BlinkSettings, lights);
            intensityHandler.Init(_Components._Settings, lights);

            _executioner = GetComponent<LightEffectExecutioner>();
            _executioner.Init(_Components);
        }


        public void OnClick()
        {
            _executioner.ExecuteHardCommand(TrainLightCommands.TurnOn);
        }
        public void OnRelease()
        {
            _executioner.ExecuteEffect(TrainLightEffects.Disable);
        }
        public void OnStart()
        {
            _executioner.ExecuteEffect(TrainLightEffects.LongBlink);
        }

        public void OnSuccess()
        {
            _executioner.ExecuteEffect(TrainLightEffects.Enable);
            _executioner.ExecuteEffect(TrainLightEffects.Success);
        }
        public void OnCollision()
        {
            _executioner.ExecuteEffect(TrainLightEffects.BlinkWarning);
        }

    }
}
