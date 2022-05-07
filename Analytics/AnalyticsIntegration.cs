
using UnityEngine;
using CommonGame.Events;
using Ketchapp.MayoSDK;
namespace PuzzleGame
{
    public class AnalyticsIntegration : MonoBehaviour
    {
        [SerializeField] private LevelStartChannelSO _startChannel;
        [SerializeField] private LevelFinishChannelSO _finishChannel;
        [SerializeField] private LevelLoadChannelSO _levelLoadChannel;


        private int _currentLevel;
        private void Start()
        {
            _levelLoadChannel.OnLevelLoaded += OnLevel;
            _startChannel.OnLevelStarted.AddListener(OnLevelStarted);
            _finishChannel.OnLevelFinished.AddListener(OnLevelFinished);

        }

        private void OnLevel(int level)
        {
            Debug.Log($"level loaded {level}");
            KetchappSDK.Analytics.GetLevel(level);
            _currentLevel = level;
        }

        private void OnLevelStarted()
        {
            KetchappSDK.Analytics.GetLevel(_currentLevel.ToString()).ProgressionStart();
            Debug.Log("called progression start");

        }

        private void OnLevelFinished()
        {
            KetchappSDK.Analytics.GetLevel(_currentLevel.ToString()).ProgressionComplete();
            Debug.Log("called progression finish");
        }

    }
}