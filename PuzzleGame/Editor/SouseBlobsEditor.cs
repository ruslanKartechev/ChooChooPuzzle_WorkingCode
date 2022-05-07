using UnityEditor;
using UnityEngine;

namespace PuzzleGame
{
    [CustomEditor(typeof(SouseBlobs))]
    public class SouseBlobsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SouseBlobs me = target as SouseBlobs;
            if (GUILayout.Button("SavePositions"))
            {
                me.Init();
                me.SavePositions();
                
            }
            if (GUILayout.Button("ApplyPositions"))
            {
                me.Init();
                me.UpdatePositions();
            }
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}