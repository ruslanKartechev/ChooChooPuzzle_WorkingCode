using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace PuzzleGame
{
    public class LightEffectExecutioner : MonoBehaviour
    {
        private TrainLightComponents _Components;
        private TrainLightSettings _settings;
       
        public LightColorHandler ColorsHandler;
        
        public void Init(TrainLightComponents components)
        {
            _Components = components;
            _settings = _Components._Settings;
        }
        public void ExecuteEffect(TrainLightEffects effect)
        {
            switch (effect)
            {
                case TrainLightEffects.Enable:
                    Show();
                    break;
                case TrainLightEffects.Disable:
                    Hide();
                    break;
                case TrainLightEffects.BlinkMild:
                    BlinkMild();
                    break;
                case TrainLightEffects.BlinkWarning:
                    BlinkWarning();
                    break;
                case TrainLightEffects.LongBlink:
                    LongBlink();
                    break;
                case TrainLightEffects.Success:
                    Success();
                    break;
            }
        }

        public void ExecuteHardCommand(TrainLightCommands command)
        {
            switch (command)
            {
                case TrainLightCommands.TurnOn:
                    _Components.IntensityHandler.AllOn();
                    break;
                case TrainLightCommands.TurnOff:
                    _Components.IntensityHandler.AllOff();
                    break;
            }
        }

        private void Show()
        {
            _Components.ColorHandler.SetNormalColor();
            _Components.IntensityHandler.SetIntensityAll(0f);
            _Components.IntensityHandler.Show(null);
        }
        private void Hide()
        {
            _Components.IntensityHandler.Hide(_Components.IntensityHandler.AllOff);
        }
        private void LongBlink()
        {
            _Components.ColorHandler.SetNormalColor();
            _Components.BlinkingHandler.AllOn();
            _Components.BlinkingHandler.Wait(1f, _Components.BlinkingHandler.AllOff);
        }
        private void BlinkMild()
        {
            _Components.IntensityHandler.ResetNormal();
            _Components.BlinkingHandler.BlinkMild(null);
        }
        private void BlinkWarning()
        {
            _Components.IntensityHandler.ResetNormal();
            _Components.ColorHandler.SetWarningColor();
            _Components.BlinkingHandler.BlinkWarning(
                _Components.ColorHandler.SetNormalColor);

        }
        private void Success()
        {
            _Components.IntensityHandler.AllOn();
            _Components.IntensityHandler.ResetNormal();
            _Components.ColorHandler.SetSuccessColor();
        }


    }
}