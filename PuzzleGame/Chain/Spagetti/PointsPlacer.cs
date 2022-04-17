using System;
using System.Collections.Generic;
using UnityEngine;
namespace PuzzleGame
{
    public class PointsPlacer
    {
        private PointsPosCalculator _calculator;
        private List<Transform> points;
        public List<Transform> Points { get { return points; } }
        private int PointsCount { get { if (points != null) return points.Count; else return 0; } }

        float Yoffset = 0.15f;
        public PointsPlacer(PointsPosCalculator calculator)
        {
            _calculator = calculator;
            points = new List<Transform>();
        }
        public void SetPoints(List<Transform> _points)
        {
            points = _points;
        }
        public void SpawnPoints(int n, Transform parent, List<Transform> container)
        {
            for (int i = 0; i < n; i++)
            {
                Transform p = new GameObject(i.ToString()).transform;
                points.Add(p);
                p.parent = parent;
                container.Add(p);
            }
        }

        public void UpdatePositions()
        {
            if (_calculator == null) { Debug.LogError("Calculator not assigned"); return; }
            List<Vector3> positions = _calculator.GetPointPositions(PointsCount);
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i] != null)
                    points[i].position = positions[i] + new Vector3(0, Yoffset, 0);
            }
        }
    }
}
