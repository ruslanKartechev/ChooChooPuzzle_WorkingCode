using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonGame;
namespace PuzzleGame
{

    public class FinishEffectsController : MonoBehaviour, IEffect
    {
        private FinishEffectsData Data;
        private void Start()
        {
         
        }
        public void Init()
        {
            Data = GetComponent<FinishEffectsData>();
            GameManager.Instance._events.LevelEndreached.AddListener(OnLevelEnd);
            Data.ConfettiManager.InitParticles(Data.Confetti);
        }
        public void Play()
        {
            Data.ConfettiManager?.ShootOnce();
        }
        public void OnLevelEnd()
        {
            Data.ConfettiManager?.ShootLoop();
        }
    }
}
