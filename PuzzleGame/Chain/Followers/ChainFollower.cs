using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace PuzzleGame
{
    public class ChainFollower : BaseSplineFolower
    {
        private ChainFollowersController _Controller;
        private List<ChainFollower> _links = new List<ChainFollower>();
        private FollowerMover _mover;
        private ChainFolloweRotationData rotationData;
        [SerializeField] private TrainLightsController _lightsController;

        private bool IsLead = false;
        private List<SplineNode> _history = new List<SplineNode>();
        private List<ChainFollower> _chain = new List<ChainFollower>();

        private bool _enabledFollower = true;
        public void Init(ChainFollowersController controller, FollowerSettings settings)
        {
            _Controller = controller;
            InitSettings(settings);
            InitMover();
            onMove = SetupSegments;

        }
        private void Update()
        {
            if (currentNode != null)
            {
                if (currentNode.currentFollower == null)
                    currentNode.SetCurrentFollowerForced(this);
            }
        }

        #region LeadingFollower
        public void SetupLead(List<ChainFollower> chain)
        {
            IsLead = true;
            _chain = chain;
            if (_chain.Contains(this))
                _chain.Remove(this);
            rotationData = new ChainFolloweRotationData(transform, _links[0].transform, false);
            StartRotation();
            gameObject.tag = _settings.LeadTag;
        }

        public void SetAsLead()
        {
            _history = _Controller.GetChainNodes(this);
            if (currentSegment != null) { onMove = MoveOnSegment; Debug.Log("START seg != null"); }
            else { onMove = SetupSegments; Debug.Log("START seg == null"); }
        }

        public void ReleaseEndFollower()
        {
            rotationData.target = _links[0].transform;
            rotationData.forward = false;
        }


        public void MoveLead(Vector2 input)
        {
            //if (input.magnitude < _settings.inputThreshold) { return; }
            if (onMove == null)
                onMove = SetupSegments;
            onMove.Invoke(input);
        }
        private void MoveOthers()
        {
            for (int i = 0; i < _chain.Count; i++)
            {
                _chain[i].MoveToFollow(_history[_history.Count - i - 2]);
            }
        }


        public void SetSegment(SplineNode end)
        {
            if (end == null)
                currentSegment = null;
            else
                currentSegment = new Segment(currentNode, end);
            //Debug.Log("<color=green>SEG SEGMENT: " + gameObject.name + " \nEND: " + end.gameObject.name +"</color>");
        }
        public void SnapToSegmentEnd()
        {
            if (currentSegment == null) { Debug.Log("Null segment error"); return; }
            ResetCurrentNode(currentSegment.end, true);
            AddToHistory(currentSegment.end);
            //transform.position = currentSegment.end._position;
            currentSegment = null;
        }
        public void SnapToSegmentStart()
        {
            if (currentSegment == null) { Debug.Log("Null segment error"); return; }
            ResetCurrentNode(currentSegment.start, true);
           // transform.position = currentSegment.start._position;
            currentSegment = null;
        }

        protected override bool SetupSegments(Vector2 input)
        {
            Debug.Log("1");
            if (_Controller.LeadingFollower == null)
                _Controller.ResetLead();
            ConstraintResult result = _Controller._ConstraintHandler.CheckConstraint(currentNode._Constraints, input);
            if (result.Allowed == false || result.Options == null)
            {
                _Controller.HandleConstraintMessage(result._message, this);
                return false;
            }
            SplineNode next = NodeSeeker.FindNextNode(input, currentNode, result.Options, null);
            if (next == null) { Debug.Log("Did not find next lead node"); return false; }
            if (currentSegment == null)
                currentSegment = new Segment(currentNode, next);

            rotationData.target = next.transform;
            rotationData.forward = true;

            _history = _Controller.GetChainNodes(this);
            if (_Controller.IsChainOccupied(currentSegment.end) == true)
            {
                NullifySegments();
                _Controller.ResetLead();//.MoveLead(input);
                return false;
            }
            else
            {
                for (int i = 0; i < _chain.Count; i++)
                {
                    _chain[i].SetSegment(_history[_history.Count - i - 1]);
                }
            }
            MoveOnSegment(input);
            onMove = MoveOnSegment;
            return true;
        }
        //private SplineNode GetOtherEndNext(Vector2 input)
        //{
        //    ChainFollower other = _Controller.GetOtherEnd(this);
        //    SplineNode next = other.FindFreeNode(input);
        //    if (next == null) {return null;}
        //    Debug.Log("found other next node: " + next.gameObject.name);

        //    return next;
        //}
        private void NullifySegments()
        {
            currentSegment = null;
            foreach (ChainFollower f in _chain)
            {
                f.SetSegment(null);
            }
        }


        public SplineNode FindFreeNode(Vector2 input)
        {
            ConstraintData data = new ConstraintData();
            data.ScreenDirection = input;
            data.chainPositions = _Controller.GetChainPosition();
            data.chainPositions.leadingNode = currentNode;
            ConstraintResult result = _Controller._ConstraintHandler.CheckConstraint(currentNode._Constraints, data);
            List<SplineNode> nextOptions = currentNode.linkedNodes.FindAll(t => _Controller.IsChainOccupied(t) == false);
            if (nextOptions.Count == 0) { return null; }
            else if (nextOptions.Count == 1) { return nextOptions[0]; }
            else
            {
                if (result.Allowed == false || result.Options == null)
                {
                    _Controller.HandleConstraintMessage(result._message, this);
                    return null;
                }
                SplineNode next = NodeSeeker.FindNextNode(input, currentNode, result.Options, null);
                return next;
            }
        }

        protected override bool MoveOnSegment(Vector2 input)
        {
            Debug.Log("2");
            if (currentSegment == null) { Debug.Log("Move on null seg"); onMove = SetupSegments; return false; }
            // float proj = Vector2.Dot(input.normalized, currentSegment.GetScreenDir().normalized);
            float proj = Vector2.Dot(input.normalized, GetChainDirScreen());
            float percent = currentSegment.currentPercent;
            if (proj >= 0) 
            { 
                percent += _settings.TesterSpeed * Time.deltaTime; 
            }
            else 
            { 
                percent -= _settings.TesterSpeed * Time.deltaTime; 
            }
            percent = Mathf.Clamp01(percent);
            currentSegment.currentPercent = percent;
            MoveByPercent(percent);
            foreach (ChainFollower f in _chain)
            {
                f.MoveByPercent(percent);
            }
            if (percent >= 1)
            {
                SnapToSegmentEnd();
                foreach (ChainFollower f in _chain)
                {
                    f.SnapToSegmentEnd();
                }
                _Controller.OnMoveMade();
                _Controller.OnLeadNodeSet(currentNode);
                onMove = SetupSegments;
            }
            else if (percent <= 0)
            {
                SnapToSegmentStart();
                foreach (ChainFollower f in _chain)
                {
                    f.SnapToSegmentStart();
                }
                onMove = SetupSegments;
            }
            return true;
        }

        public void MoveByPercent(float percent)
        {
            //Debug.Log("Called to move: " + gameObject.name + " :  " + percent);
            if (currentSegment == null) { Debug.Log("<color=red>Cannot set percent, segment is null</color>"); return; }
            currentSegment.currentPercent = percent;
            transform.position = currentSegment.start._position +
                currentSegment.Dir * currentSegment.Distance * currentSegment.currentPercent;
        }
        private Vector2 GetChainDirScreen()
        {
            Vector2 end = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 start = Camera.main.WorldToScreenPoint(_Controller.GetOtherEnd(this).transform.position);
            return (end - start).normalized;
        }

        public bool CheckNextNode(SplineNode node)
        {
            if (node.currentFollower != null)
            {
                string message = node.PushFromNode(this).message;
                return HandlePushNodeMessage(message, node);
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
                        return HandlePushNodeMessage(message, node);
                    }
                }
            }
            return true;
        }

        public void Bounce(Vector3 target, float percent, float time, ChainFollower caller)
        {
            _mover.Bounce(target, percent, time);
            ChainFollower f = _links.Find(t => t != caller);
            if (f != null)
            {
                f.Bounce(currentNode._position, percent, time, this);
            }
        }
        private bool HandlePushNodeMessage(string message, SplineNode targetNode)
        {
            if (message == NodePushMessage.ContactBlock.ToString() || message == NodePushMessage.SideBlock.ToString())
            {
                Bounce(targetNode.transform.position, _settings.bouncingPercent, _settings.bounceTime, this);
                _mover.OnBounceHit = OnBounceHit;
                return false;
            }
            else
                return true;
        }

        private void OnBounceHit()
        {
            _Controller.HandleConstraintMessage(ConstraintMessages.CloseContanctBlock, this);
            _mover.OnBounceHit = null;
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
            if (_settings == null) { return; }
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
            OccupyCurrentNode();
        }

        public void ResetFollower()
        {
            currentSegment = null;
            onMove = SetupSegments;
            if (IsLead)
                _history = _Controller.GetChainNodes(this);
        }
        public void MoveToFollow(SplineNode node)
        {
            ResetCurrentNode(node, true);
            MoveToNode(node);
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
            if (currentSegment == null) { Debug.Log("null segment: " + gameObject.transform);  return transform.forward; }
            return currentSegment.end._position - currentSegment.start._position;
        }
        public override NodePushResult PushFromNode(ISplineFollower pusher)
        {
            NodePushResult result = new NodePushResult();
            result.success = false;
            result.message = NodePushMessage.PushFail.ToString();
            if (_Controller.IsEndFollower(this) == false)
            {
                result = TryPushSide(pusher);
                return result;
            }
            _Controller.SetLeadingFollower(this); // myself

            ChainFollower otherLead = _Controller.ResetLead(); // other end
            if (otherLead.currentNode.currentFollower != pusher) // if other end is not occupied by the same follower
            {
                result.success = false;
                result.message = NodePushMessage.PushFail.ToString();
                if (currentNode.HasAngleConstraint == true)
                {
                    Vector3 other = pusher.GetSegmentVector();
                    Vector3 mine = currentNode._position - _links[0].transform.position;
                    float angle = AngleHandler.GetAngle(other, mine, Vector3.up);
                    if (angle <= _settings.PushAngleThreshold)
                    {
                        result.success = false;
                        result.message = NodePushMessage.ContactBlock.ToString();
                    }
                }
            }
            else
            {
                result = TryPushForward();
            }
            return result;
        }
        private NodePushResult TryPushForward()
        {
            NodePushResult result = new NodePushResult();
            result.success = PushForward(); // try to push
            if (result.success == false)
            { result.message = NodePushMessage.ContactBlock.ToString();  }
            else
            { result.message = NodePushMessage.PushSucess.ToString();  }
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
            _Controller.SetLeadingFollower(this);
            SplineNode node = FindFreeNode();
            if (node == null) {return false; }
            _mover.AddNode(node);
            AddToHistory(node);
            ResetCurrentNode(node, true);
            MoveOthers();
            //_Controller.SetLeadingFollower(null);
            return true;
        }
        #endregion


        public SplineNode FindFreeNode()
        {
            Vector2 input = AngleHandler.GetScreenVector(_links[0].transform.position, transform.position);
            ConstraintResult result = _Controller._ConstraintHandler.CheckConstraint(currentNode._Constraints, input);
            if (result.Allowed == false || result.Options == null)
            {
                _Controller.HandleConstraintMessage(result._message,this);
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

        public bool MoveToNode(SplineNode node)
        {
            return _mover.AddNode(node);
        }

        public override void StopNodeSnapping()
        {
            currentSegment = null;
            onMove = SetupSegments;
            _mover.DisableAcceleration();
            _mover.UseAdditionalModifier = false;
            _mover.StopMoving();
        }

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
          //  _rotationHandler = StartCoroutine(RotationHandler());
        }
        private void StopRotation()
        {
            if (_rotationHandler != null) StopCoroutine(_rotationHandler);
        }
        private IEnumerator RotationHandler()
        {
            while (true)
            {
                //Vector3 lookVector = rotationData.target.position-rotationData.origin.position;
                //if (rotationData.forward == false)
                //    lookVector = -lookVector;
                //transform.rotation = Quaternion.LookRotation(lookVector);
                yield return null;
            }
        }
        #endregion





    }

}