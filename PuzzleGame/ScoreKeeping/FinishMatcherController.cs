using System.Collections.Generic;
using CommonGame.Events;
using UnityEngine;
using CommonGame;
namespace PuzzleGame
{
    public class FinishMatcherController : SingletonMB<FinishMatcherController>
    {
        [SerializeField] private LevelStartChannelSO _levelStartChannel;
        [SerializeField] private LevelFinishChannelSO _levelFinishChannel;
        [Space(10)]
        private FinishCounter counter;
        private List<FinishHeadView> views = new List<FinishHeadView>();
        private FinishHeadView currentView;
        public void Awake()
        {
            counter = new FinishCounter();
            counter.OnAllRegistered = OnAllMatched;
        }

        public void RegisterChain(ChainNumber number) => counter.Register(number);
        public void RegisterFinishView(FinishHeadView view)
        {
            if (views.Contains(view) == false)
                views.Add(view);
        }
        public void ChainFinished(ChainNumber number)
        {
            counter.SetFinished(number);
        }

        public void OnChainSelected(ChainNumber number)
        {
            FinishHeadView v = views.Find(t => t != null && t._number == number); 
            if (v == null) { Debug.Log("Corresponding finish view is not registered"); return; }
            currentView = v;
            currentView.Activate();
        }

        public void OnChainDeselected(ChainNumber number)
        {
            if (currentView != null) currentView.Deactivate();
            else
            {
                FinishHeadView v = views.Find(t => t._number == number);
                if (v == null) { Debug.Log("Corresponding finish view is not registered"); return; }
                v.Deactivate();
            }
        }

        private void OnAllMatched()
        {
            Debug.Log("<color=green>All matched level end</color>");
            _levelFinishChannel.RaiseEvent();
            if (currentView != null) currentView.Deactivate();
            Refresh();
        }

        public void ChainCompleted(ChainNumber number)
        {
            if(currentView != null)
            {
                currentView.Deactivate();
            }
            else
            {
                FinishHeadView v = views.Find(t => t._number == number);
                v?.Deactivate();
            }
        }

        private void Refresh()
        {
            counter.Refresh();
            views.Clear();
            currentView = null;
        }


    }
}
