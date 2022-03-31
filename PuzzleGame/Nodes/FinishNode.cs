using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PuzzleGame
{
    public class FinishNode : SplineNode
    {
        public Action OnFinishReached;
        [SerializeField] private ChainNumber _NodeNumber;

        private void Start()
        {
            InitConstraints();
            InitFinishNode();
        }

        public void InitFinishNode()
        {
            OnOccupied = OnFinishOccupied;
            _type = NodeType.Finish;
            gameObject.name = GONames.FinishNode;
        }
        
        public bool CompareNumbers(ChainNumber num)
        {
            if (_NodeNumber == num) 
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