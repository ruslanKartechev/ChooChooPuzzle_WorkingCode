using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonGame
{
    [CreateAssetMenu(fileName = "CamShakeSettings", menuName = "Camera/CamShakeSettings", order = 1)]
    public class CamShakeSettings : ScriptableObject
    {
        public float Duration;
        public float Magnitude;
    }
}