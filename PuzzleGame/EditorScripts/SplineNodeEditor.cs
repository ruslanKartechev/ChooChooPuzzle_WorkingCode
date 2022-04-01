using UnityEditor;
using UnityEngine;
using CommonGame;
namespace PuzzleGame
{

#if UNITY_EDITOR
    [CustomEditor(typeof(SplineNode))]
    public class SplineNodeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SplineNode me = target as SplineNode;

            LinksDrawer.DrawLinks(me);

            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();

        }

        private void OnSceneGUI()
        {
            SplineNode me = target as SplineNode;
            LinksDrawer.DrawLinks(me);
        }
    }
#endif


}
