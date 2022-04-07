using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace PuzzleGame
{
    public class LightEffectExecutioner : MonoBehaviour
    {
        private TrainLightComponents _Components;
        public LightColorHandler ColorsHandler;
        
        public void Init(TrainLightComponents components)
        {
            _Components = components;
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
                    _Components.BlinkingHandler.AddCommand(new BlinkerCommand(BlinkerCommandName.AllOn, null));
                    break;
                case TrainLightCommands.TurnOff:
                    _Components.BlinkingHandler.AddCommand(new BlinkerCommand(BlinkerCommandName.AllOff, null));
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
            //_Components.IntensityHandler.Hide(_Components.IntensityHandler.AllOff);
            _Components.BlinkingHandler.AddCommand(new BlinkerCommand(BlinkerCommandName.AllOff, ResetNormal));
        }
        private void LongBlink()
        {
            _Components.ColorHandler.SetNormalColor();
            _Components.BlinkingHandler.AddCommand(new BlinkerCommand(BlinkerCommandName.AllOn, null));
            _Components.BlinkingHandler.AddCommand(new BlinkerCommand(BlinkerCommandName.Wait, null));
            _Components.BlinkingHandler.AddCommand(new BlinkerCommand(BlinkerCommandName.AllOff, null));

        }
        private void BlinkMild()
        {
            _Components.IntensityHandler.ResetNormal();
            _Components.BlinkingHandler.AddCommand(new BlinkerCommand(BlinkerCommandName.BlinkHard, ResetNormal));

        }
        private void BlinkWarning()
        {
            _Components.IntensityHandler.SetIntensityAll(_Components._Settings.NormalIntensity*2);
            _Components.ColorHandler.SetWarningColor();
            _Components.BlinkingHandler.AddCommand(new BlinkerCommand(BlinkerCommandName.BlinkHard,ResetNormal) );
        }
        private void Success()
        {
            _Components.BlinkingHandler.AddCommand(new BlinkerCommand(BlinkerCommandName.AllOn, null));
            _Components.IntensityHandler.SetIntensityAll(_Components._Settings.NormalIntensity * 2);
            _Components.ColorHandler.SetSuccessColor();
        }
        public void ResetNormal()
        {
            _Components.IntensityHandler.SetIntensityAll(_Components._Settings.NormalIntensity);
            _Components.ColorHandler.SetNormalColor();

        }

    }
}