using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CommonGame.UI
{

    public class PanelManager : MonoBehaviour
    {

        protected PanelUI m_ui;
        protected List<string> mHeaders = new List<string>();

        protected int currentHeader = 0;
        public virtual void OnMainButtonClick()
        {

        }
        public virtual void ShowPanel(bool showButton = true)
        {
            if (!showButton)
            {
                m_ui.HideMainButton(false);
            }
            m_ui.ShowPanel();
            if (showButton)
            {
                m_ui.ShowMainButton(true);
            }
        }
        public virtual void HidePanel(bool immidiate)
        {
            if (m_ui.gameObject.activeInHierarchy == false)
                return;
            if (!immidiate)
                m_ui.HidePanel();
            else
                m_ui.HidePanelImmidiate();
        }
        public virtual void SwitchHeader(int dir)
        { 
            currentHeader += dir;
            if (currentHeader >= mHeaders.Count || currentHeader < 0)
                return;
            string newHeader = mHeaders[currentHeader];
            m_ui.SetHeaderText(newHeader);
        }
        public virtual void ShowMainButton(bool animate)
        {
            m_ui.ShowMainButton(animate);
        }
        public virtual void HideMainButton(bool animate) {
            m_ui.HideMainButton(animate);
        }



        public virtual void OnPanelHidden()
        {
        }
        public virtual void OnPanelShown()
        {
        }

    }
}