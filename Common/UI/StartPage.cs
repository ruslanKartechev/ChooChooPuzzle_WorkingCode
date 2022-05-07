using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CommonGame.Events;
namespace CommonGame.UI
{
    public class StartPage : MonoBehaviour
    {
        [SerializeField] private PageView _view;
        [SerializeField] private LevelStartChannelSO _startChannel;
        [SerializeField] private Button _button;
        [SerializeField] private string _headerText = "DRAG TO START";
        public void Show(int currentLevelIndex)
        {
            //_view.SetHeader($"Level {currentLevelIndex+1}");
            _view.SetHeader(_headerText);
            _button.onClick.AddListener(StartLevel);
            _button.interactable = true;
            _view.ShowPage();

        }
        public void Hide()
        {
            _view.HidePage();
            _button.interactable = false;
        }
        public void StartLevel()
        {
            _startChannel.RaiseEvent();
            Hide();
        }



    }
}