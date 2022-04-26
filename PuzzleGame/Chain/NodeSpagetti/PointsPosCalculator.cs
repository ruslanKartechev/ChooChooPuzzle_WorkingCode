using System;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;
namespace PuzzleGame
{
    public class PointsPosCalculator
    {
        private SplineComputer _spline;
        public float VerticalOffset { get; set; }
        public PointsPosCalculator(SplineComputer spline)
        {
            _spline = spline;
        }
        public void SetSpline(SplineComputer spline)
        {
            _spline = spline;
        }
        public List<Vector3> GetPointPositions(int num)
        {
            if (_spline == null) { Debug.LogError("Spline was not assigned"); return null; }
            List<Vector3> result = new List<Vector3>(num);
            double percent = 0f;
            float spacing = (float)1 / (num - 1);
            for (int i = 0; i <= num - 1; i++)
            {
                Vector3 pos = _spline.Evaluate(percent).position;
                pos.y += VerticalOffset;
                percent += spacing;
                result.Add(pos);
            }
            return result;
        }
    }
}
