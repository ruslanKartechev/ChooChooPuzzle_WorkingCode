using UnityEditor;
using UnityEngine;

namespace PuzzleGame
{
    [CustomEditor(typeof(PathBranchPointModels))]
    public class PathBranchPointModelsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            PathBranchPointModels me = target as PathBranchPointModels;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("SetModels"))
            {
                me.GetModels();
                me.ResetModel();
            }
            if (GUILayout.Button("ScaleModels"))
            {
                me.GetModels();
                me.ScaleModels();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("ShowModels"))
            {
                me.GetModels();
                me.ShowPoints();
            }
            if (GUILayout.Button("HideModels"))
            {
                me.GetModels();
                me.HidePoints();
            }
            GUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();

        }

    }
}
