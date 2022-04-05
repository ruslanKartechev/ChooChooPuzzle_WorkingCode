using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonGame;
using System;
namespace PuzzleGame {

    public interface ICounter
    {
        int GetCount();
        void SubscribeOnValueChange(Action action);
    }
    public class ScoreKeeperController : MonoBehaviour, ICounter
    {
        private MovesTracker _movesKeeper;
        private Action ValueChanged;
        private List<Action> _listeners = new List<Action>();
        private void Awake()
        {
            ValueChanged = OnCounterTick;
            _movesKeeper = new MovesTracker();
            _movesKeeper.Init(ValueChanged);
            GameManager.Instance._events.LevelLoaded.AddListener(OnNewLevel);
            GameManager.Instance._events.MoveMade.AddListener(OnMoveMade);
        }

        private void OnNewLevel()
        {
            _movesKeeper.Refresh();
        }
        private void OnMoveMade()
        {
            _movesKeeper.AddCount();
        }
        
        public int GetCount()
        {
            return _movesKeeper.TotalMoves;
        }
        private void OnCounterTick()
        {
            foreach (Action a in _listeners)
            {
                a?.Invoke();
            }
                
        }
        public void SubscribeOnValueChange(Action action)
        {
            if (_listeners.Contains(action) == false)
                _listeners.Add(action);
        }
    }
}