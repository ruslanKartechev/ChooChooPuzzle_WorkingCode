
using UnityEngine;
using CommonGame.Events;
namespace CommonGame.UI
{
    public class FinishPage : MonoBehaviour
    {

        [SerializeField] private PageViewBase _view;
        [SerializeField] private LevelLoadChannelSO _levelLoadChannel;

        public void Show(int currentLevelIndex)
        {
            _view.SetHeader($"Level {currentLevelIndex} Completed");
            _view.OnButtonClick = NextLevel;
            _view.ShowPage();
        }
        public void Hide()
        {
            _view.HidePage();
        }
        public void NextLevel()
        {
            _levelLoadChannel.RaiseEventLoadNext();
            _view.HidePage();
        }
    }
}