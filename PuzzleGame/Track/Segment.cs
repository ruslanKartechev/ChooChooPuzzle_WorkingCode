
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    public class Segment
    {
        public SplineNode start;
        public SplineNode end;
        public float currentPercent;
        private Vector3 _direction;
        private float _distance;
        public Vector3 Dir { get { return _direction; } }
        public float Distance { get { return _distance; } }

        public Segment()
        {

        }
        public Segment(SplineNode _start, SplineNode _end)
        {
            currentPercent = 0;
            start = _start;
            end = _end;
            if(start == null) { Debug.Log("Start node is null"); return; }
            if(end == null) { Debug.Log("End node is null");return; }
            _direction = (end.transform.position - start.transform.position).normalized;
            _distance = (end.transform.position - start.transform.position).magnitude;
        }
    }
}