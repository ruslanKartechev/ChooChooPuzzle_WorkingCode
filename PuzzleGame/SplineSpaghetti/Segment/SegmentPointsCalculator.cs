using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
namespace PuzzleGame
{
    public class SegmentPointsCalculator
    {
        public SplineComputer CurrentSpline;
        public int PointCount;
        public float EndPointsVertOffset;
        public float AllPointsVertOffset;
        public List<Vector3> GetSplinePositions(float leftPercent, float rightPercent)
        {
            if(CurrentSpline == null)
            {
                Debug.Log("current spline not asigned");
                return null;
            }
            List<Vector3> result = new List<Vector3>(PointCount);
            Vector3 leftEnd = CurrentSpline.EvaluatePosition(leftPercent) + Vector3.up * (EndPointsVertOffset+ AllPointsVertOffset);
            Vector3 rightEnd = CurrentSpline.EvaluatePosition(rightPercent) + Vector3.up * (EndPointsVertOffset + AllPointsVertOffset);
            result.Add(leftEnd);
            float percentDelta = (rightPercent - leftPercent) / (PointCount - 1);
            for (int i = 1; i < PointCount - 1; i++)
            {
                float p = leftPercent + percentDelta * i;
                result.Add(CurrentSpline.EvaluatePosition(p)+Vector3.up * AllPointsVertOffset);
            }
            result.Add(rightEnd);
            return result;
        }
        public List<Vector3> GetSplinePositions(Transform leftPivot, Transform rightPivot)
        {
            if (CurrentSpline == null)
            {
                Debug.Log("Current Spline not assigned");
                return null;
            }
            float l = (float)CurrentSpline.Project(leftPivot.position).percent;
            float r = (float)CurrentSpline.Project(rightPivot.position).percent;
            return GetSplinePositions(l, r);
        }

    }
}