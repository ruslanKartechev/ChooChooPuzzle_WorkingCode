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
        public FollowerSettings _CommonSettings;
        [Header("Pivot Followers")]
        public List<ChainFollower> _Followers = new List<ChainFollower>();

        [Header("Start nodes")]
        public List<SplineNode> _Nodes = new List<SplineNode>();
        [Header("Chain segment managers")]
        public List<ChainSegmentManager> _ChainSegments = new List<ChainSegmentManager>();
        [Space(10)]
        public PathTester tester;

        [HideInInspector] public ChainFollower end_1;
        [HideInInspector] public ChainFollower end_2;
        [HideInInspector] public ChainFollower leadingFollower;

        [Header("SnapOnStart")]
        [Space(10)]
        public bool SnapOnStart = true;


        [HideInInspector] public ChainPositionInfo mPosInfo;
        public ChainConstaintHandler _ConstraintHandler;


        private void Start()
        {
            foreach(ChainFollower f in _Followers)
            {
                f.Init(this, _CommonSettings);
            }
            _ChainSegments.TrimExcess();
            InitSegments();

            if (tester == null) Debug.Log("no tester");

            SetNodes();
            SetChainPositions();

            end_1 = _Followers[0];
            end_2 = _Followers[_Followers.Count - 1];
            
            tester.SnapTo(_Nodes[_Nodes.Count - 1]);
            tester.Init(this, _CommonSettings);
            tester.HideForced();

            _ConstraintHandler = new ChainConstaintHandler();
            _ConstraintHandler.chain = this;
        }






        private void InitSegments()
        {
            foreach(ChainSegmentManager segment in _ChainSegments)
            {
                if (segment != null) segment.InitSegment(this);
            }
        }



        public virtual void OnMoveMade()
        {
            GameManager.Instance._events.MoveMade.Invoke();
        }


        public void OnClick(ChainFollower follower)
        {
            tester.SnapTo(follower.currentNode);
            tester.HideForced();
            StartMovingChain();
        }
        public void OnRelease()
        {
            tester.Hide();
            tester.OnMoveEnd();
            leadingFollower = null;
            StopMovingChain();
        }
        public void MoveChain(SplineNode testerNode)
        {
            if (leadingFollower != null)
                leadingFollower.MoveLead(testerNode);
        }
        public void MoveTesterBy(Vector2 input)
        {
            tester.TakeInput(input);
        }
        
        public SplineNode GetOtherEndNode()
        {
            return GetOtherEnd().currentNode;
        }

        public void Cut(ChainSegmentManager segmentCaller, int linkIndex)
        {
            SegmentIndecesCalculator calculator = new SegmentIndecesCalculator();
            ChainCutter cutter = new ChainCutter();
            ChainCutResult res =  cutter.Cut(calculator.ConvertAll(_ChainSegments), 
                calculator.Convert(_ChainSegments, segmentCaller), 
                linkIndex);
            segmentCaller.DropLinks(res.LinksToCut);
            if(res.SegmentsAway != null)
            {
                foreach (int i in res.SegmentsAway)
                {
                    if (_ChainSegments[i] != null)
                        _ChainSegments[i].DropAllLinks();
                    _ChainSegments[i] = null;
                }
                _ChainSegments.RemoveAll(x => x = null);
            }
   
        }

        public ChainFollower GetOtherEnd()
        {
            if (leadingFollower == null) { Debug.Log("No Leading Follower"); return end_1; }
            if (leadingFollower == end_1)
            {
                leadingFollower = end_2;
            }
            else
            {
                leadingFollower = end_1;
            }
            return leadingFollower;
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
                    if (end_1.GetLastNode()._position.y >= end_2.GetLastNode()._position.y)
                        lead = end_1;
                    else
                        lead = end_2;
                }
                else
                {
                    if (end_1.GetLastNode()._position.y < end_2.GetLastNode()._position.y)
                        lead = end_1;
                    else
                        lead = end_2;
                }
            }
            leadingFollower = lead;
            SplineNode node = lead.GetLastNode();
            return node;
        }
        public void ConnectChainFollowers()
        {
            if (_Followers.Count < 3) {Debug.Log("Min amount of links is 3"); return; }
            _Followers[0].ResetLinks();
            _Followers[0].AddLink(_Followers[1]);
            for(int i = 1; i< _Followers.Count; i++)
            {
                _Followers[i].ResetLinks();
                _Followers[i].AddLink(_Followers[i-1]);
                if (i < _Followers.Count - 1)
                    _Followers[i].AddLink(_Followers[i + 1]);
            }
        }


        
        public void StartMovingChain()
        {
            foreach (ChainSegmentManager chain in _ChainSegments)
            {
                if(chain != null)
                    chain.StartChainMovement();
            }
        }

        public void StopMovingChain()
        {
            foreach (ChainSegmentManager chain in _ChainSegments)
            {
                if (chain != null)
                    chain.StopChainMovement();
            }
        }


        public ChainPositionInfo GetChainPosition()
        {
            ChainPositionInfo info = new ChainPositionInfo();
            info.leadingNode = leadingFollower.GetLastNode();
            List<SplineNode> positions = new List<SplineNode>(_Followers.Count);
            foreach (ChainFollower f in _Followers)
                positions.Add(f.GetLastNode());
            info.chainNodes = positions;
            return info;
        }






        #region FromEditor
        public void GetFollowers()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                ChainFollower follower = transform.GetChild(i).gameObject.GetComponent<ChainFollower>();
                if (follower != null && _Followers.Contains(follower) == false)
                {
                    _Followers.Add(follower);
                }
            }
        }

        public void SetNodes()
        {
            if (_Nodes.Count != _Followers.Count)
            {
                Debug.Log("<color=red>" + "Nodes count doesn't match follwers count" + "</color>");
                return;
            }
            for (int i = 0; i < _Followers.Count; i++)
            {
                if (_Nodes[i] == null) { Debug.Log("node is null"); return; }
                _Followers[i].SetCurrentNode(_Nodes[i], true);

            }
        }

        public void GetChainSegments()
        {
            _ChainSegments = new List<ChainSegmentManager>();
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                ChainSegmentManager temp = transform.parent.GetChild(i).GetComponent<ChainSegmentManager>();
                if (temp != null && _ChainSegments.Contains(temp) == false)
                {
                    _ChainSegments.Add(temp);
                }
            }
        }

        public void SetChainPositions()
        {

            foreach (ChainSegmentManager segment in _ChainSegments)
            {
                if (segment != null)
                    segment.SetPositions();
            }



        }
        #endregion




    }

    [CustomEditor(typeof(ChainController))]
    public class ChainFollowerControllerEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ChainController me = target as ChainController;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("GetFollowers"))
                me.GetFollowers();
            if (GUILayout.Button("SetChain"))
                me.ConnectChainFollowers();
            GUILayout.EndHorizontal();
            if (GUILayout.Button("SetNodes"))
                me.SetNodes();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("GetSegments"))
                me.GetChainSegments();
            if (GUILayout.Button("SetChainPositions"))
                me.SetChainPositions();
            GUILayout.EndHorizontal();
        }
    }
}