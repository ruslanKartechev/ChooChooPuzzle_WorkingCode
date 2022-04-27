using UnityEngine;
using CommonGame.Data;
using CommonGame.Events;
using CommonGame;
namespace CommonGame.UI
{
    [DefaultExecutionOrder(99)]
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private bool SelfInit = false;
        [Space(5)]
        [SerializeField] private GameStateSO _gameState;
        [Space(5)]
        [SerializeField] private LevelStartChannelSO _levelStartChannel;
        [SerializeField] private LevelLoadChannelSO _levelLoadChannel;
        [SerializeField] private LevelFinishChannelSO _levelFinishChannel;

        [Space(10)]
        public StartPage _startPage;
        public FinishPage _finishPage;

        private void Start()
        {
            if (SelfInit)
                Init();
        }
        public void Init()
        {
            _finishPage?.Hide();
            _startPage?.Hide();

            _levelStartChannel.OnLevelStarted.AddListener(OnLevelStarted);
            _levelLoadChannel.OnLevelLoaded += OnLevelLoaded;
            _levelFinishChannel.OnLevelFinished += OnLevelFinished;
        }

        private void OnLevelLoaded(int index)
        {
            _startPage?.Show(_gameState.CurrentLevelIndex);
        }

        public void OnLevelStarted()
        {
         //   _startPage.Hide();
        }

        public void OnLevelFinished()
        {
            _finishPage.Show(_gameState.CurrentLevelIndex);
        }

    }
}