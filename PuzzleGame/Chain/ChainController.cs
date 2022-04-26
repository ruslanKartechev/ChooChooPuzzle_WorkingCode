using System.Collections.Generic;
using UnityEngine;
using CommonGame;
using CommonGame.Sound;
using CommonGame.UI;
using CommonGame.Events;
namespace PuzzleGame
{

    [DefaultExecutionOrder(100)]
    public class ChainController : MonoBehaviour
    {
        [SerializeField] private LevelStartChannelSO _levelStartChannel;
        [SerializeField] private LevelFinishChannelSO _levelFinishChannel;
        [SerializeField] private SoundFXChannelSO _soundFXChannel;
        [Space(10)]
        [SerializeField] private ChainSettings settings;
        [Space(10)]
        [SerializeField] private bool IsEatable = true;
        [SerializeField] private ChainNumber _number;
        [Header("Pivot Followers")]
        [Space(10)]
        [SerializeField] private List<ChainFollower> _followers = new List<ChainFollower>();
        [Header("Start nodes")]
        [SerializeField] private List<SplineNode> _nodes = new List<SplineNode>();
        [Header("Chain segment managers")]
        [SerializeField] private List<ChainSegmentManager> _chainSegments = new List<ChainSegmentManager>();
        [Space(10)]
        [SerializeField] ChainFinishEffectBase ChainFinishEffect;
        [Space(10)]
        public bool InitSegmentPivots = true;

        private ChainEffectsManager _Effects;
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
            _followersController.InitPositionRecorder(_recorder); 

            GetChainSegments();
            _chainSegments.TrimExcess();
            SetChainPositions();

            _recorder = new PositionRecorder();
            _levelStartChannel.OnLevelStarted.AddListener(OnLevelStart);
            _levelFinishChannel.OnLevelFinished += OnLevelEnd;
        }

        private void OnLevelStart()
        {
            _followersController.Activate();
        }

        private void OnLevelEnd()
        {
            _followersController.Deactivate();
        }
        #endregion


        #region FollowersControll
        public void OnChainContolled()
        {
            _Effects?.ExecuteEffect(ChainEffects.Start, _followersController.GetChainPosition());
            if(IsEatable)
                FinishMatcherController.Instance.OnChainSelected(_number);
        }
        public void OnChainReleased()
        {
            _Effects?.ExecuteEffect(ChainEffects.Stop, _followersController.GetChainPosition());
            if(IsEatable)
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

        private FinishNode mFinish;
        public void HandleFinishNode(SplineNode node)
        {
            mFinish = (FinishNode)node;
            if (mFinish == null) return;
            if (mFinish.CompareNumbers(_number) == true && IsEatable)
            {
                Debug.Log("<color=green> Finish Found: " + _number.ToString() + " </color>");
                foreach (ChainFollower f in _followers)
                    f.DisableFollower();
                _followersController.InitFollowerActions(true);

                if (ChainFinishEffect != null)
                {
                    ChainFinishEffect.OnEffectEnd = OnFinishEffectEnd;
                    ChainFinishEffect.ExecuteEffect(mFinish.FinishPoint.position);
                }
                else
                {
                    _Effects?.ExecuteEffect(ChainEffects.Stop, _followersController.GetChainPosition());
                    _Effects.JumpTo(mFinish.FinishPoint, OnSegmentEffectEnd, OnFinishEffectEnd);
                }
                mFinish._FinishView?.OnReached();
               // CameraController.Instance.MoveToFinishCP(_number);
                FinishMatcherController.Instance.ChainFinished(_number);
                _soundFXChannel.RaiseEventPlay(SoundNames.SpaghettiEat.ToString());
            }
            else
            {
                Debug.Log("<color=red> Wrong finish " + _number.ToString() + " </color>");
                mFinish?._FinishView.OnWrong();
                HitCrossManager.Instance.ShowCross(node.transform.position);
              //  CameraController.Instance.Shake();
                _soundFXChannel.RaiseEventPlay(SoundNames.FinishWrong.ToString());
            }
        }
        
        public void OnSegmentEffectEnd(ChainSegmentData segment)
        {

        }
        public void OnFinishEffectEnd()
        {
            mFinish?._FinishView?.OnCompleted();
            foreach (ChainFollower f in _followers)
            {
                f.DisableFollower();
            }
            _soundFXChannel.RaiseEventPlay(SoundNames.FinishCorrect.ToString());
            FinishMatcherController.Instance.ChainCompleted(_number);
          //  CameraController.Instance.MoveToDefaultPos();
            StopMovingLinks();
            transform.parent.gameObject.SetActive(false);
        }
        #endregion


        public void HandleConstraintMessage(string message, ChainFollower caller)
        {
            switch (message)
            {
                case ConstraintMessages.WrongAngle:
                    _Effects.ExecuteEffect(ChainEffects.Shake, _followersController.GetChainPosition());
                    break;
                case ConstraintMessages.Blocked:
                    _soundFXChannel.RaiseEventPlay(SoundNames.FinishWrong.ToString());
                    break;
                case ConstraintMessages.CloseContanctBlock:
                    _Effects.ExecuteEffect(ChainEffects.Shake, _followersController.GetChainPosition());
                  //  CameraController.Instance.Shake();
                    _soundFXChannel.RaiseEventPlay(SoundNames.FinishWrong.ToString());
                    HitCrossManager.Instance.ShowCross(caller.transform.position);
                    break;
            }
        }
   

        #region FromEditor
        public void ConnectChainFollowers()
        {
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
                    SetSegmentPivots(temp, _chainSegments.IndexOf(temp));
                }
            }
        }
        private void SetSegmentPivots(ChainSegmentManager segment, int index)
        {
            if (InitSegmentPivots == false) return;
            if(index == 0)
            {
                segment.InitSegment(_followers[0].transform, _followers[1].transform);
            }
            else if(index == _followers.Count-2) 
            {
                segment.InitSegment(_followers[_followers.Count - 2].transform, _followers[_followers.Count - 1].transform);
            }
            else
            {
                segment.InitSegment(_followers[index].transform, _followers[index+1].transform);
            }
        }


        public void SetChainPositions()
        {

            foreach (ChainSegmentManager segment in _chainSegments)
            {
                if (segment != null)
                    segment.UpdateSegment();
            }
        }
        #endregion

    }

  
}