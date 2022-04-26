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

            GUILayout.Space(10);
            GUILayout.Label("PRESS THIS TO INIT TRAIN");
            if (GUILayout.Button("FullInit"))
            {
                me.GetFollowers();
                me.ConnectChainFollowers();
                me.GetChainSegments();
                me.SetNodes();
                me.SetChainPositions();
            }
            GUILayout.Space(10);
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



//private static SplineNode TryFindHorizontal(Vector2 input, SplineNode from, List<SplineNode> nodes, List<SplineNode> exclude)
//{
//    List<SplineNode> clearedOptions = ClearOptions(nodes, exclude);
//    SplineNode result = FindFromHor(input, from.transform, clearedOptions);
//    if (result == null)
//    {
//        //Debug.Log("DID NOT FIND FROM CLEARED LIST");
//        result = FindFromHor(input, from.transform, nodes);
//    }
//    return result;
//}

//private static SplineNode TryFindVertical(Vector2 input, SplineNode from, List<SplineNode> nodes, List<SplineNode> exclude)
//{
//    List<SplineNode> clearedOptions = ClearOptions(nodes, exclude);
//    SplineNode result = FindFromVert(input, from.transform, clearedOptions);
//    if (result == null)
//    {
//        //Debug.Log("DID NOT FIND FROM CLEARED LIST");
//        result = FindFromVert(input, from.transform, nodes);
//    }
//    return result;
//}