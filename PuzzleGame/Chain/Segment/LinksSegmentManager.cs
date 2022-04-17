using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace PuzzleGame
{

    public class LinksSegmentManager : ChainSegmentManager
    {
        public Transform pivot_1;
        public Transform pivot_2;

        public ChainLinksPositioner _positioner;
        [Space(10)]
        public List<ChainLink> _Links = new List<ChainLink>();
        private void Start()
        {
            if (_positioner == null) _positioner = GetComponent<ChainLinksPositioner>();
        }
        public override void InitSegment(Transform pivot1, Transform pivot2)
        {
            pivot_1 = pivot1;
            pivot_2 = pivot2;
        }


        public override void StartChainMovement()
        {
            if(_positioner == null) { Debug.Log("No positioner set"); return ; }
            
            _positioner.StartMovement(new ChainSegmentData(_Links, pivot_1, pivot_2, _positioner));

        }
        public override void StopChainMovement()
        {
            _positioner.StopMovement();
        }
        public override ChainSegmentData GetChainInfo()
        {
            return new ChainSegmentData(_Links, pivot_1, pivot_2, _positioner);
        }

        // From Editor
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

        public override void UpdateSegment()
        {
            if (_Links.Count == 0) GetLinks();
            if (_positioner == null) _positioner = gameObject.GetComponent<ChainLinksPositioner>();
            if (_positioner == null || pivot_1 == null || pivot_2 == null || _Links.Count == 0)
                return;
            transform.position = pivot_1.transform.position;
            _positioner.SetPositionsForced(new ChainSegmentData(_Links, pivot_1, pivot_2,_positioner));

        }



    }


}