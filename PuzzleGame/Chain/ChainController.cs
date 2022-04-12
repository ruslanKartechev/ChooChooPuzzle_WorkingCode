using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CommonGame;
using CommonGame.Sound;
using CommonGame.UI;
namespace PuzzleGame
{

    [DefaultExecutionOrder(100)]
    public class ChainController : MonoBehaviour
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
        private ChainEffectsManager _Effects;
        //public ChainConstaintHandler _ConstraintHandler;
        private PositionRecorder _recorder;

        private ChainFollowersController _followersController;
        private void Awake()
        {
            _Effects = new ChainEffectsManager(settings.ChainEffects,_chainSegments);
            _followersController = new ChainFollowersController(this);
            _followersController.SetData(_followers, _nodes);
        }

        private void Start()
        {
            InitChain();
            StartMovingLinks();
        }

        #region Init
        private void InitChain()
        {
            SetNodes();

            _followersController.Init(settings.followerSettings);
            _followersController.InitPositionRecorder(_recorder); //

            GetChainSegments();
            _chainSegments.TrimExcess();
            InitSegments();
            SetChainPositions();

            _recorder = new PositionRecorder();
            GameManager.Instance._events.LevelStarted.AddListener(OnLevelStart);
            GameManager.Instance._events.LevelEndreached.AddListener(OnLevelEnd);
        }

        private void OnLevelStart()
        {
            _followersController.Activate();
        }

        private void OnLevelEnd()
        {
            GameManager.Instance._events.LevelStarted.RemoveListener(OnLevelStart);
            GameManager.Instance._events.LevelFinished.RemoveListener(OnLevelEnd);
            _followersController.Deactivate();
        }
        private void InitSegments()
        {
            foreach(ChainSegmentManager segment in _chainSegments)
            {
                if (segment != null) segment.InitSegment(this);
            }
        }
        #endregion


        #region FollowersControll
        public void OnChainContolled()
        {
            _Effects?.ExecuteEffect(ChainEffects.Start, _followersController.GetChainPosition());
            FinishMatcherController.Instance.OnChainSelected(_number);
        }
        public void OnChainReleased()
        {
            _Effects?.ExecuteEffect(ChainEffects.Stop, _followersController.GetChainPosition());
            FinishMatcherController.Instance.OnChainDeselected(_number);
        }
        #endregion



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

        public void OnPositionChanged()
        {
            GameManager.Instance._events.MoveMade.Invoke();
            GameManager.Instance._sounds.PlaySingleTime(SoundNames.TrainMove.ToString());
        }

        #region NodeTypesHandler
        public void OnLeadNodeSet(SplineNode node)
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
            FinishNode finish = (FinishNode)node;
            if (finish == null) return;
            if (finish.CompareNumbers(_number) == true)
            {
                Debug.Log("<color=green> Finish Found: " + _number.ToString() + " </color>");
                foreach (ChainFollower f in _followers)
                    f.DisableFollower();
                _followersController.InitFollowerActions(true);
                _Effects?.ExecuteEffect(ChainEffects.Stop, _followersController.GetChainPosition());
                StopMovingLinks();
                _Effects.JumpTo(node.transform,OnSegmentEaten, OnChainEaten);
                FinishMatcherController.Instance.FinishReached(_number);
                GameManager.Instance._sounds.PlaySingleTime(SoundNames.FinishCorrect.ToString());
            }
            else
            {
                Debug.Log("<color=red> Wrong finish " + _number.ToString() + " </color>");
                GameManager.Instance._sounds.PlaySingleTime(SoundNames.FinishWrong.ToString());
            }
        }
        
        private void OnSegmentEaten(ChainSegmentData segment)
        {
            segment._links[0].gameObject.SetActive(false);
        }

        private void OnChainEaten()
        {
            foreach(ChainFollower f in _followers)
            {
                f.DisableFollower();
            }
            transform.parent.gameObject.SetActive(false);
            FinishMatcherController.Instance.ChainCompleted(_number);
        }
        #endregion


        public void HandleConstraintMessage(string message)
        {
            switch (message)
            {
                case ConstraintMessages.WrongAngle:
                    _Effects.ExecuteEffect(ChainEffects.Shake, _followersController.GetChainPosition());
                    break;
                case ConstraintMessages.Blocked:
                    GameManager.Instance._sounds.PlaySingleTime(SoundNames.FinishWrong.ToString());
                    break;
                case ConstraintMessages.CloseContanctBlock:
                    _Effects.ExecuteEffect(ChainEffects.Shake, _followersController.GetChainPosition());
                    CameraController.Instance.Shake();
                    GameManager.Instance._sounds.PlaySingleTime(SoundNames.FinishWrong.ToString());
                    if (_followersController.LeadingFollower== null) { Debug.Log("Leading is null, can't show cross"); return; }
                    HitCrossManager.Instance.ShowCross(_followersController.LeadingFollower.transform.position);
                    break;
            }
        }
        public void Cut(ChainSegmentManager segmentCaller, int linkIndex)
        {
            SegmentIndecesCalculator calculator = new SegmentIndecesCalculator();
            ChainCutter cutter = new ChainCutter();
            ChainCutResult res = cutter.Cut(calculator.ConvertAll(_chainSegments),
                calculator.Convert(_chainSegments, segmentCaller),
                linkIndex);
            segmentCaller.DropLinks(res.LinksToCut);
            if (res.SegmentsAway != null)
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