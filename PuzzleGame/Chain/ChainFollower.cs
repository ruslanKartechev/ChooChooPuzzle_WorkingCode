using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            _Controller.OnFollowerInput?.Invoke(input);
        }

        public override void OnMoveStart()
        {
            _Controller.OnFollowerClick?.Invoke(this);
        }

        public override void OnMoveEnd()
        {
            _Controller.OnFollowerRelease?.Invoke();
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
                if (node == currentNode) return;
                SplineNode temp = currentNode;
                MoveToNode(node, onEnd);
                _links[0].MoveToLead(this, temp);
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
            return true;
        }

       
        public void SnapToEnd()
        {
            if (currentSegment == null) return;
            ResetCurrentNode(currentSegment.end);
            currentSegment = null;
            onMove = SetSegment;

        }


        public SplineNode FindFreeNode()
        {
            SplineNode res = null;
            foreach(SplineNode n in currentNode.linkedNodes)
            {
                if (n.IsOccupied == false)
                    res = n;
            }
            return res;
        }




    }

}