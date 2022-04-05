using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CommonGame;
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
        private IEffect effects;
        private void Start()
        {
            effects = GetComponentInChildren<IEffect>();
            if(effects == null) { Debug.Log("<color=red>Finish Node effects are not assigned"); }
            effects?.Init();
            InitConstraints();
            InitFinishNode();
            GameManager.Instance._events.LevelStarted.AddListener(OnGameStart);
        }
        private void OnGameStart()
        {
            FinishMatcherController.Instance.RegisterFinish(Number);
        }

        public void InitFinishNode()
        {
            _type = NodeType.Finish;
            gameObject.name = GONames.FinishNode;
        }
        
        public bool CompareNumbers(ChainNumber num)
        {
            if (_number == num) 
            {
                OnCorrectOccupied();
                return true;
            }
            else
                return false;
        }


        public void OnCorrectOccupied()
        {
            effects?.Play();
            OnFinishReached?.Invoke();
        }

    }
}