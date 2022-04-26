using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace PuzzleGame
{
    [CustomEditor(typeof(SplineChainController))]
    public class SplineChainControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SplineChainController me = target as SplineChainController;
            if (GUILayout.Button("InitStart"))
            {
                me.InitEditor();
            }
        }
    }
}