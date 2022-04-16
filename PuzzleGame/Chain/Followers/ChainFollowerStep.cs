using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace PuzzleGame
{
    public class ChainFollowerStep : BaseSplineFolower //, IChainFollower
    {
        private ChainFollowersController _Controller;
        private List<ChainFollower> _links = new List<ChainFollower>();
        private FollowerMover _mover;
        private ChainFolloweRotationData rotationData;
        [SerializeField] private TrainLightsController _lightsController;
        private ChainLineTester _tester;

        private bool IsLead = false;
        private List<SplineNode> _history = new List<SplineNode>();
        private List<IChainFollower> _chain = new List<IChainFollower>();

        private bool _enabledFollower = true;
        public bool EnabledFollower { get { return _enabledFollower; } }
        public void Init(ChainFollowersController controller, FollowerSettings settings)
        {
            _Controller = controller;
            InitSettings(settings);
            InitMover();
            onMove = SetSegment;

        }
        private void Update()
        {
            if(currentNode != null)
            {
                if (currentNode.currentFollower == null)
                    currentNode.SetCurrentFollowerForced(this);
            }
        }

        #region LeadingFollower
        public void SetupLead(List<IChainFollower> chain)
        {
            IsLead = true;
            _chain = chain;
         //   if (_chain.Contains(this))
           //     _chain.Remove(this);
            rotationData = new ChainFolloweRotationData(transform, _links[0].transform, false);
            StartRotation();
            gameObject.tag = _settings.LeadTag;
        }

        public void SetAsLead()
        {
            if(_chain.Count >1)
                _mover.UseAdditionalModifier = true;
            onMove = SetSegment;
        }

        public void ReleaseEndNode()
        {
            rotationData.target = _links[0].transform;
            rotationData.forward = false;
            _tester?.Hide();
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
               // _chain[i].MoveToFollow(_history[_history.Count - i - 2]);
            }
        }
        public void Bounce(Vector3 target, float percent, float time, ChainFollower caller)
        {
            _mover.Bounce(target, percent, time);
            ChainFollower f = _links.Find(t => t != caller);
            if(f != null)
            {
             //   f.Bounce(currentNode._position, percent, time, this);
            }
        }

        protected override bool SetSegment(Vector2 input)
        {
            if (_Controller.LeadingFollower == null)
                _Controller.ResetLead();
            ConstraintResult result = _Controller._ConstraintHandler.CheckConstraint(currentNode._Constraints, input);
            if (result.Allowed == false || result.Options == null)
            {
                _Controller.HandleConstraintMessage(result._message);
                return false;
            }
            #region DebugOptions
            //int i = 0;
            //foreach(SplineNode n in result.Options)
            //{
            //    Debug.Log("option: " + i.ToString() + " "+ n.transform.parent.parent.name
            //   + "  " + n.name);
            //    i++;
            //}
            #endregion

            SplineNode next = NodeSeeker.FindNextNode(input, currentNode, result.Options, _Controller.GetChainPosition().chainNodes);
            if (next == null) { Debug.Log("RETURNED NULL SEEKER"); return false; }
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
            if(currentSegment == null)
                currentSegment = new Segment(currentNode, next);
            if (currentSegment.end != next)
                currentSegment = new Segment(currentNode, next);
            rotationData.target = next.transform;
            rotationData.forward = true;
            MoveOnSegment(input);
            return true;
        }

        private void HandleBackwards()
        {
            //Debug.Log("Handling backwards");
            onMove = null;
            _mover.UseAdditionalModifier = false;
            OnDirectionChange();
            ChangeDirectionAsLead();
            _tester?.Hide();
            _Controller.ResetLead();
        }

        protected override bool MoveOnSegment(Vector2 input)
        {
            if (currentSegment == null) { Debug.LogError("No segment"); return false; }
            float proj = Vector2.Dot(input.normalized, currentSegment.GetScreenDir().normalized);
            float percent = currentSegment.currentPercent;
            if (proj >= 0) { percent += _settings.TesterSpeed * Time.deltaTime; }
            else { percent -= _settings.TesterSpeed * Time.deltaTime; }
            Mathf.Clamp01(percent);
            currentSegment.currentPercent = percent;
            if (percent >= _settings.NodeSnapPercent)
            {
                if (_mover.IsBusy == true || _links[0].IsMoving()) { /*Debug.Log("_mover is busy");*/ return false; }
                if (CheckNextNode(currentSegment.end) == false)
                {
                    onMove = null;
                    currentSegment = null;
                    _Controller.ResetLead();
                    return false;
                }
                _mover.AddNode(currentSegment.end); // moving self
                AddToHistory(currentSegment.end);
                ResetCurrentNode(currentSegment.end, true);
                MoveOthers(); // moving others

                _Controller.OnMoveMade();
                onMove = null;
            }
            else if (percent < 0) { currentSegment = null; return false; }
            return true;
        }
        private bool CheckNextNode(SplineNode node)
        {
            if (_Controller.IsChainOccupied(node))
            {
                HandleBackwards();
                return false;
            }
            if (node.currentFollower != null)
            {
                string message = node.PushFromNode(this).message;
                return HandlePushnodeMessage(message);
            }
            else
            {
                Ray ray = new Ray();
                ray.origin = transform.position; ray.direction = transform.position - _links[0].transform.position;
                if (Physics.Raycast(ray, out RaycastHit hit, 0.5f, _settings.castMask))
                {
                    if (hit.collider.gameObject.tag == _settings.LeadTag)
                    {
                        ChainFollower follower = hit.collider.GetComponent<ChainFollower>();
                        string message = follower.PushFromNode(this).message;
                        return HandlePushnodeMessage(message);
                    }
                }
            }

            return true;
        }
        private bool HandlePushnodeMessage(string message)
        {
            if (message == NodePushMessage.ContactBlock.ToString() || message == NodePushMessage.SideBlock.ToString())
            {
               // Bounce(currentSegment.end._position, _settings.bouncingPercent, _settings.bounceTime, this);
                _mover.OnBounceHit = OnBounceHit;
                return false;
            }
            else
                return true;
        }

        private void OnBounceHit()
        {
            _Controller.HandleConstraintMessage(ConstraintMessages.CloseContanctBlock);
            _mover.OnBounceHit = null;
        }


        // Called by the lead, when direction is changed;
        private void ChangeDirectionAsLead()
        {
            _mover.UseAdditionalModifier = false;
            foreach (IChainFollower f in _chain)
                f.OnDirectionChange();
        }
        public void BlockedLightEffect()
        {
            //_lightsController?.OnCollision();
        }
        public void SuccessLightEffect()
        {
            //_lightsController?.OnRelease();
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
            _Controller.OnLeadNodeSet(currentNode);
            if (IsLead)
            {
                rotationData.target = _links[0].transform;
                rotationData.forward = false;
            }
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
            //currentSegment = null;
            //onMove = SetSegment;
            //if(IsLead)
            //    _history = _Controller.GetChainNodes(this);
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
        public override Vector3 GetSegmentVector()
        {
            return currentSegment.end._position - currentSegment.start._position;
        }
        public override NodePushResult PushFromNode(ISplineFollower pusher)
        {
            NodePushResult result = new NodePushResult();
            //result.success = false;
            //result.message = NodePushMessage.PushFail.ToString();
            //if (_Controller.IsEndFollower(this) == false)
            //{
            //    result = TryPushSide(pusher);
            //    return result;
            //}
            //_Controller.SetLeadingFollower(this); // myself

            //ChainFollower otherLead = _Controller.ResetLead(); // other end
            //if (otherLead.currentNode.currentFollower != pusher) // if other end is not occupied by the same follower
            //{
            //    result.success = false;
            //    result.message = NodePushMessage.PushFail.ToString();
            //    if (currentNode.HasAngleConstraint == true)
            //    {
            //        Vector3 other = pusher.GetSegmentVector();
            //        Vector3 mine = currentNode._position - _links[0].transform.position;
            //        float angle = AngleHandler.GetAngle(other, mine, Vector3.up);
            //        Debug.Log("Angle: " + angle);
            //        if (angle <= _settings.PushAngleThreshold)
            //        {
            //            //result = TryPushOtherSide();
            //            result.success = false;
            //            result.message = NodePushMessage.ContactBlock.ToString() ;
            //        }
            //    }
            //}
            //else
            //{
            //    result = TryPushForward();
            //}
            return result;
        }
        private NodePushResult TryPushForward()
        {
            NodePushResult result = new NodePushResult();
            result.success = PushForward(); // try to push
            if (result.success == false)
            { result.message = NodePushMessage.ContactBlock.ToString(); Debug.Log("Push fail"); }
            else
            { result.message = NodePushMessage.PushSucess.ToString(); Debug.Log("Push success"); }
            return result;
        }

        private NodePushResult TryPushSide(ISplineFollower pusher)
        {
            NodePushResult result = new NodePushResult();
            result.success = false;
            result.message = NodePushMessage.SideBlock.ToString();
            if (_Controller.End_1.currentNode.currentFollower == pusher)
            {
                _Controller.SetLeadingFollower(_Controller.End_2);
                result.success = _Controller.End_2.PushForward();
                result.message = NodePushMessage.ContactBlock.ToString();
            }
            else if (_Controller.End_2.currentNode.currentFollower == pusher)
            {
                _Controller.SetLeadingFollower(_Controller.End_1);
                result.success = _Controller.End_1.PushForward();
                result.message = NodePushMessage.ContactBlock.ToString();
            }
            else
            {
                result.success = false; result.message = NodePushMessage.SideBlock.ToString();
            }
            if (result.success == true)
            {
                result.message = NodePushMessage.PushSucess.ToString();
            }
            return result;
        }

        public bool PushForward()
        {
            //_Controller.SetLeadingFollower(this);
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


        public SplineNode FindFreeNode()
        {
            Vector2 input = AngleHandler.GetScreenVector(_links[0].transform.position, transform.position);
            ConstraintResult result = _Controller._ConstraintHandler.CheckConstraint(currentNode._Constraints, input);
            if (result.Allowed == false || result.Options == null)
            {
                _Controller.HandleConstraintMessage(result._message);
                return null;
            }
            SplineNode res = null;
            foreach (SplineNode n in result.Options /*currentNode.linkedNodes*/)
            {
                if (n == null) continue;
                if (n.IsOccupied == false)
                    res = n;
            }
            return res;
        }

        public void ClearMoverHistory()
        {
            _mover.ClearHistory();
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
            currentNode.ReleaseNode();
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
      //      _Controller.OnFollowerClick?.Invoke(this);
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