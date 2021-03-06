using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PuzzleGame
{

    public class ChainFollowerController : MonoBehaviour
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
        [HideInInspector]  public ChainFollower end_2;
        [HideInInspector]  public ChainFollower leadingFollower;

        [Space(10)]
        [Header("Debug")]
        public bool SnapOnStart = true;

        private void Start()
        {
            foreach(ChainFollower f in _Followers)
            {
                f.Init(this, _CommonSettings);
            }
            if (tester == null) Debug.Log("no tester");

            if (SnapOnStart)
            {
                SetNodes();
                SetChainFollowers();
            }

            end_1 = _Followers[0];
            end_2 = _Followers[_Followers.Count - 1];
            
            tester.SnapTo(_Nodes[_Nodes.Count - 1]);
            tester.Init(this, _CommonSettings);
            tester.HideForced();
        }


        #region FromEditor
        public void GetFollowers()
        {
            for(int i =0;i<transform.childCount; i++)
            {
                ChainFollower follower = transform.GetChild(i).gameObject.GetComponent<ChainFollower>();
                if(follower != null && _Followers.Contains(follower) == false)
                {
                    _Followers.Add(follower);
                }
            }
        }

        public void SetNodes()
        {
            if(_Nodes.Count != _Followers.Count)
            {
                Debug.Log("<color=red>" + "Nodes count doesn't match follwers count" + "</color>");
                return;
            }
            for(int i=0; i<_Followers.Count; i++)
            {
                if(_Nodes[i] == null) { Debug.Log("node is null"); return; }
                _Followers[i].SetCurrentNode(_Nodes[i],true);
                
            }
        }

        public void GetChainSegments()
        {
            for(int i=0; i< transform.parent.childCount; i++)
            {
                ChainSegmentManager temp = transform.parent.GetChild(i).GetComponent<ChainSegmentManager>();
                if(temp != null && _ChainSegments.Contains(temp) == false)
                {
                    _ChainSegments.Add(temp);
                }
            }
        }

        public void SetChainPositions()
        {
            foreach(ChainSegmentManager segment in _ChainSegments)
            {
                if (segment != null)
                    segment.SetPositions();
            }



        }
        #endregion


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
        public void MoveChainFollwoers(Vector2 input)
        {
            if(leadingFollower != null)
                leadingFollower.MoveLead(input);
        }
        public void MoveChainFollwoers(SplineNode testerNode)
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

        public ChainFollower GetOtherEnd()
        {
            if (leadingFollower == null) { Debug.Log("no leading yet"); return end_1; }
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
            if (lead != leadingFollower && leadingFollower != null)
                StopChainFollowers();
            leadingFollower = lead;
            SplineNode node = lead.GetLastNode();
            return node;
        }

        private void StopChainFollowers()
        {
            foreach (ChainFollower f in _Followers)
                f.StopMovingToNode();
        }

        public void SetChainFollowers()
        {
            if (_Followers.Count < 3) {Debug.Log("Min amount of links is 3"); return; }
            _Followers[0].AddLink(_Followers[1]);
            for(int i = 1; i< _Followers.Count; i++)
            {
                _Followers[i].AddLink(_Followers[i-1]);
                if (i < _Followers.Count - 1)
                    _Followers[i].AddLink(_Followers[i + 1]);
            }
        }



        public void StartMovingChain()
        {
            foreach(ChainSegmentManager chain in _ChainSegments)
            {
                chain.StartChainMovement();
            }
        }

        public void StopMovingChain()
        {
            foreach (ChainSegmentManager chain in _ChainSegments)
            {
                chain.StopChainMovement();
            }
        }

    }

    [CustomEditor(typeof(ChainFollowerController))]
    public class ChainFollowerControllerEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ChainFollowerController me = target as ChainFollowerController;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("GetFollowers"))
                me.GetFollowers();
            if (GUILayout.Button("SetChain"))
                me.SetChainFollowers();
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