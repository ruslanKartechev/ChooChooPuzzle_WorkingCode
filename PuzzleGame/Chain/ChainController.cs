using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CommonGame;
using CommonGame.Sound;
namespace PuzzleGame
{
    [DefaultExecutionOrder(100)]
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

        private ChainFollower _leading;
        private ChainFollower end_1;
        private ChainFollower end_2;
        public ChainFollower End_1 { get { return end_1; } }
        public ChainFollower End_2 { get { return end_2; } }


        public ChainConstaintHandler _ConstraintHandler;
        private ChainEffectsController _Effects;
        private PositionRecorder _recorder;

        private Action OnCurrentMoveEnd;
        private Action<ChainFollower> _OnFollowerClick;
        private Action _OnFollowerRelease;
        private Action<Vector2> _OnFollowerInput;

        public ChainFollower LeadingFollower { get { return _leading; } }
        public Action<ChainFollower> OnFollowerClick { get { return _OnFollowerClick; } }
        public Action OnFollowerRelease { get { return _OnFollowerRelease; } }
        public Action<Vector2> OnFollowerInput { get { return _OnFollowerInput; } }

        private int TotalMoves = 0;
        [Space(10)]
        [SerializeField] private ChainLineTester TesterPF;
        [HideInInspector] public ChainLineTester _testerInst;

        private void Start()
        {
            if (TesterPF == null) { Debug.Log("<color=red>NO TESTER PF GIVEN </color>"); }
            else
            {
                _testerInst = Instantiate(TesterPF);
                _testerInst.Hide();
            }
            InitChain();
        }

        #region Init
        private void InitChain()
        {
            SetNodes();
            foreach (ChainFollower f in _followers)
            {
                f.Init(this, settings.followerSettings);
            }
            ConnectChainFollowers();
            GetChainSegments();
            _chainSegments.TrimExcess();
            InitSegments();
            SetChainPositions();
            end_1 = _followers[0];
            end_2 = _followers[_followers.Count - 1];
            InitLead();

            _ConstraintHandler = new ChainConstaintHandler();
            _ConstraintHandler.chain = this;

            _Effects = new ChainEffectsController(settings.ChainEffects, _chainSegments);
            _recorder = new PositionRecorder();
            GameManager.Instance._events.LevelStarted.AddListener(OnLevelStart);
            GameManager.Instance._events.LevelEndreached.AddListener(OnLevelEnd);
        }

        private void InitLead()
        {
            List<IChainFollower> chain_2 = new List<IChainFollower>(_followers.Count);
            List<IChainFollower> chain_1 = new List<IChainFollower>(_followers.Count);

            for (int i = 0; i < _followers.Count; i++)
            {
                chain_1.Add(_followers[i]);
            }

            for (int i = _followers.Count-1; i >= 0; i--)
            {
                chain_2.Add(_followers[i]);
            }
            end_1.SetupLead(chain_1);
            end_2.SetupLead(chain_2);

        }
        public List<SplineNode> GetChainNodes(ChainFollower follower)
        {
            List<SplineNode> hist = new List<SplineNode>(_followers.Count);
            if (follower == end_1)
            {
                for (int i = _followers.Count - 1; i >= 0; i--)
                {
                    hist.Add(_followers[i].currentNode);
                }

            } else if(follower == end_2)
            {
                for (int i = 0; i < _followers.Count; i++)
                {
                    hist.Add(_followers[i].currentNode);
                }
            }
            return hist;
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

        #endregion
        public void OnClick(ChainFollower follower)
        {
            foreach (ChainFollower f in _followers)
                f.Prepare();
            _OnFollowerInput = MoveByInput;
            if (follower == end_1 || follower == end_2)
                _leading = follower;
            else
                _leading = end_2;
            _leading.ResetFollower();
            _leading.SetAsLead();
            StartMovingLinks();
            _Effects?.ExecuteEffect(ChainEffects.Start);
            GameManager.Instance._events.ChainSelected.Invoke(_number);
        }

        public void OnRelease()
        {
            end_1.ReleaseEndNode();
            end_2.ReleaseEndNode();
            StopAllFollowers();
            foreach (ChainFollower f in _followers)
                f.ClearMoverHistory();
            _Effects?.ExecuteEffect(ChainEffects.Stop);
            _OnFollowerInput = null;
            _leading = null;
            GameManager.Instance._events.ChainDeselected.Invoke(_number);

        }
        public void MoveByInput(Vector2 input)
        {
            LeadingFollower.MoveLead(input);
        }

        public void MoveChain(SplineNode NexNode)
        {
        }

        public void RecordPosition()
        {
            _recorder.RecordPosision(new PositionData(_followers, TotalMoves));
            TotalMoves++;
        }
        public void SetLeadingFollower(ChainFollower follower)
        {
            end_1.ReleaseEndNode();
            end_2.ReleaseEndNode();
            if (follower == null) 
            {   
                Debug.LogError("Trying to assign lead to null");
                return;
            }
            _leading = follower;
            _leading?.ResetFollower();
            _leading?.SetAsLead();
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


        #region SegmentLinks
        public void StartMovingLinks()
        {
            foreach (ChainSegmentManager chain in _chainSegments)
            {
                if (chain != null)
                    chain.StartChainMovement();
            }
        }

        public void StopMovingLinks()
        {
            foreach (ChainSegmentManager chain in _chainSegments)
            {
                if (chain != null)
                    chain.StopChainMovement();
            }
        }
        #endregion

        #region Info
        public bool IsChainOccupied(SplineNode node)
        {
            foreach (ChainFollower n in _followers)
            {
                if (node == n.GetActualNode())
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
            info.leadingNode = _leading.GetActualNode();
            //info.testerPosition = _pathTester.currentNode;
            List<SplineNode> positions = new List<SplineNode>(_followers.Count);
            foreach (ChainFollower f in _followers)
                positions.Add(f.GetActualNode());
            info.chainNodes = positions;
            return info;
        }
        #endregion
        public void OnMoveMade()
        {
            RecordPosition();
            GameManager.Instance._events.MoveMade.Invoke();
            GameManager.Instance._sounds.PlaySingleTime(SoundNames.TrainMove.ToString());
        }

        private void StopAllFollowers()
        {
            foreach (ChainFollower f in _followers)
                f.StopNodeSnapping();
        }
        public ChainFollower ResetLead()
        {
            _leading?.ReleaseEndNode();
            if (_leading == null)
                _leading = end_2;
            else if (_leading == end_1)
                _leading = end_2;
            else if (_leading == end_2)
                _leading = end_1;
            foreach (ChainFollower f in _followers)
                f.ResetFollower();
            _leading?.SetAsLead();
            return _leading;
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
            if (finish.CompareNumbers(_number) == true)
            {
                Debug.Log("<color=green> Finish Found: " + _number.ToString() + " </color>");
                foreach (ChainFollower f in _followers)
                    f.DisableFollower();
                InitFollowerActions(true);
                end_1.SuccessLightEffect();
                end_2.SuccessLightEffect();
                FinishMatcherController.Instance.FinishReached(_number);
                _Effects?.ExecuteEffect(ChainEffects.Stop);
                GameManager.Instance._events.ChainDeselected.Invoke(_number);
                GameManager.Instance._sounds.PlaySingleTime(SoundNames.FinishCorrect.ToString());
                TrainFinishMatcher matcher = FindObjectOfType<TrainFinishMatcher>();
                if(matcher !=null) { matcher.OnMatched(_number); }
            }
            else
            {
                Debug.Log("<color=red> Wrong finish " + _number.ToString() + " </color>");
                End_1.BlockedLightEffect();
                End_2.BlockedLightEffect();
                GameManager.Instance._sounds.PlaySingleTime(SoundNames.FinishWrong.ToString());
            }
        }
        #endregion


        public void HandleConstraintMessage(string message)
        {
            switch (message)
            {
                case ConstraintMessages.WrongAngle:
                    _Effects.ExecuteEffect(ChainEffects.Shake);
                    break;
                case ConstraintMessages.Blocked:
                    end_1.BlockedLightEffect();
                    end_2.BlockedLightEffect();
                    break;
            }
        }


        #region FromEditor
        public void ConnectChainFollowers()
        {
            //if (_followers.Count < 3) { Debug.Log("Min amount of links is 3"); return; }
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
                _followers[i].OccupyNodes = true;
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
                    SetSegmentEnds(temp, _chainSegments.IndexOf(temp));
                }
            }
        }
        private void SetSegmentEnds(ChainSegmentManager segment, int index)
        {
            if(index == 0)
            {
                segment.pivot_1 = _followers[0].transform;
                segment.pivot_2 = _followers[1].transform;
            }
            else if(index == _followers.Count-2) 
            {
                segment.pivot_1 = _followers[_followers.Count - 2].transform;
                segment.pivot_2 = _followers[_followers.Count - 1].transform;
            }
            else
            {
                segment.pivot_1 = _followers[index].transform;
                segment.pivot_2 = _followers[index+1].transform;

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