using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PuzzleGame {
    public class ScoreKeeperController : MonoBehaviour
    {
        private MovesTracker movesKeeper;
        void Start()
        {
            movesKeeper = new MovesTracker();
            movesKeeper.Init();
        }

    }
}