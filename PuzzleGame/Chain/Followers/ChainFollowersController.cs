using System;
using System.Collections.Generic;
using UnityEngine;
namespace PuzzleGame
{
    public class ChainFollowersController: IChainMovable
    {
        private ChainController _Controller;

        private ChainFollower _leading;
        private ChainFollower end_1;
        private ChainFollower end_2;
        public ChainFollower End_1 { get { return end_1; } }
        public ChainFollower End_2 { get { return end_2; } }
        public ChainFollower LeadingFollower { get { return _leading; } }
        [SerializeField] private List<ChainFollower> _followers = new List<ChainFollower>();
        [SerializeField] private List<SplineNode> _nodes = new List<SplineNode>();

        private PositionRecorder _recorder;
        private int TotalMoves = 0;
        public ChainConstaintHandler _ConstraintHandler;
        //
        private Action<ChainFollower> _OnFollowerClick;
        private Action _OnFollowerRelease;
        private Action<Vector2> _OnFollowerInput;
        public Action<ChainFollower> OnFollowerClick { get { return _OnFollowerClick; } }
        public Action OnFollowerRelease { get { return _OnFollowerRelease; } }
        public Action<Vector2> OnFollowerInput { get { return _OnFollowerInput; } }
        public ChainFollowersController(ChainController controller)
        {
            _Controller = controller;
        }

        public void SetData(List<ChainFollower> followers, List<SplineNode> nodes)
        {
            _followers = followers;
            _nodes = nodes;
        }
        private void ConnectChainFollowers()
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
        public void Init(FollowerSettings settings)
        {
            foreach (ChainFollower f in _followers)
            {
                f.Init(this, settings);
            }
            end_1 = _followers[0];
            end_2 = _followers[_followers.Count - 1];
            ConnectChainFollowers();
            InitLead();

            _ConstraintHandler = new ChainConstaintHandler();
            _ConstraintHandler.chain = this;
        }
        #region PositionRecording
        public void InitPositionRecorder(PositionRecorder recorder)
        {
            _recorder = recorder;
        }
        public void RecordPosition()
        {
            _recorder?.RecordPosision(new PositionData(_followers, TotalMoves));
            TotalMoves++;
        }
        #endregion

        #region LeadFollower
        private void InitLead()
        {
            List<IChainFollower> chain_2 = new List<IChainFollower>(_followers.Count);
            List<IChainFollower> chain_1 = new List<IChainFollower>(_followers.Count);

            for (int i = 0; i < _followers.Count; i++)
            {
                chain_1.Add(_followers[i]);
            }

            for (int i = _followers.Count - 1; i >= 0; i--)
            {
                chain_2.Add(_followers[i]);
            }
            end_1.SetupLead(chain_1);
            end_2.SetupLead(chain_2);
        }
        public void SetLeadingFollower(ChainFollower follower)
        {
            end_1.ReleaseEndNode();
            end_2.ReleaseEndNode();
            if (follower == null)
            {
                _leading = null;
                return;
            }
            _leading = follower;
            _leading?.ResetFollower();
            _leading?.SetAsLead();
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
        #endregion
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
            _Controller.OnChainContolled();
        }

        public void OnRelease()
        {
            foreach (ChainFollower f in _followers)
            {
                f.ClearMoverHistory();
                //f.OnChainRelease();
            }
            end_1.ReleaseEndNode();
            end_2.ReleaseEndNode();
            StopAllFollowers();
 

            _OnFollowerInput = null;
            _leading = null;
            _Controller.OnChainReleased();
        }

        public void MoveByInput(Vector2 input)
        {
            LeadingFollower.MoveLead(input);
        }

        public void OnMoveMade()
        {
            RecordPosition();
            _Controller.OnPositionChanged();
            _Controller.OnLeadNodeSet(_leading.currentNode); ///
        }
        public void OnLeadNodeSet(SplineNode node)
        {
            _Controller.OnLeadNodeSet(node);
        }

        private void StopAllFollowers()
        {
            foreach (ChainFollower f in _followers)
                f.StopNodeSnapping();
        }

        #region INFO
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
            if (_leading == null)
                return null;
            info.leadingNode = _leading.GetActualNode();
            //info.testerPosition = _pathTester.currentNode;
            List<SplineNode> positions = new List<SplineNode>(_followers.Count);
            foreach (ChainFollower f in _followers)
                positions.Add(f.GetActualNode());
            info.chainNodes = positions;
            return info;
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

            }
            else if (follower == end_2)
            {
                for (int i = 0; i < _followers.Count; i++)
                {
                    hist.Add(_followers[i].currentNode);
                }
            }
            return hist;
        }
        #endregion

        public void HandleConstraintMessage(string message, ChainFollower caller)
        {
            _Controller.HandleConstraintMessage(message, caller);
            switch (message)
            {
                case ConstraintMessages.WrongAngle:
                    break;
                case ConstraintMessages.Blocked:
                    end_1.BlockedLightEffect();
                    end_2.BlockedLightEffect();
                    break;
                case ConstraintMessages.CloseContanctBlock:
                    if (_leading == null) { Debug.Log("Leading is null, can't show cross"); return; }
                    break;
            }
        }

        public void Activate() { InitFollowerActions(false); }
        public void Deactivate() { InitFollowerActions(true); }

    }


}
