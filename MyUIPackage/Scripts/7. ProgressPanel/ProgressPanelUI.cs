using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonGame.UI
{
    public class ProgressPanelUI : PanelUI
    {
        private ProgressPanel _panel;
        public void Init(ProgressPanel panel)
        {
            _panel = panel;
            panelManager = panel;
        }
    }
}