using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace PuzzleGame
{
    public class ChainFollower : BaseSplineFolower, IChainFollower
    {


        private ChainController _Controller;
        private List<ChainFollower> _links = new List<ChainFollower>();
        private FollowerMover _mover;
        private ChainFolloweRotationData rotationData;
        [SerializeField] private TrainLightsController _lightsController;
        private ChainLineTester _tester;

        private bool IsLead = false;
        private List<SplineNode> _history = new List<SplineNode>();
        private List<IChainFollower> _chain = new List<IChainFollower>();

        private bool FirstSegmentSet = false;

        private bool _enabledFollower = true;
        public bool EnabledFollower { get { return _enabledFollower; } }
        public void Init(ChainController controller, FollowerSettings settings)
        {
            _Controller = controller;
            InitSettings(settings);
            InitMover();
            onMove = SetSegment;

        }
        #region LeadingFollower
        public void SetupLead(List<IChainFollower> chain)
        {
            IsLead = true;
            _chain = chain;
            _tester = _Controller._testerInst;
            if (_chain.Contains(this))
                _chain.Remove(this);
            rotationData = new ChainFolloweRotationData(transform, _links[0].transform, false);
            StartRotation();
            if (_lightsController == null) Debug.Log("<color=red> Lights are not assigned </color>");
            _lightsController.OnStart();
        }

        public void SetAsLead()
        {
            if(_chain.Count >1)
                _mover.UseAdditionalModifier = true;
            onMove = SetSegment;
        }

        public void ReleaseLead()
        {
            _lightsController?.OnRelease();
            rotationData.target = _links[0].transform;
            rotationData.forward = false;
            FirstSegmentSet = false;
        }

        public void MoveLead(Vector2 input)
        {
            if (input.magnitude < _settings.inputThreshold) { return; }
            if (onMove == null)
                onMove = SetSegment;
            onMove.Invoke(input);
        }
        private void MoveOthers()
        {
            for (int i = 0; i < _chain.Count; i++)
            {
                _chain[i].MoveToFollow(_history[_history.Count - i - 2]);
            }
        }
        protected override bool SetSegment(Vector2 input)
        {
            //_tester?.Hide();
            if (_Controller.LeadingFollower == null)
                _Controller.ResetLead();
            SplineNode prev = currentNode;
            ConstraintResult result = _Controller._ConstraintHandler.CheckConstraint(currentNode._Constraints, input);
            if (result.Allowed == false || result.Options == null)
            {
                _Controller.HandleConstraintMessage(result._message);
                return false;
            }

            SplineNode next = NodeSeeker.FindNextNode(input, currentNode, result.Options);
            if (next == null) { return false; }
            if (currentSegment != null && next == currentSegment.start)
            {
                HandleBackwards();
                return false;
            }

            if (_Controller.IsChainOccupied(next))
            {
                HandleBackwards();
                return false;
            }
            currentSegment = new Segment(currentNode, next);
            onMove = MoveAlongSegment;
            SetTester(0f);
            rotationData.target = next.transform;
            rotationData.forward = true;
            if (FirstSegmentSet == false)
            {
                FirstSegmentSet = true;
                _lightsController?.OnClick();
            }
            return true;
        }

        private void HandleBackwards()
        {
            Debug.Log("Handle backwards");
            onMove = null;
            _mover.UseAdditionalModifier = false;
            OnDirectionChange();
            ChangeDirectionAsLead();
            _tester?.Hide();
            _Controller.ResetLead();
        }

        protected override bool MoveAlongSegment(Vector2 input)
        {
            if (currentSegment == null) { Debug.LogError("No segment"); return false; }
            _tester?.Show();
            float proj = Vector2.Dot(input.normalized, currentSegment.GetScreenDir().normalized);
            float percent = currentSegment.currentPercent;
            if (proj >= 0) // moving forward
                percent += _settings.TesterSpeed * Time.deltaTime;
            else // moving backwards
                percent -= _settings.TesterSpeed * Time.deltaTime;

            Mathf.Clamp01(percent);
            currentSegment.currentPercent = percent;
            if (percent >= _settings.NodeSnapPercent)
            {
                if (_mover.IsBusy == true || _links[0].IsMoving()) { /*Debug.Log("_mover is busy");*/ return false; }
                if (CheckBusy(currentSegment.end) == false) 
                {
                    _Controller.HandleConstraintMessage(ConstraintMessages.Blocked);
                    onMove = null; 
                    return false; 
                }
                _mover.AddNode(currentSegment.end); // moving self
                AddToHistory(currentSegment.end);
                ResetCurrentNode(currentSegment.end, true);
                MoveOthers(); // moving others

                _Controller.OnMoveMade();
                onMove = null;
                //_tester?.Hide();
            }
            else if (percent < 0)
            {
                currentSegment = null;
                SetSegment(input);
                onMove = SetSegment;
                return false;
            }
            SetTester(percent);
            return true;
        }
        // Called by the lead, when direction is changed;
        private void ChangeDirectionAsLead()
        {
            _mover.UseAdditionalModifier = false;
            foreach (IChainFollower f in _chain)
                f.OnDirectionChange();
        }

        private void SetTester(float percent)
        {
            if (_tester == null) { return; }
            Vector3 start = currentSegment.start._position;
            Vector3 end = currentSegment.start.transform.position + currentSegment.Dir * percent;
            _tester.SetPositions(start, end);
        }

        public void BlockedLightEffect()
        {
            _lightsController?.OnCollision();
        }
        public void SuccessLightEffect()
        {
            _lightsController?.OnSuccess();
        }
        #endregion




        private void InitMover()
        {
            if(_settings == null) { Debug.Log("settings are not assigned");return; }
            _mover = gameObject.AddComponent<FollowerMover>();
            _mover.Init(_settings);
            _mover.StartNotifier = OnMoverStart;
            _mover.StopNotifier = OnMoverEnd;
            _mover.NodeReachedNotifier = OnNewNodeReached;
            _enabledFollower = true;
        }

        #region MoverNotifiers
        public void OnMoverStart(SplineNode node)
        {

        }
        public void OnMoverEnd()
        {

        }
        public void OnNewNodeReached(SplineNode node)
        {
            _Controller.CheckNodeType(currentNode);
        }
        #endregion
        public override void Prepare()
        {
            _mover.ReEnable();
            _mover.EnableAcceleration();
            OccupyCurrentNode();
        }
        public void OnDirectionChange()
        {
            _mover.DisableAcceleration();
            _mover.DoAccelerateOnStart = true;
        }
        public void ResetFollower()
        {
            currentSegment = null;
            onMove = SetSegment;
            if(IsLead)
                _history = _Controller.GetChainNodes(this);
        }
        public void MoveToFollow(SplineNode node)
        {
            ResetCurrentNode(node, true);
            MoveViaMover(node);
        }
        private void AddToHistory(SplineNode node)
        {
            _history.Add(node);
        }

        private bool CheckBusy(SplineNode node)
        {
            if (_Controller.IsChainOccupied(node))
            {
                HandleBackwards();
                return false;
            }
            if (node.currentFollower != null)
            {
                if (node.PushFromNode(this) == false) { return false; }
            }
            return true;
        }

        #region Linking
        public void AddLink(ChainFollower link)
        {
            if (_links.Contains(link) == false)
                _links.Add(link);
        }
        public void ResetLinks()
        {
            _links = new List<ChainFollower>();
        }
        #endregion

        #region Pushing
        public override bool PushFromNode(ISplineFollower pusher)
        {
            bool allow = false;
            if (_Controller.IsEndFollower(this) == false)
            {
                if(_Controller.End_1.currentNode.currentFollower == pusher)
                {
                    _Controller.SetLeadingFollower(_Controller.End_2);
                    allow = _Controller.End_2.PushForward();

                } else if (_Controller.End_2.currentNode.currentFollower == pusher)
                {
                    _Controller.SetLeadingFollower(_Controller.End_1);
                    allow =  _Controller.End_1.PushForward();
                }
                if (allow == true)
                    _Controller.StartMovingLinks();
                return false;
            }
            _Controller.SetLeadingFollower(this); // myself
            ChainFollower otherLead = _Controller.ResetLead(); // other end
            if (otherLead.currentNode.currentFollower != pusher) // if other end is not occupied by the same follower
            {
                allow = true; // allow to occupy my node
            }
            else
                allow = PushForward(); // try to push
            if (allow == true)
                _Controller.StartMovingLinks();
            return allow;
        }

        public bool PushForward()
        {
            _Controller.SetLeadingFollower(this);
            SplineNode node = FindFreeNode();
            if (node == null) return false;

            _mover.AddNode(node);
            AddToHistory(node);
            ResetCurrentNode(node, true);
            MoveOthers();
            _Controller.SetLeadingFollower(null);
            return true;
        }
#endregion

        public void ClearMoverHistory()
        {
            _mover.ClearHistory();
        }

        public SplineNode FindFreeNode()
        {
            SplineNode res = null;
            foreach(SplineNode n in currentNode.linkedNodes)
            {
                if (n == null) continue;
                if (n.IsOccupied == false)
                    res = n;
            }
            return res;
        }

        public bool MoveViaMover(SplineNode node)
        {
            return _mover.AddNode(node);
        }

        public override void StopNodeSnapping()
        {
            currentSegment = null;
            onMove = SetSegment;
            _mover.DisableAcceleration();
            _mover.UseAdditionalModifier = false;
            // _mover.StopMoving();
        }

        public GameObject GetGo() { return gameObject; }

        public bool IsMoving()
        {
            return _mover.IsBusy;
        }
        public void DisableFollower()
        {
            _mover.StopAccepting();
            _mover.UseAdditionalModifier = false;
            _mover.ClearHistory();
            StopNodeSnapping();
            _enabledFollower = false;
            _tester?.Hide();
        }

        #region Movable
        public override void TakeInput(Vector2 input)
        {
            if (_enabledFollower == false) return;
            _Controller.OnFollowerInput?.Invoke(input);
        }

        public override void OnMoveStart()
        {
            if (_enabledFollower == false) return;
            _Controller.OnFollowerClick?.Invoke(this);
        }

        public override void OnMoveEnd()
        {
            if (_enabledFollower == false) return;
            _Controller.OnFollowerRelease?.Invoke();
        }
        #endregion

        #region Rotation
        public class ChainFolloweRotationData
        {
            public ChainFolloweRotationData(Transform origin, Transform lookTarget, bool forward)
            {
                this.origin = origin;
                this.target = lookTarget;
                this.forward = forward;
            }
            public Transform origin;
            public Transform target;
            public bool forward;
        }
        private Coroutine _rotationHandler;
        private void StartRotation()
        {
            StopRotation();
            _rotationHandler = StartCoroutine(RotationHandler());
        }
        private void StopRotation()
        {
            if (_rotationHandler != null) StopCoroutine(_rotationHandler);
        }
        private IEnumerator RotationHandler()
        {
            while (true)
            {
                Vector3 lookVector = rotationData.target.position-rotationData.origin.position;
                if (rotationData.forward == false)
                    lookVector = -lookVector;
                transform.rotation = Quaternion.LookRotation(lookVector);
                yield return null;
            }
        }
        #endregion





    }

}