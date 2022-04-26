using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
namespace PuzzleGame
{
    [System.Serializable]
    public class SplineChainMoveSettings
    {
        public float MoveSpeed = 5;
        public float DirChangeThreshold = 30f;
        [Space(10)]
        public float SplineMaxDist = 0.01f;
        public float SplineChangeMinDist = 0.5f;
        public List<SplineComputer> AvailableSplines = new List<SplineComputer>();

    }
    [System.Serializable]
    public class SpaghettiSegmentSettings
    {
        public float RandomizeRange = 0.1f;
        public float WiggleTime = 0.5f;
        public float TiltHeight = 0.5f;
    }

}