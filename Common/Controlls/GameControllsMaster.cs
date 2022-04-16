using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonGame;
namespace CommonGame.Controlls
{
    public class GameControllsMaster : MonoBehaviour
    {
        [SerializeField] protected IControlls _manager;
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