using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PuzzleGame
{

#if UNITY_EDITOR
    [CustomEditor(typeof(SegmentsModelsManager))]
    public class SegmentsModelsManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SegmentsModelsManager me = target as SegmentsModelsManager;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("GetModels"))
                me.GetAllModels();
            if (GUILayout.Button("InitModels"))
            {
                me.GetAllModels();
                me.InitModels();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Scale"))
            {
                me.GetAllModels();
                me.ScaleModels();
            }

            if (GUILayout.Button("ApplyRotation"))
            {
                me.GetAllModels();
                me.ApplyRotation();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(15);
            GUILayout.Label("All current links will be destroyd. New Will be spawnd with models from the list");
            if(GUILayout.Button("Spawn New Links From List"))
            {
                me.SetNewModels();
                me.GetAllModels();
                me.InitModels();
                me.ScaleModels();
                me.ApplyRotation();
            }
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }

    }
#endif
}
