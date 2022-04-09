using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
namespace PuzzleGame
{
    [DefaultExecutionOrder(0)]
    public class SplineNode : MonoBehaviour
    {
        [SerializeField] protected NodeType _type;
        [SerializeField] private bool HasAngleConstr;
        [SerializeField] private AngleConstraint AngleConst;
        public List<SplineNode> linkedNodes;
        private List<IConstrained> mConstraints = new List<IConstrained>();
        private ISplineFollower _currentFollower = null;
        protected Action OnOccupied;

        public NodeType Type { get { return _type; } }
        public Vector3 _position { get { return transform.position; } }
        public bool IsOccupied
        {
            get
            {
                if (currentFollower == null)
                    return false;
                else
                    return true;
            }
        }
        public ISplineFollower currentFollower { get { return _currentFollower; } }
      
        public List<IConstrained> _Constraints { get { return mConstraints; } }
        public bool HasAngleConstraint { get { return HasAngleConstr; } }

        private void Start()
        {
            InitConstraints();
        }
        protected void InitConstraints()
        {
            AddBaseConstr();
            if (HasAngleConstr)
                AddAngleConstr();
        }

        #region Constraints
        private void AddBaseConstr()
        {
            BaseConstraint bc = new BaseConstraint();
            mConstraints.Add(bc);
        }
        private void AddAngleConstr()
        {
            mConstraints.Add(AngleConst);
        }
        #endregion


        public void ConnectNode(SplineNode node)
        {
            if (linkedNodes == null) linkedNodes = new List<SplineNode>();
            if(linkedNodes.Contains(node) == false && node != this)
                linkedNodes.Add(node);
        }


        public NodePushResult PushFromNode(ISplineFollower pusher)
        {
            NodePushResult result = new NodePushResult();
            result.success = true;
            result.message = NodePushMessage.PushSucess.ToString();
            if (currentFollower != null)
            {
                result = currentFollower.PushFromNode(pusher);
            }
            else
                Debug.Log("no current follower");

            return result;
        }

        public bool SetCurrentFollower(ISplineFollower follower)
        {
            if (_currentFollower == null)
            {
                OnOccupied?.Invoke();
                _currentFollower = follower;
                return true;
            }
            Debug.Log("<color=red>DO NOT ALLOW OCCUPY</color>");
            return false;
        }
        public void SetCurrentFollowerForced(ISplineFollower follower) => _currentFollower = follower;

        public virtual void ReleaseNode()
        {
            _currentFollower = null;
        }
    }

}
