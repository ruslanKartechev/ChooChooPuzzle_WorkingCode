using System.Collections;
using UnityEngine;

namespace PuzzleGame
{


    [System.Serializable]
    public class ChainEffectsSettings
    {
        public ChainEffect_Shaking shaking;
        public ChainEffect_Squeezing squeezing;
        public ChainEffect_Start start;
        public ChainEffect_Stop stop;
    }




    [System.Serializable]
    public class ChainEffect_Shaking
    {
        public float ShakeTime;
        public float ShakeMagnitude;
    }
    [System.Serializable]
    public class ChainEffect_Start
    {

    }
    [System.Serializable]
    public class ChainEffect_Stop
    {

    }
    [System.Serializable]
    public class ChainEffect_Squeezing
    {
        public float SqueezeTime = 0.5f;
        public float ReleaseTime = 0.5f;
        public bool SqueezeWhole = false;

    }
}
