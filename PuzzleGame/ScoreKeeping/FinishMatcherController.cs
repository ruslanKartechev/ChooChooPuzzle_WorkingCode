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
        private FinishCounter counter;
        private List<FinishViewController> views = new List<FinishViewController>();
        private FinishViewController currentView;
        public void Start()
        {
            counter = new FinishCounter();
            counter.OnAllRegistered = OnAllMatched;
        }

        public void RegisterFinish(ChainNumber number) => counter.RegisterFinish(number);
        public void RegisterFinishView(FinishViewController view)
        {
            if (views.Contains(view) == false)
                views.Add(view);
        }


        public void OnChainSelected(ChainNumber number)
        {
            FinishViewController v = views.Find(t => t != null && t.number == number); 
            if (v == null) { Debug.Log("Corresponding finish view is not registered"); return; }
            currentView = v;
            currentView.Activate();
        }

        public void OnChainDeselected(ChainNumber number)
        {
            if (currentView != null) currentView.Deactivate();
            else
            {
                FinishViewController v = views.Find(t => t.number == number);
                if (v == null) { Debug.Log("Corresponding finish view is not registered"); return; }
                v.Deactivate();
            }
        }

        private void OnAllMatched()
        {
            Debug.Log("<color=green>All matched level end</color>");
            GameManager.Instance._events.LevelEndreached.Invoke();
            if (currentView != null) currentView.Deactivate();
            Refresh();
        }

        public void ChainFinished(ChainNumber number)
        {
            counter.FinishReached(number);
            FinishViewController v = views.Find(t => t.number == number);
            if (v == null) { Debug.Log("Corresponding vew was not found" + number.ToString());return; }
            counter.FinishReached(number);
        }
        public void ChainCompleted(ChainNumber number)
        {
            if(currentView != null)
            {
                currentView.Deactivate();
            }
            else
            {
                FinishViewController v = views.Find(t => t.number == number);
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
