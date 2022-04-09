using System.Collections;
using UnityEngine;

namespace PuzzleGame
{


    [System.Serializable]
    public class ChainEffectsSettings
    {
        public ChainEffect_Shaking shaking;
        public ChainEffect_Lean leaning;
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
    public class ChainEffect_Lean
    {
        public float LeanTime = 0.25f;
        public float RelapseTime = 0.15f;
        [Header("In percent of the length of the segment")]
        public float Amount = 0.2f;

    }
}
