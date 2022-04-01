using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
namespace PuzzleGame
{
    [CustomEditor(typeof(FinishNode))]
    public class FinishNodeEditor : Editor
    {
        SplineNode me;
        private void OnEnable()
        {
            me = target as FinishNode;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            FinishNode me = target as FinishNode;
            LinksDrawer.DrawLinks(me);



            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();

        }
        private void OnSceneGUI()
        {
            if(me != null)
                LinksDrawer.DrawLinks(me);
        }

    }
}
#endif
