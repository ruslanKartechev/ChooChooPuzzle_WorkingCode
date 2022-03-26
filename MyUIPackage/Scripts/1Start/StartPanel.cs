using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace CommonGame.UI
{
    public class StartPanel : UIPanelManager
    {
        [SerializeField] private StartPanelUI panelUI;
        public Action MainButtonPressed;

        public void Init()
        {
            if (panelUI == null)
                panelUI = FindObjectOfType<StartPanelUI>();
            mPanel = panelUI;
            panelUI.Init(this);
        }

        public void OnNewLevel(int level)
        {
            panelUI.SetLevel(level);
        }

        public override void OnMainButtonClick()
        {
            HidePanel(true);
            MainButtonPressed?.Invoke();
        }

        public override void OnPanelHidden()
        {
            base.OnPanelHidden();
        }

    }
}