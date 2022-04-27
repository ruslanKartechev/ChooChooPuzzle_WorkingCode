using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonGame
{
    [CreateAssetMenu(fileName = "GameStateSO", menuName = "SO/GameStateSO", order = 1)]
    public class GameStateSO : ScriptableObject
    {
        public int CurrentLevelIndex { get; private set; }
        public void SetLevelIndex(int ind)
        {
            CurrentLevelIndex = ind;
        }
    }
}