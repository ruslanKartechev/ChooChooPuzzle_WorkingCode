using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CommonGame;
namespace PuzzleGame
{
    public class FinishMatcherController : SingletonMB<FinishMatcherController>
    {
        private FinishMatcher matcher;

        public void Start()
        {
            GameManager.Instance._events.LevelStarted.AddListener(OnLevelStart);
            matcher = new FinishMatcher();
            matcher.OnAllRegistered = OnAllMatched;
        }
        private void OnLevelStart()
        {
     
        }
        private void OnAllMatched()
        {
            Debug.Log("<color=green>All matched level end</color>");
            GameManager.Instance._events.LevelEndreached.Invoke();
            matcher.Refresh();
        }
        public void RegisterFinish(ChainNumber number) => matcher.RegisterFinish(number);

        public void FinishReached(ChainNumber number) => matcher.FinishReached(number);
    }
}
