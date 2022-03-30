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
        public List<ChainLink> _Links = new List<ChainLink>();
        private ChainController _controller;


        public void InitSegment(ChainController controller)
        {
            Debug.Log("init segment");
            InitCuttableLinks();
            _controller = controller;

        }
        private void InitCuttableLinks()
        {
            foreach (ChainLink link in _Links)
                link.InitOnCutReciver(OnLinkCutStart);
        }

        public void OnLinkCutStart(ChainLink caller)
        {
            _controller.Cut(this, _Links.IndexOf(caller));
        }

        public void StartChainMovement()
        {
            if(_Positioner == null) { Debug.Log("No positioner set"); return ; }
            
            _Positioner.StartMovement(new ChainLinksInfo(_Links), pivot_1, pivot_2);

        }
        public void StopChainMovement()
        {
            _Positioner.StopMovement();
        }
        public ChainLinksInfo GetChainInfo()
        {
            return new ChainLinksInfo(_Links);
        }

        public void DropAllLinks()
        {
            for (int i = 0; i < _Links.Count; i++)
            {
                if(_Links[i] != null)
                    _Links[i].Drop();
            }
            _Links = new List<ChainLink>();
        }
        public void DropLinks(List<int> indeces)
        {
            if (indeces.Count == 0) return;
            Debug.Log("dropping " + indeces.Count + " links");
            foreach(int i in indeces)
            {
                //Debug.Log("<color=red>" + "index: " + i + "</color>");

                if (_Links[i] != null)
                {
                    _Links[i].Drop();
                    _Links[i] = null;
                }
               // _Links.RemoveAt(i);
            }
        }

        public void GetLinks()
        {
            _Links = new List<ChainLink>();
            for (int i = 0; i < transform.childCount; i++)
            {
                ChainLink link = transform.GetChild(i).gameObject.GetComponent<ChainLink>();
                if(link != null)
                    link.transform.rotation = Quaternion.LookRotation(Vector3.right);
                if (link != null && _Links.Contains(link) == false)
                {
                    _Links.Add(link);
                    link.gameObject.name = "link_" + i.ToString();
                }
            }
        }

        public void SetPositions()
        {
            if (_Positioner == null || pivot_1 == null || pivot_2 == null || _Links.Count == 0)
                return;
            transform.position = pivot_1.transform.position;
            _Positioner.SetPositionsForced(new ChainLinksInfo(_Links), pivot_1, pivot_2);

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