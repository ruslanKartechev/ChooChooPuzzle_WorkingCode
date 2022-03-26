using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace CommonGame.UI
{
    public class LevelCompletePanel : UIPanelManager
    {
        [SerializeField] private LevelCompletePanelUI panelUI;
        public Action MainButtonPressed;
        public void Init()
        {
            panelUI.Init(this);
            mPanel = panelUI;
        }
        public void OnNewLevel(int currentLevel)
        {
            panelUI.SetLevel(currentLevel);
        }

        public override void ShowPanel(bool showButton = true)
        {
            base.ShowPanel(false);

        }



        public override void OnMainButtonClick()
        {
            HidePanel(false);
            MainButtonPressed?.Invoke();
        }

        public void ShowRetryPanel(int CurrentLevel)
        {
            panelUI.gameObject.SetActive(true);
            panelUI.SetHeaderText("Level " + CurrentLevel + " Failed");
            panelUI.StartHeaderAnimator();
            panelUI.HideMainButton(false);
            panelUI.ShowRetryButton();

        }

        public void OnRetryButtonClick()
        {
            OnMainButtonClick();
        }
    }
}