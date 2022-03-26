using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Dreamteck.Splines;
namespace PuzzleGame
{
    [CustomEditor(typeof(UnitSplineBuilder))]
    public class UnitSplineBuilderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            UnitSplineBuilder me = target as UnitSplineBuilder;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Update"))
            {
                me.UpdateSpline();
            }
            if (GUILayout.Button("InitSpline"))
                me.InitSpline();
            GUILayout.EndHorizontal();
        }
    }


    [ExecuteInEditMode]
    public class UnitSplineBuilder : MonoBehaviour
    {
        public SplineComputer spline;
        public List<Transform> Nodes = new List<Transform>();
        public void Start()
        {
        }

        public void SetSpline(SplineComputer spline)
        {
            this.spline = spline;
        }
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
                newPoints[i].type = SplinePoint.Type.SmoothFree;
                newPoints[i].color = Color.green;


            }
            spline.SetPoints(newPoints);
        }



        public void AddKnotFront(Transform point)
        {
            Nodes.Add(point);
            UpdateSpline();
        }
        public void AddKnotEnd(Transform point)
        {
            Nodes.Insert(0, point);
            UpdateSpline();
        }

        public void TrimFront()
        {
            Nodes.RemoveAt(Nodes.Count - 1);
        }
        public void TrimEnd()
        {
            Nodes.RemoveAt(0);
        }



    }
}