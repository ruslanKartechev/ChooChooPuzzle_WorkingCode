using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PuzzleGame
{
    public class ChainSegmentManager : MonoBehaviour
    {
        public Transform pivot_1;
        public Transform pivot_2;

        public ChainLinksPositioner _Positioner;
        [Space(10)]
        public List<ChainLink> ChainLinks = new List<ChainLink>();
        
        public void StartChainMovement()
        {
            if(_Positioner == null) { Debug.Log("No positioner set"); return ; }
            _Positioner.StartMovement(ChainLinks, pivot_1, pivot_2);

        }
        public void StopChainMovement()
        {
            _Positioner.StopMovement();
        }


        public void GetLinks()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                ChainLink link = transform.GetChild(i).gameObject.GetComponent<ChainLink>();
                if(link != null && ChainLinks.Contains(link) == false)
                {
                    ChainLinks.Add(link);
                }
            }
        }

        public void SetPositions()
        {
            if (_Positioner == null || pivot_1 == null || pivot_2 == null || ChainLinks.Count == 0)
                return;
            _Positioner.SetPositionsForced(ChainLinks, pivot_1, pivot_2);

        }

    }


    [CustomEditor(typeof(ChainSegmentManager))]
    public class ChainSegmentManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ChainSegmentManager me = target as ChainSegmentManager;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("GetLinks"))
            {
                me.GetLinks();
            }
            if (GUILayout.Button("SetPositions"))
                me.SetPositions();
            GUILayout.EndHorizontal();
        }

    }
}