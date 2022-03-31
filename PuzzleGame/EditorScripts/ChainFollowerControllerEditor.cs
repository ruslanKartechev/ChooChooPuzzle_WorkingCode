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
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("GetFollowers"))
                me.GetFollowers();
            if (GUILayout.Button("SetChain"))
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
