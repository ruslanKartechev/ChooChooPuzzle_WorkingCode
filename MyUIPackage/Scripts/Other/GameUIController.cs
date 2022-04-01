﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonGame.UI;
namespace CommonGame.UI
{
    public class GameUIController : MonoBehaviour
    {
        public StartPanel startPanel;
        public LevelCompletePanel levelEndPanel;
        public StickControllsPanel controllsPanel;
        public void Init()
        {

            if (startPanel == null)
                startPanel = GetComponent<StartPanel>();
            if (levelEndPanel == null)
                levelEndPanel = GetComponent<LevelCompletePanel>();

            startPanel?.Init();
            levelEndPanel?.Init();
            controllsPanel?.Init();
            controllsPanel?.HidePanel(true);
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
            startPanel.OnNewLevel(GameManager.Instance.levelManager.CurrentLevelIndex + 1 );
            levelEndPanel.OnNewLevel(GameManager.Instance.levelManager.CurrentLevelIndex + 1);
        }
        public void OnLevelFinishReached()
        {
            levelEndPanel.ShowPanel();
        }


        private void StartGame()
        {
            GameManager.Instance._events.LevelStarted.Invoke();
        }
        private void RestartLevel()
        {
            GameManager.Instance.levelManager.RestartLevel();
        }
        private void NextLevel()
        {
            GameManager.Instance.levelManager.NextLevel();
        }

    }
}