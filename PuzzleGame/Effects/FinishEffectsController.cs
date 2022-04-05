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
            Data = GetComponent<FinishEffectsData>();
            GameManager.Instance._events.LevelEndreached.AddListener(OnLevelEnd);
        }
        public void Init()
        {
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
