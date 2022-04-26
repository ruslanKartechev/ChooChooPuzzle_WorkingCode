using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using Obi;
namespace PuzzleGame
{
    public class StretchSegmentData : MonoBehaviour
    {
   
        [Header("Spline")]
        public SplineComputer Spline;
        [Header("Model")]
        public SplineMesh splineMesh;
      //  public ObiRope Rope;
        [Space(10)]
        //public int PivotPointsCount = 6;
        public float VerticalOffset = 0.25f;
        public List<Transform> Points = new List<Transform>();
        public List<ChainLink> _Links = new List<ChainLink>();
    }
}