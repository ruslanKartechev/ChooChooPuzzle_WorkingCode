using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;




namespace PuzzleGame
{
    public class UnitSplineController : MonoBehaviour
    {
        public List<SplineNode> Nodes = new List<SplineNode>();
        [Header("components")]
        public UnitSplineBuilder _builder;
        public UnitSplineFollower _follower;

        public void InitSpline()
        {
            List<Transform> pos = new List<Transform>();
            foreach(SplineNode node in Nodes)
            {
                if(node != null)
                {
                    pos.Add(node.transform);
                }
            }
            if(_builder != null)
            {
                _builder.SetNodes(pos);
                _builder.InitSpline();
                _builder.UpdateSpline();
            }

            _follower.SetCurrentNode(Nodes[0]);
            //_follower.SetSpline(_builder.spline);

        }


    }



#if UNITY_EDITOR
    [CustomEditor(typeof(UnitSplineController))]
    public class UnitSplineControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            UnitSplineController me = target as UnitSplineController;
            if (GUILayout.Button("Init"))
            {
                me.InitSpline();
            }
        }

    }
#endif
}