using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
namespace PuzzleGame
{
    public abstract class SpaghettiSegmentBase : MonoBehaviour
    {
        [SerializeField] protected Transform _segPivot_1;
        [SerializeField] protected Transform _segPivot_2;

        public abstract void Init(object settings);
        public abstract void SetSpline(SplineComputer spline);
        public abstract void UpdateSegment();
    }
}