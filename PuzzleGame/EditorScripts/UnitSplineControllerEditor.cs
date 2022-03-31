using UnityEditor;
using UnityEngine;

namespace PuzzleGame
{

#if UNITY_EDITOR
    [CustomEditor(typeof(UnitSplineController))]
    public class UnitSplineControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            UnitSplineController me = target as UnitSplineController;
            if (GUILayout.Button("Init"))
            {
                me.InitSpline();
            }
            serializedObject.ApplyModifiedProperties();
        }

    }
#endif
}
