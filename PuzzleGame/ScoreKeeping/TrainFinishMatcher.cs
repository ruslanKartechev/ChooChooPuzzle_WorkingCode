using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonGame;
namespace PuzzleGame {

    public class TrainFinishMatcher : MonoBehaviour
    {
        public List<FinishArrowMatchData> FinishByNumber = new List<FinishArrowMatchData>();

        private void Start()
        {
            GameManager.Instance._events.ChainSelected.AddListener(OnChainSelected);
            GameManager.Instance._events.ChainDeselected.AddListener(OnChainDeselected);
        }

        public void InitArrow(ChainNumber num, FinishArrowController arrow)
        {
            if(FinishByNumber.Exists(t => t.Number == num) == false)
            {
                FinishByNumber.Add(new FinishArrowMatchData(num,arrow));
            }
        }

        private void OnChainSelected(ChainNumber num)
        {
            var arrow = GetArrowByNum(num);
            if (arrow == null) return;
            arrow.Activate();
        }

        private void OnChainDeselected(ChainNumber num)
        {
            var arrow = GetArrowByNum(num);
            if (arrow == null) return;
            arrow.Deactivate();

        }
        private FinishArrowController GetArrowByNum(ChainNumber num)
        {
            var result =  FinishByNumber.Find(t => t.Number == num).Arrow;
            if (result == null) Debug.Log("did not find the result");
            return result;

        }

        public void OnMatched(ChainNumber num)
        {
            var result = FinishByNumber.Find(t => t.Number == num).Arrow;
            if (result == null) Debug.Log("did not find the result");
            result?.OnlyLight();
        }

    }
}