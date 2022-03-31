using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Dreamteck.Splines;
namespace PuzzleGame
{



    [ExecuteInEditMode]
    public class UnitSplineBuilder : MonoBehaviour
    {
        public SplineComputer spline;
        [HideInInspector] protected List<Transform> Nodes = new List<Transform>();
        public void InitSpline()
        {
            if (spline == null)
                spline = GetComponent<SplineComputer>();
            if (spline == null) return;
            spline.type = Spline.Type.Linear;
            spline.space = SplineComputer.Space.World;
            //
        }


        public void SetNodes(List<Transform> points)
        {
            Nodes = points;
        }

        public void UpdateSpline()
        {
            if (spline == null)
                return;

            SplinePoint[] newPoints = new SplinePoint[Nodes.Count];
            for (int i=0; i<Nodes.Count; i++)
            {
                newPoints[i].position = Nodes[i].position;
                newPoints[i].normal = Vector3.up;
                newPoints[i].type = SplinePoint.Type.SmoothFree;
                newPoints[i].tangent = Vector3.right;
            }
            spline.SetPoints(newPoints);
            spline.Rebuild();
        }

    }
}