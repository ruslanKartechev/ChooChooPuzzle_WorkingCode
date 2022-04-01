using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PuzzleGame
{
    public class FinishMatcher
    {
        public List<ChainNumber> chains = new List<ChainNumber>();
        public List<ChainNumber> finishes = new List<ChainNumber>();
        public Action OnAllRegistered;
        public void Refresh()
        {
            chains = new List<ChainNumber>();
            finishes = new List<ChainNumber>();
        }
        
        public void RegisterFinish(ChainNumber num)
        {
            if (finishes.Contains(num) == false)
                finishes.Add(num);
        }
        public void FinishReached(ChainNumber num)
        {
            if (chains.Contains(num) == false)
            {
                if (finishes.Contains(num) == true)
                    chains.Add(num);
                CheckFinishAll();
            }
        }
        public void CheckFinishAll()
        {
            if (finishes.Count != 0 && (finishes.Count == chains.Count))
                OnAllRegistered?.Invoke();
        }
    }
}