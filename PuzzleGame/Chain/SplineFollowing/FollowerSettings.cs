using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    [System.Serializable]
    public class FollowerSettings
    {
        public bool DebugMessages = false;
        public float inputThreshold = 1;
        public float TesterSpeed = 5;
        public float NodeSnapPercent = 1f;
        [Header("Movement")]
        public bool UseAcceleration = true;
        public float accelerationTime = 0.5f;
        [Range(0f,1f)]
        public float startModifier = 0.5f;
        [Range(0f, 1f)]
        public float LeadModifier = 1.2f;
        public float NodeMovingTime = 0.3f;
        [Header("Angle to push")]
        public float PushAngleThreshold = 95;
        [Space(10)]
        public LayerMask castMask;
        public string LeadTag;
        [Space(10)]
        public float bouncingPercent = 0.7f;
        public float bounceTime = 0.3f;
    }
}