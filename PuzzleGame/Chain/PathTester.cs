using System.Collections;
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


    public class PathTester : BaseSplineFolower
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
                //model.SetActive(false);
                IsVilisble = false;
            }
        }
        public void HideForced()
        {
           // model.SetActive(false);
            IsVilisble = false;
        }
        public void SnapTo(SplineNode node)
        {
            currentNode = node;
            transform.position = node._position;
        }
        public override void TakeInput(Vector2 input)
        {
            if (input == Vector2.zero) return;
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
            if (_Controller.LeadingFollower == null)
                FindNewLead(input);

            if (currentSegment != null && currentSegment.end != null)
                ResetCurrentNode(currentSegment.end);

            ConstraintResult result = _Controller._ConstraintHandler.CheckConstraint(currentNode._Constraints, input);
            if (result.Allowed == false || result.Options == null)
            {
                _Controller.MoveChain(currentNode);
                _Controller.BlockNextMovement();
                return false;
            }
            SplineNode nextNode = NodeSeeker.FindNextNode(input, currentNode, result.Options);

            if (_Controller.IsChainOccupied(nextNode))
            {
                FindNewLead(input);
                return base.SetSegment(input);
            }
            currentSegment = new Segment(currentNode, nextNode);
            _Controller.MoveChain(currentNode);
            onMove = MoveAlongSegment;
            return true;
        }

        protected void FindNewLead(Vector2 input)
        {
            if(_Controller.LeadingFollower == null)
            {
                SplineNode node = _Controller.GetLeadingFollower(input);
                currentNode = node;
                transform.position = currentNode._position;
            }
            else
            {
                SplineNode old = _Controller.LeadingFollower.currentNode;
                SplineNode node = _Controller.GetLeadingFollower(input);
                if (old != node)
                {
                    currentNode = node;
                }
            }
        }
        protected override bool MoveAlongSegment(Vector2 input)
        {
            if (currentSegment == null) return false;
            if (currentSegment.currentPercent >= 5f / 100)
                Show();

            Vector3 planeInput = (Vector3)input;
            planeInput.z = input.y;
            planeInput.y = 0;
            float proj = Vector2.Dot(planeInput, currentSegment.Dir);
            float percent = currentSegment.currentPercent;
            if (proj >= 0) // moving forward
            {
                percent += _settings.moveSpeed / 100;

                Mathf.Clamp01(percent);
                if (percent >= 1)
                {
                    onMove = SetSegment;
                    return true;
                }
                if (percent >= 0.8f)
                {
                    bool allow = OnNodeApproach(input);
                    if (allow == false) return false;
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



        private bool OnNodeApproach(Vector2 input)
        {
            if (_Controller.IsChainOccupied(currentSegment.end) == true) return false;
            bool allow = currentSegment.end.PushFromNode(input);
            if(allow == false)
            {
                return false;
            }
            ResetCurrentNode(currentSegment.end);
            transform.position = currentSegment.end._position;
            _Controller.MoveChain(currentSegment.end);
            _Controller.OnMoveMade();
            onMove = SetSegment;
            return true;

        }

        protected override void ResetCurrentNode(SplineNode node)
        {
            base.ResetCurrentNode(node);
            _Controller.OnMoveMade();
        }
    }
}