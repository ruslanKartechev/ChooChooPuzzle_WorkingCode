using UnityEngine;
using UnityEditor;

namespace PuzzleGame
{
    [CustomEditor(typeof(ChainController))]
    public class ChainFollowerControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ChainController me = target as ChainController;

            if (GUILayout.Button("FullInit"))
            {
                me.GetFollowers();
                me.ConnectChainFollowers();
                me.GetChainSegments();
                me.SetNodes();
                me.SetChainPositions();
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("GetFollowers"))
                me.GetFollowers();
            if (GUILayout.Button("Connect"))
                me.ConnectChainFollowers();
            GUILayout.EndHorizontal();
            if (GUILayout.Button("SetNodes"))
                me.SetNodes();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("GetSegments"))
                me.GetChainSegments();
            if (GUILayout.Button("SetChainPositions"))
                me.SetChainPositions();
            GUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
