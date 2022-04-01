using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PuzzleGame
{
    public class FinishNode : SplineNode
    {
        public Action OnFinishReached;
        [SerializeField] private ChainNumber _number;
        public ChainNumber Number { get { return _number; } }

        [Space(10)]
        [SerializeField] private List<Transform> _finishTrack = new List<Transform>();
        public List<Transform> FinishTrack { get { return _finishTrack; } }

        private void Start()
        {
            InitConstraints();
            InitFinishNode();
            FinishMatcherController.Instance.RegisterFinish(Number);
        }

        public void InitFinishNode()
        {
            OnOccupied = OnFinishOccupied;
            _type = NodeType.Finish;
            gameObject.name = GONames.FinishNode;
        }
        
        public bool CompareNumbers(ChainNumber num)
        {
            if (_number == num) 
            {
                OnFinishOccupied();
                return true;
            }
            else
                return false;
        }

        public void OnFinishOccupied()
        {
            OnFinishReached?.Invoke();
        }

    }
}