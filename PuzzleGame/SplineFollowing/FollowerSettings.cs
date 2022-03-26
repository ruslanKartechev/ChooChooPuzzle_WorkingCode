using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    [System.Serializable]
    public class FollowerSettings
    {
        public bool DebugMessages = false;
        public float moveSpeed = 15;
        public float totalSnapTime = 0.3f;
        public float snapPercent = 0.9f;
    }
}