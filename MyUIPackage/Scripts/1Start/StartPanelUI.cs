using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace CommonGame.UI
{
    public class StartPanelUI : UIPanel
    {
        private StartPanel panel;
        public void Init(StartPanel _panel)
        {
            Init();
            panelManager = _panel;
            panel = _panel;
            mainButton.interactable = true;
            mainButton.onClick.AddListener(OnMainButtonClick);
        }
        public void SetLevel(int level)
        {
            string text = "Level " + level.ToString();
            SetHeaderText(text);
        }
        public override void OnPanelShown()
        {
            base.OnPanelShown();
            mainButton.interactable = true;
            mainButton.enabled = true;
        //    OnMainButtonClick();
        }
        public void OnMainButtonClick()
        {

            panel.OnMainButtonClick();
        }

    }
}