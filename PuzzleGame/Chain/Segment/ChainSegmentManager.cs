using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PuzzleGame
{

    
    public class ChainSegmentManager : MonoBehaviour, IChainSegment
    {
        public Transform pivot_1;
        public Transform pivot_2;

        public ChainLinksPositioner _Positioner;
        [Space(10)]
        public List<ChainLink> ChainLinks = new List<ChainLink>();
        private ChainController _controller;


        public void InitSegment(ChainController controller)
        {
            InitCuttableLinks();
            _controller = controller;

        }
        private void InitCuttableLinks()
        {
            foreach(ChainLink link in ChainLinks)
                link.OnCutCall = OnLinkCutStart;
        }

        public void OnLinkCutStart(ChainLink caller)
        {
            _controller.Cut(this, caller);
        }




        public void StartChainMovement()
        {
            if(_Positioner == null) { Debug.Log("No positioner set"); return ; }
            
            _Positioner.StartMovement(new ChainLinksInfo(ChainLinks), pivot_1, pivot_2);

        }
        public void StopChainMovement()
        {
            _Positioner.StopMovement();
        }
        public ChainLinksInfo GetChainInfo()
        {
            return new ChainLinksInfo(ChainLinks);
        }

        public void GetLinks()
        {
            ChainLinks = new List<ChainLink>();
            for (int i = 0; i < transform.childCount; i++)
            {
                ChainLink link = transform.GetChild(i).gameObject.GetComponent<ChainLink>();
                if(link != null)
                    link.transform.rotation = Quaternion.LookRotation(Vector3.right);
                if (link != null && ChainLinks.Contains(link) == false)
                {
                    ChainLinks.Add(link);
                    link.gameObject.name = "link_" + i.ToString();
                }
            }
        }

        public void SetPositions()
        {
            if (_Positioner == null || pivot_1 == null || pivot_2 == null || ChainLinks.Count == 0)
                return;
            transform.position = pivot_1.transform.position;
            _Positioner.SetPositionsForced(new ChainLinksInfo(ChainLinks), pivot_1, pivot_2);

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