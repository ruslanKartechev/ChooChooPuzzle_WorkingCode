using UnityEditor;
using UnityEngine;
namespace PuzzleGame
{

#if UNITY_EDITOR
    [CustomEditor(typeof(LinksSegmentController))]
    public class ChainSegmentManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            LinksSegmentController me = target as LinksSegmentController;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("GetLinks"))
            {
                me.GetLinks();
            }
            if (GUILayout.Button("SetPositions"))
            {
                me.GetLinks();
                me.SetPositions();
            }
               
            GUILayout.EndHorizontal();
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
