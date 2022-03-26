using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace CommonGame.UI
{
    public class StickControllsPanelUI : UIPanel
    {
        [SerializeField] private Image backCircle;
        [SerializeField] private Image stickImage;

        public Image StickImage { get { return stickImage; } }
        public Transform stickTransform { get { return stickImage.transform; } }
        public Action OnControllStickClick;
        private StickControllsPanel _panel;
        public void Init(StickControllsPanel panel)
        {
            base.Init();
            base.Init(panel);
            _panel = panel;
        }

        public override void ShowPanel()
        {

            base.ShowPanelImmidiate();
            backCircle.enabled = true;
            stickImage.enabled = true;
        }
        public override void HideHeader()
        {
            base.HidePanelImmidiate();
            backCircle.enabled = false;
            stickImage.enabled = false;
        }

        public void SetStickLocalPos(Vector2 localPos)
        {
            stickImage.transform.localPosition = localPos;
        }
        public bool CheckPointerOver()
        {

            return false;
        }

    }
}
