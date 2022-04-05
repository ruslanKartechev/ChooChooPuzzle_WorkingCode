using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
namespace PuzzleGame
{
    public class LightColorHandler
    {
        private ColorSettings _settings;
        private List<Light> lights = new List<Light>();
        public void Init(ColorSettings settings, List<Light> lights)
        {
            this.lights = lights;
            _settings = settings;
        }
        public void SetNormalColor()
        {
            SetColor(_settings.normalColor);
        }
        public void SetWarningColor()
        {
            SetColor(_settings.warningColor);
        }
        public void SetSuccessColor()
        {
            SetColor(_settings.successColor);
        }
        public void SetColor(Color color)
        {
            foreach (Light l in lights)
                l.color = color;
        }
    }
}