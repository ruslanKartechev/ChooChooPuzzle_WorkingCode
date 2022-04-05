using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonGame.UI;
namespace CommonGame.UI
{
    public class GameUIController : MonoBehaviour
    {
        public StartPanel startPanel;
        public LevelCompletePanel levelEndPanel;
        public ProgressPanel progressPanel;
        public void Init()
        {

            if (startPanel == null)
                startPanel = GetComponent<StartPanel>();
            if (levelEndPanel == null)
                levelEndPanel = GetComponent<LevelCompletePanel>();

            startPanel?.Init();
            levelEndPanel?.Init();
            progressPanel?.Init();
            progressPanel?.HidePanel(true);
            startPanel?.HidePanel(true);
            levelEndPanel?.HidePanel(true);

            InitButtons();

            GameManager.Instance._events.LevelLoaded.AddListener(ShowStart);
            GameManager.Instance._events.LevelEndreached.AddListener(OnLevelFinishReached);
            GameManager.Instance._events.PlayerLose.AddListener(OnPlayerLoose);
            GameManager.Instance._events.LevelLoaded.AddListener(OnNewLevel);
        }
        private void InitButtons()
        {
            startPanel.MainButtonPressed = StartGame;
            levelEndPanel.MainButtonPressed = NextLevel;
        }

        public void ShowStart()
        {
            levelEndPanel.HidePanel(true);
            startPanel.ShowPanel();
        }
        public void OnPlayerLoose()
        {
            levelEndPanel.MainButtonPressed = RestartLevel;
            levelEndPanel.ShowRetryPanel(GameManager.Instance.levelManager.CurrentLevelIndex);
        }
        public void OnPlayerWin()
        {
            levelEndPanel.MainButtonPressed = NextLevel;
        }

        private void OnNewLevel()
        {
            progressPanel.HidePanel(true);
            startPanel.OnNewLevel(GameManager.Instance.levelManager.CurrentLevelIndex + 1 );
            levelEndPanel.OnNewLevel(GameManager.Instance.levelManager.CurrentLevelIndex + 1);
        }
        public void OnLevelFinishReached()
        {
            progressPanel.HidePanel(true);
            levelEndPanel.ShowPanel();
        }


        private void StartGame()
        {
            GameManager.Instance._events.LevelStarted.Invoke();
            progressPanel.ShowPanel(false);
        }
        private void RestartLevel()
        {
            GameManager.Instance.levelManager.RestartLevel();
            progressPanel.ShowPanel(true);
        }
        private void NextLevel()
        {
            GameManager.Instance.levelManager.NextLevel();
            progressPanel.HidePanel(true);
        }

    }
}