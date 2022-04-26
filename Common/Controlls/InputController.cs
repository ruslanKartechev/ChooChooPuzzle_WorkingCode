using CommonGame.Events;
using UnityEngine;
namespace CommonGame.Controlls
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private LevelStartChannelSO _levelStartChannel;
        [SerializeField] private LevelFinishChannelSO _levelFinishChannel;

        [SerializeField] protected IControllsManager _manager;
        [SerializeField] private bool IsDebug = true;
        public void Init()
        {
            if (_manager == null) return;
            _manager.Init(null);
            _levelFinishChannel.OnLevelFinished += OnLevelEnd;
            _levelStartChannel.OnLevelStarted.AddListener(OnLevelStarted);
            Input.simulateMouseWithTouches = true;
            if (IsDebug)
            {
                _manager.EnableControlls();
            }
        }
        protected void OnLevelStarted()
        {
            _manager.ShowControlls();
            _manager.EnableControlls();

        }
        protected void OnLevelEnd()
        {
            _manager.HideControlls();
            _manager.DisableControlls();
        }
    }
}