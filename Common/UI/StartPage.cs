using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonGame.Events;
namespace CommonGame.UI
{
    public class StartPage : MonoBehaviour
    {
        [SerializeField] private PageViewBase _view;
        [SerializeField] private LevelStartChannelSO _startChannel;
        
        public void Show(int currentLevelIndex)
        {
            _view.SetHeader($"Level {currentLevelIndex}");
            _view.OnButtonClick = StartLevel;
            _view.ShowPage();

        }
        public void Hide()
        {
            _view.HidePage();
        }
        public void StartLevel()
        {
            _startChannel.RaiseEvent();
            _view.HidePage();
        }



    }
}