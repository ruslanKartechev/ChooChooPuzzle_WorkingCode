using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PuzzleGame
{
    [CustomEditor(typeof(PathBranch))]
    public class PathBranchEditor : Editor
    {
        PathBranch me;

        private void OnEnable()
        {
            me = (PathBranch)target;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PathBranch me = target as PathBranch;
            GUILayout.BeginHorizontal();

            GUILayout.Space(10);
            if (GUILayout.Button("AddPoint"))
                me.AddNode();
            if (GUILayout.Button("SetZeroDepth"))
                me.SetDepth();

            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.Label("Spacing Nodes equally from first or second nodes");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("SpaceFrom_F"))
                me.SpaceFromFirst();
            if (GUILayout.Button("SpaceFrom_L"))
                me.SpaceFromLast();
            GUILayout.EndHorizontal();


            GUILayout.Space(10);
            GUILayout.Label("Centering Parent Pivot");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("CenterAround_F"))
                me.CenterAround_F();
            if (GUILayout.Button("CenterAround_L"))
                me.CenterAround_L();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("RenameAll"))
                me.ReName();
            GUILayout.Space(10);
            if (GUILayout.Button("SetNeighbours"))
                me.SetNodeNeighbours();

            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();

        }


        private void OnSceneGUI()
        {
            me.UpdatePoints();
            DrawLinks(me._transforms);
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }


        public void DrawLinks(List<Transform> points)
        {
            Handles.color = Color.red;
            for (int i = 1; i < points.Count; i++)
            {
                Vector3[] ends = new Vector3[2];
                ends[0] = points[i - 1].position;
                ends[1] = points[i].position;
                Handles.DrawAAPolyLine(10, ends);
            }
        }

    }
}
