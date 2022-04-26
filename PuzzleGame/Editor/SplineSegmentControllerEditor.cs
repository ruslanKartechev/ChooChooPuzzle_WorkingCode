using UnityEditor;
using UnityEngine;

namespace PuzzleGame
{
#if UNITY_EDITOR
    [CustomEditor(typeof(SplineSegmentController))]
    public class SplineSegmentControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SplineSegmentController me = target as SplineSegmentController;
            if (GUILayout.Button("Init rope"))
            {
                me.Init();
            }
        }
    }
#endif
}