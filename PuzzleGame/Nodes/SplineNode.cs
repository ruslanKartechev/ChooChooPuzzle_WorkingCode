using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
namespace PuzzleGame
{

    public class SplineNode : MonoBehaviour
    {
        [SerializeField] protected NodeType _type;
        public NodeType Type { get { return _type; } }


        [SerializeField] private bool HasAngleConstr;
        [SerializeField] private AngleConstraint AngleConst;
        public List<SplineNode> linkedNodes;
        private List<IConstrained> mConstraints = new List<IConstrained>();
        public Vector3 _position { get { return transform.position; } }
        protected Action OnOccupied;
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
        public ISplineFollower currentFollower { get; set; }
        public List<IConstrained> _Constraints { get { return mConstraints; } }


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

        public bool PushFromNode(Vector2 dir)
        {
            bool allow = true;
            if(currentFollower != null)
            {
                allow = currentFollower.PushFromNode(dir);
            }
            return allow;
         
        }
        public bool SetCurrentFollower(ISplineFollower follower) 
        {
            if(currentFollower == null)
            {
                OnOccupied?.Invoke();
                currentFollower = follower;
                return true;
            }
            return false;
        }
        public void SetCurrentFollowerForce(ISplineFollower follower) => currentFollower = follower;

        public virtual void ReleaseNode()
        {
            currentFollower = null;
        }
    }

}
