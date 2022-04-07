using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    public class TrainLightComponents : MonoBehaviour
    {
        public Light left;
        public Light right;
        [Header("Settings")]
        public TrainLightSettings _Settings;
        [Header("auto set")]
        [Space(10)]
        //public TrainEffectExecutioner Executioner;
        public LightColorHandler ColorHandler;
        public LightsBlinker BlinkingHandler;
        public LightsIntensityChanger IntensityHandler;



    }
}