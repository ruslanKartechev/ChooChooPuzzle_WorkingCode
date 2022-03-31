using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CommonGame;

namespace PuzzleGame
{
    public class MovesTracker
    {

        public int TotalMoves = 0;
        public void Init()
        {
            GameManager.Instance._events.MoveMade.AddListener(OnMoveMade);
            TotalMoves = 0;
        }

        private void OnMoveMade()
        {
            TotalMoves++;
            //Debug.Log("<color=blue>Total Moves: " + TotalMoves + "</color>");
        }




    }
}
