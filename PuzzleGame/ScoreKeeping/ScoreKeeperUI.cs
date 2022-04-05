using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonGame;
namespace PuzzleGame
{
    public class ScoreKeeperUI : MonoBehaviour
    {
        private ICounter counter;
        private void Start()
        {
            counter = GetComponent<ICounter>();
            if (counter == null) {Debug.Log("ICounter was not found"); return;}
            counter.SubscribeOnValueChange(OnCountChange);
        }

        
        public void OnCountChange()
        {
            int num = counter.GetCount();
            GameManager.Instance._ui.progressPanel.OutputNumber(num);
        }
        

    }
}