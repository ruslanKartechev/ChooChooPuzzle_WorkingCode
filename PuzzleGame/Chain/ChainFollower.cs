using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
namespace PuzzleGame
{


    public class MoveData
    {
        public Vector3 newPos;
        public float newPercent;
        public MoveData() { }
        public MoveData(Vector3 pos, float percent)
        {
            newPos = pos;
            newPercent = percent;
        }
    }
    public class ChainFollower : UnitSplineFollower
    {
        public ChainController _Controller;
        public MoveData currentData;

        public List<ChainFollower> _links = new List<ChainFollower>();

        public bool UnderControl = false;

        private void Start()
        {
            onMove = SetSegment;
        }

        public void Init(ChainController controller, FollowerSettings settings)
        {
            _Controller = controller;
            InitSettings(settings);
        }

        public void ResetLinks()
        {
            _links = new List<ChainFollower>();
        }
        public void AddLink(ChainFollower link)
        {
            if (_links.Contains(link) == false)
                _links.Add(link);
        }


        public override bool PushFromNode(Vector2 dir)
        {
            bool can = false;
            _Controller.leadingFollower = this;
            ChainFollower end = _Controller.GetOtherEnd();
            can = end.PushForward();
            _Controller.StartMovingChain();
            return can;
        }

        public override void TakeInput(Vector2 input)
        {
            _Controller.MoveTesterBy(input);
        }

        public override void OnMoveStart()
        {
            _Controller.OnClick(this);
        }

        public override void OnMoveEnd()
        {
            _Controller.OnRelease();
        }
        public bool PushForward()
        {
            _Controller.leadingFollower = this;
            SplineNode node = FindFreeNode();
            if (node == null)
                return false;
            else
            {
                MoveLead(node, OnPushEnd);
                return true;
            }
        }
        private void OnPushEnd()
        {
            _Controller.StopMovingChain();
        }

        public void MoveToLead(ChainFollower caller, SplineNode node)
        {
            ChainFollower link = _links.Find(x => x != caller);
            if (link != null)
            {
                SplineNode temp = currentNode;
                MoveToNode(node);
                link.MoveToLead(this, temp);
            }
            else
                MoveToNode(node);
        }

        public void MoveLead(SplineNode node, Action onEnd = null)
        {
            if (node)
            {
                SplineNode temp = currentNode;
                MoveToNode(node, onEnd);
                _links[0].MoveToLead(this, temp);
            }
        }


        public void MoveLead(Vector2 dir)
        {
            var next = NodeSeeker.FindNextNode(dir, GetLastNode());
            if (next)
            {
             //   _links[0].MoveChain(this, currentNode);
                MoveToNode(next);

            }
        }

        protected override bool MoveAlongSegment(Vector2 input)
        {
            if (currentSegment == null) return false;
            float proj = Vector2.Dot(input, (Vector2)currentSegment.Dir);
            float percent = currentSegment.currentPercent;
            if (proj > 0) // moving forward
            {
                percent += _settings.moveSpeed / 100;
                Mathf.Clamp01(percent);
                if (percent >= 0.94f)
                {
                    currentData = new MoveData(currentSegment.end._position, 0);
                    SnapToEnd();
                    return true;
                }
            }
            else // moving backward
            {
                percent -= _settings.moveSpeed / 100;
                Mathf.Clamp01(percent);
                if (percent <= 0f)
                {
                    DebugMovement("percent is 0");
                    onMove = SetSegment;
                    return false;
                }
            }
            Vector3 newPos = currentSegment.start.transform.position + currentSegment.Dir * percent;
            currentData = new MoveData(newPos, percent);
           //transform.position = newPos;
           //currentSegment.currentPercent = percent;
            return true;
        }

       
        public void SnapToEnd()
        {
            if (currentSegment == null) return;
            Debug.Log("snapping to: " + currentSegment.end);
            ResetCurrentNode(currentSegment.end);
           // transform.position = currentNode._position;
            currentSegment = null;
            onMove = SetSegment;

        }


        public SplineNode FindFreeNode()
        {
            SplineNode res = null;
            foreach(SplineNode n in currentNode.linkedNodes)
            {
                // more contrainted logic here
                if (n.IsOccupied == false)
                    res = n;
            }
            //if (res == null)
            //    Debug.Log("didn't find next node. Current: " + currentNode.gameObject.name);
            //else
            //    Debug.Log("Found next: " + res.gameObject.name);
            return res;
        }


    }

    [CustomEditor(typeof(ChainFollower))]
    public class SplineChainFollowerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ChainFollower me = target as ChainFollower;
        }
    }
}