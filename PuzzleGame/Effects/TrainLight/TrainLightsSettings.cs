using System.Collections;
using UnityEngine;

namespace PuzzleGame
{
    [System.Serializable]
    public class ColorSettings
    {
        public Color normalColor;
        public Color warningColor;
        public Color successColor;
    }
    [System.Serializable]

    public class BlinkingSettings
    {
        public int MildBlinkCount = 2;
        public float MildBlinkDelay = 0.4f;
        [Space(10)]
        public int WarningBlinkCount = 4;
        public float WarningBlinkDelay = 0.15f;

    }
    [System.Serializable]

    public class TrainLightSettings
    {
        [Header("Intensities")]
        [Space(10)]
        public float NormalIntensity;
        [Header("Hide/Show time")]
        [Space(10)]
        public float SwitchTime = 0.4f;

        [Header("Blinking")]
        [Space(10)]
        public BlinkingSettings _BlinkSettings;

        [Header("Colors")]
        [Space(10)]
        public ColorSettings _ColorSettings;
    }
}