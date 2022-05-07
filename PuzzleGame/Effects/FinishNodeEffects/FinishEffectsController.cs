using CommonGame.Events;
using UnityEngine;
using CommonGame;
namespace PuzzleGame
{

    public class FinishEffectsController : MonoBehaviour, IEffect
    {
        private FinishEffectsData Data;
        [SerializeField] private LevelFinishChannelSO _levelFinishChannel;
        private void Start()
        {
         
        }
        public void Init()
        {
            Data = GetComponent<FinishEffectsData>();
            _levelFinishChannel.OnLevelFinished.AddListener(OnLevelEnd);
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
