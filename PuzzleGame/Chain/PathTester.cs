﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;


namespace PuzzleGame
{
    public interface IMovable
    {
        void OnMoveStart();
        void OnMoveEnd();
        void TakeInput(Vector2 input);
    }


    public class PathTester : UnitSplineFollower
    {
        [SerializeField] private GameObject model;
        public ChainController _Controller;
        public MoveData currentData;
        [HideInInspector] public bool IsVilisble = true;

        

        public void Init(ChainController controller, FollowerSettings settings)
        {
            _Controller = controller;
            InitSettings(settings);
            onMove = SetSegment;
            OccupyNodes = false;
        }
        
        public void Show()
        {
            if (!IsVilisble)
            {
                model.SetActive(true);
                IsVilisble = true;
            }
        }
        public void Hide()
        {
            if (IsVilisble)
            {
                model.SetActive(false);
                IsVilisble = false;
            }
        }
        public void HideForced()
        {
            model.SetActive(false);
            IsVilisble = false;
        }
        public void SnapTo(SplineNode node)
        {
            currentNode = node;
            transform.position = node._position;
        }
        public override void TakeInput(Vector2 input)
        {
            if(onMove != null)
                onMove(input);
        }
        public override void OnMoveEnd()
        {
            StopNodeSnapping();
            currentSegment = null;
            transform.position = currentNode._position;
            onMove = SetSegment;
            
        }


        protected override bool SetSegment(Vector2 input)
        {
            if (_Controller.leadingFollower == null)
            {
                FindNewLead(input);
            }
            //currentNode = _Controller.leadingFollower.currentNode;

            ConstraintResult result = _Controller._ConstraintHandler.CheckConstraint(currentNode._Constraints, input);
            if (result.Allowed == false || result.Options == null)
            {
      
                return false;
            }

            SplineNode temp = NodeSeeker.FindNextNode(input, currentNode, result.Options);

            if (IsChainOccupied(temp))
            {
                FindNewLead(input);
                return base.SetSegment(input);
            }
            else
            {
                currentSegment = new Segment(currentNode, temp);
                onMove = MoveAlongSegment;
                return true;
            }
            //return false;
        }
        protected void FindNewLead(Vector2 input)
        {
            if(_Controller.leadingFollower == null)
            {
                SplineNode node = _Controller.GetLeadingFollower(input);
                currentNode = node;
                transform.position = currentNode._position;
            }
            else
            {
                SplineNode old = _Controller.leadingFollower.currentNode;
                SplineNode node = _Controller.GetLeadingFollower(input);
                if (old != node)
                {
                    currentNode = node;
                   // transform.position = currentNode._position;
                }
            }
  
        }

        protected bool IsChainOccupied(SplineNode node)
        {
            foreach(ChainFollower n in _Controller._Followers)
            {
                if (node == n.GetLastNode())
                {
                    return true;
                }
            }
            return false;
        }

        protected override bool MoveAlongSegment(Vector2 input)
        {
            //Debug.Log("moving on segment");
            if (currentSegment == null) return false;
            if (currentSegment.currentPercent >= 5f / 100)
                Show();

            float proj = Vector2.Dot(input, (Vector2)currentSegment.Dir);
            float percent = currentSegment.currentPercent;
            if (proj > 0) // moving forward
            {
                percent += _settings.moveSpeed / 100;

                Mathf.Clamp01(percent);
                if (percent >= 0.9f)
                {
                    OnNodeApproach(input);
                    return true;
                }
            }
            else // moving backwards
            {
                percent -= _settings.moveSpeed / 100;
                Mathf.Clamp01(percent);

                if (percent <= 0f)
                {
                    onMove = SetSegment;
                    return true;
                }
            }
            Vector3 newPos = currentSegment.start.transform.position + currentSegment.Dir * percent;
            transform.position = newPos;
            currentSegment.currentPercent = percent;
            return true;
        }


        private void OnNodeApproach(Vector2 input)
        {
            if (IsChainOccupied(currentSegment.end) == true) return;
            bool allow = currentSegment.end.PushFromNode(input);
            if(allow == false)
            {
                //Debug.Log("<color=red>NOT Allowed to Push Further</color>");
                return;
            }
            _Controller.MoveChain(currentSegment.end);
            //transform.position = currentSegment.end._position;
            ResetCurrentNode(currentSegment.end);
            onMove = SetSegment;
        }

        protected override void ResetCurrentNode(SplineNode node)
        {
            base.ResetCurrentNode(node);
            _Controller.OnMoveMade();
        }
    }
}