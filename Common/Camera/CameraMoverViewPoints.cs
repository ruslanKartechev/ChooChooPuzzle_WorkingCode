using System;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    [System.Serializable]
    public class CameraMoverViewPoints
    {
        public float ControllPointsMoveTime;
        public List<CamContollPoint> pointByNumber;
    }


    [System.Serializable]
    public class CamContollPoint
    {
        public ChainNumber _number;
        public Transform _point;
        public CamContollPoint(Transform target, ChainNumber num)
        {
            _point = target;
            _number = num;
        }
    }
}
