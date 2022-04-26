using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PuzzleGame
{
    public class FinishCounter
    {
        private List<ChainNumber> _started = new List<ChainNumber>();
        private List<ChainNumber> _finished = new List<ChainNumber>();
        public Action OnAllRegistered;
        public void Refresh()
        {
            _started.Clear();
            _finished.Clear();
        }
        
        public void Register(ChainNumber num)
        {
            _started.Add(num);
        }
        public void SetFinished(ChainNumber num)
        {
            _finished.Add(num);

            CheckFinishAll();
        }
        public void CheckFinishAll()
        {
            if (_finished.Count != 0 && (_finished.Count >= _started.Count))
                OnAllRegistered?.Invoke();
        }
    }
}