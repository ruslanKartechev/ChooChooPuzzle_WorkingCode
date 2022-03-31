using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;




namespace PuzzleGame
{
    public class UnitSplineController : MonoBehaviour
    {
        public Transform NodesSource;
        public List<Transform> Nodes = new List<Transform>();
        [Header("components")]
        public UnitSplineBuilder _builder;
        public void InitSpline()
        {
            IPointsSource source = NodesSource.GetComponent<IPointsSource>();
            if(source != null)
                Nodes = source.GetPoints();
            List<Transform> pos = new List<Transform>();
            foreach(Transform node in Nodes)
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
        }


    }



}