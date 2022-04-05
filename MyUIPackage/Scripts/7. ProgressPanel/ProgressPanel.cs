using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonGame.UI
{
    public class ProgressPanel : PanelManager
    {
        [SerializeField] private ProgressPanelUI _ui;
        public void Init()
        {
            m_ui = _ui;
            _ui.Init(this);
        }

        public void OutputNumber(int number)
        {
            _ui?.SetHeaderText("Moves: " + number.ToString());
        }
        

    }
}