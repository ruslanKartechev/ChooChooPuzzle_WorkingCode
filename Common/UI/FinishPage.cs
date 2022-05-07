
using UnityEngine;
using CommonGame.Events;
using UnityEngine.UI;
namespace CommonGame.UI
{
    public class FinishPage : MonoBehaviour
    {

        [SerializeField] private PageView _view;
        [SerializeField] private LevelLoadChannelSO _levelLoadChannel;
        [SerializeField] private Button _button;
        public void Show(int currentLevelIndex)
        {
            _view.SetHeader($"Level {currentLevelIndex+1} Completed");
            _button.onClick.AddListener(NextLevel);
            _button.interactable = true;
            _view.ShowPage();
            Debug.Log("shown");
        }
        public void Hide()
        {
            _view.HidePage();
            _button.interactable = false;

        }
        public void NextLevel()
        {
            Debug.Log("next level called");

            _levelLoadChannel.RaiseLoadNext();
            _view.HidePage();
        }
    }
}