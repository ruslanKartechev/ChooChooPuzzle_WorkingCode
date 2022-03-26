using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonGame;
namespace CommonGame.Controlls
{
    public class GameControllsMaster : MonoBehaviour
    {
        [SerializeField] protected ControllsManager _manager;
        [SerializeField] private bool IsDebug = true;
        public void Init()
        {
            if (_manager == null) return;
            _manager.Init(null);
     
            Input.simulateMouseWithTouches = true;
            if (IsDebug)
            {
                _manager.EnableControlls();

            }
            //GameManager.Instance._events.LevelStarted.AddListener(OnLevelStarted);
            //GameManager.Instance._events.LevelEndreached.AddListener(OnLevelEnd);
            //GameManager.Instance._events.LevelLoaded.AddListener(OnLevelLoaded);

        }
        protected void OnLevelLoaded()
        {
          //  _manager.SetMover(GameManager.Instance._data._currentLevel._Player.Components._InputHandler);
        }
        protected void OnLevelStarted()
        {
            _manager.ShowControlls();
            _manager.EnableControlls();

        }
        protected void OnLevelEnd()
        {
            _manager.HideControlls();

        }
    }
}