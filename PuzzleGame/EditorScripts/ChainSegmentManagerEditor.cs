using UnityEditor;
using UnityEngine;
namespace PuzzleGame
{

#if UNITY_EDITOR
    [CustomEditor(typeof(ChainSegmentManager))]
    public class ChainSegmentManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ChainSegmentManager me = target as ChainSegmentManager;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("GetLinks"))
            {
                me.GetLinks();
            }
            if (GUILayout.Button("SetPositions"))
                me.SetPositions();
            GUILayout.EndHorizontal();
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
