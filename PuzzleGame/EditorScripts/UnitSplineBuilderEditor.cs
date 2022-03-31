using UnityEngine;
using UnityEditor;

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
            serializedObject.ApplyModifiedProperties();
        }
    }
}
