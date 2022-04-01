using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CommonGame;
namespace PuzzleGame
{

    public class ChainController : MonoBehaviour, IChainMovable
    {
        [SerializeField] private ChainSettings settings;
        [Space(10)]
        [SerializeField] private ChainNumber _number;
        [Header("Pivot Followers")]
        [Space(10)]
        [SerializeField] private List<ChainFollower> _followers = new List<ChainFollower>();

        [Header("Start nodes")]
        [SerializeField] private List<SplineNode> _nodes = new List<SplineNode>();
        [Header("Chain segment managers")]
        [SerializeField] private List<ChainSegmentManager> _chainSegments = new List<ChainSegmentManager>();
        [Space(10)]
        [SerializeField] private PathTester tester;

        private ChainFollower end_1;
        private ChainFollower end_2;
        [SerializeField] private ChainFollower _leading;

        public ChainConstaintHandler _ConstraintHandler;
        private ChainEffectsController _Effects;

        private Action OnCurrentMoveEnd;

        private Action<ChainFollower> _OnFollowerClick;
        private Action _OnFollowerRelease;
        private Action<Vector2> _OnFollowerInput;


        public ChainFollower LeadingFollower { get { return _leading; } }
        public Action<ChainFollower> OnFollowerClick { get { return _OnFollowerClick; } }
        public Action OnFollowerRelease { get { return _OnFollowerRelease; } }
        public Action<Vector2> OnFollowerInput { get { return _OnFollowerInput; } }

        private void Start()
        {
            InitChain();
        }

        private void InitChain()
        {
            foreach (ChainFollower f in _followers)
            {
                f.Init(this, settings.followerSettings);
            }
            GetChainSegments();
            _chainSegments.TrimExcess();
            InitSegments();

            if (tester == null) Debug.Log("no tester");

            SetNodes();
            SetChainPositions();

            end_1 = _followers[0];
            end_2 = _followers[_followers.Count - 1];

            tester.SnapTo(_nodes[_nodes.Count - 1]);
            tester.Init(this, settings.followerSettings);
            tester.HideForced();

            _ConstraintHandler = new ChainConstaintHandler();
            _ConstraintHandler.chain = this;

            _Effects = new ChainEffectsController(settings.ChainEffects, _chainSegments);

            GameManager.Instance._events.LevelStarted.AddListener(OnLevelStart);
            GameManager.Instance._events.LevelFinished.AddListener(OnLevelEnd);
        }

        private void OnLevelStart()
        {
            InitFollowerActions();
        }

        private void OnLevelEnd()
        {
            GameManager.Instance._events.LevelStarted.RemoveListener(OnLevelStart);
            GameManager.Instance._events.LevelFinished.RemoveListener(OnLevelEnd);
            InitFollowerActions(true);
        }

        public void InitFollowerActions(bool setNull = false)
        {
            if (setNull)
            {
                _OnFollowerClick = null;
                _OnFollowerRelease = null;
                _OnFollowerInput = null;
                return;
            }
            _OnFollowerClick = OnClick;
            _OnFollowerRelease = OnRelease;
            _OnFollowerInput = MoveByInput;
        }

        private void InitSegments()
        {
            foreach(ChainSegmentManager segment in _chainSegments)
            {
                if (segment != null) segment.InitSegment(this);
            }
        }



        public void OnClick(ChainFollower follower)
        {
            tester.SnapTo(follower.currentNode);
            tester.HideForced();
            StartMovingChain();
            _Effects?.ExecuteEffect(ChainEffects.Start);
        }

        public void OnRelease()
        {
            tester.Hide();
            tester.OnMoveEnd();
            _leading = null;
            StopMovingChain();
            _Effects?.ExecuteEffect(ChainEffects.Stop);
        }
        public void MoveByInput(Vector2 input)
        {
            tester.TakeInput(input);
        }

        public void MoveChain(SplineNode NexNode)
        {
            if (_leading != null)
                _leading.MoveLead(NexNode, OnMoveEnd);
        }

        public void SetLeadingFollower(ChainFollower follower)
        {
            _leading = follower;
        }

        public void Cut(ChainSegmentManager segmentCaller, int linkIndex)
        {
            SegmentIndecesCalculator calculator = new SegmentIndecesCalculator();
            ChainCutter cutter = new ChainCutter();
            ChainCutResult res =  cutter.Cut(calculator.ConvertAll(_chainSegments), 
                calculator.Convert(_chainSegments, segmentCaller), 
                linkIndex);
            segmentCaller.DropLinks(res.LinksToCut);
            if(res.SegmentsAway != null)
            {
                foreach (int i in res.SegmentsAway)
                {
                    if (_chainSegments[i] != null)
                        _chainSegments[i].DropAllLinks();
                    _chainSegments[i] = null;
                }
                _chainSegments.RemoveAll(x => x = null);
            }
        }

        public ChainFollower GetOtherEnd()
        {
            if (_leading == null) { Debug.Log("No Leading Follower"); return end_1; }
            if (_leading == end_1)
            {
                _leading = end_2;
            }
            else
            {
                _leading = end_1;
            }
            return _leading;
        }

        public SplineNode GetLeadingFollower(Vector2 input)
        {
            ChainFollower lead = null;
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                if (input.x >= 0)
                {
                    if (end_1.GetLastNode()._position.x >= end_2.GetLastNode()._position.x)
                        lead = end_1;
                    else
                        lead = end_2;
                }
                else
                {
                    if (end_1.GetLastNode()._position.x < end_2.GetLastNode()._position.x)
                        lead = end_1;
                    else
                        lead = end_2;
                }
            }
            else
            {
                if (input.y >= 0)
                {
                    if (end_1.GetLastNode()._position.z >= end_2.GetLastNode()._position.z)
                        lead = end_1;
                    else
                        lead = end_2;
                }
                else
                {
                    if (end_1.GetLastNode()._position.z < end_2.GetLastNode()._position.z)
                        lead = end_1;
                    else
                        lead = end_2;
                }
            }
            _leading = lead;
            SplineNode node = lead.GetLastNode();
            return node;
        }
        
        public void StartMovingChain()
        {
            foreach (ChainSegmentManager chain in _chainSegments)
            {
                if(chain != null)
                    chain.StartChainMovement();
            }
        }

        public void StopMovingChain()
        {
            foreach (ChainSegmentManager chain in _chainSegments)
            {
                if (chain != null)
                    chain.StopChainMovement();
            }
        }

        public bool IsChainOccupied(SplineNode node)
        {
            foreach (ChainFollower n in _followers)
            {
                if (node == n.GetLastNode())
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsEndFollower(ChainFollower follower)
        {
            if (follower == end_1 || follower == end_2)
                return true;
            else
                return false;
        }
        public ChainPositionInfo GetChainPosition()
        {
            ChainPositionInfo info = new ChainPositionInfo();
            info.leadingNode = _leading.GetLastNode();
            info.testerPosition = tester.currentNode;
            List<SplineNode> positions = new List<SplineNode>(_followers.Count);
            foreach (ChainFollower f in _followers)
                positions.Add(f.GetLastNode());
            info.chainNodes = positions;
            return info;
        }

        public virtual void OnMoveMade()
        {
            GameManager.Instance._events.MoveMade.Invoke();
        }

        #region NodeTypesHandler
        public void CheckNodeType(SplineNode node)
        {
            switch (node.Type)
            {
                case NodeType.Default:
                    break;
                case NodeType.Finish:
                    HandleFinishNode(node);
                    break;
            }
        }

        public void HandleFinishNode(SplineNode node)
        {
            OnCurrentMoveEnd = null;
            FinishNode finish = (FinishNode)node;
            if (finish == null) return;
            if (finish.CompareNumbers(_number))
            {
                Debug.Log("<color=green> Finish Found: " + _number.ToString() + " </color>");
                OnRelease();
                InitFollowerActions(true);
                FinishMatcherController.Instance.FinishReached(_number);
            }
            else
                Debug.Log("<color=red> Wrong finish " + _number.ToString() + " </color>");
        }
        #endregion



        #region OnMovementEnd
        public void OnMoveEnd()
        {
            if(_leading != null)
                CheckNodeType(_leading.currentNode);
            OnCurrentMoveEnd?.Invoke();
            OnCurrentMoveEnd = null;
        }

        public void BlockNextMovement()
        {
            OnCurrentMoveEnd = OnMovementBlocked;
        }
 
        private void OnMovementBlocked()
        {
            OnRelease();
            _Effects.ExecuteEffect(ChainEffects.Shake);
        }
        #endregion



        #region FromEditor
        public void ConnectChainFollowers()
        {
            if (_followers.Count < 3) { Debug.Log("Min amount of links is 3"); return; }
            _followers[0].ResetLinks();
            _followers[0].AddLink(_followers[1]);
            for (int i = 1; i < _followers.Count; i++)
            {
                _followers[i].ResetLinks();
                _followers[i].AddLink(_followers[i - 1]);
                if (i < _followers.Count - 1)
                    _followers[i].AddLink(_followers[i + 1]);
            }
        }
        public void GetFollowers()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                ChainFollower follower = transform.GetChild(i).gameObject.GetComponent<ChainFollower>();
                if(follower != null)
                {
                    if (i == 0)
                        follower.gameObject.name = "Left";
                    else if (i == transform.childCount - 1)
                        follower.gameObject.name = "Right";
                    else
                        follower.gameObject.name = "Center_" + i.ToString();
                }
                if (follower != null && _followers.Contains(follower) == false)
                {
                    
                    _followers.Add(follower);
                }
            }
        }

        public void SetNodes()
        {
            if (_nodes.Count != _followers.Count)
            {
                Debug.Log("<color=red>" + "Nodes count doesn't match follwers count" + "</color>");
                return;
            }
            for (int i = 0; i < _followers.Count; i++)
            {
                if (_nodes[i] == null) { Debug.Log("node is null"); return; }
                _followers[i].SetCurrentNode(_nodes[i], true);

            }
        }

        public void GetChainSegments()
        {
            _chainSegments = new List<ChainSegmentManager>();
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                ChainSegmentManager temp = transform.parent.GetChild(i).GetComponent<ChainSegmentManager>();
                if (temp != null && _chainSegments.Contains(temp) == false)
                {
                    _chainSegments.Add(temp);
                }
            }
        }

        public void SetChainPositions()
        {

            foreach (ChainSegmentManager segment in _chainSegments)
            {
                if (segment != null)
                    segment.SetPositions();
            }



        }
        #endregion

    }

  
}