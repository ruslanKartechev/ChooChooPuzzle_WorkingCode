using UnityEditor;
using UnityEngine;
namespace PuzzleGame
{

#if UNITY_EDITOR
    [CustomEditor(typeof(LinksSegmentManager))]
    public class ChainSegmentManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            LinksSegmentManager me = target as LinksSegmentManager;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("GetLinks"))
            {
                me.GetLinks();
            }
            if (GUILayout.Button("SetPositions"))
            {
                me.GetLinks();
                me.UpdateSegment();
            }
               
            GUILayout.EndHorizontal();
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
