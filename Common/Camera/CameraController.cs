
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CommonGame
{
    
    public class CameraController : SingletonMB<CameraController>
    {
        [Header("Settings")]
        public CamShakeSettings _ShakeSettings;
        [Header("Components")]
        public CameraShake _shakeManager;

        private void Start()
        {
            _shakeManager.Init(this, _ShakeSettings);   
        }

        public void Shake()
        {
            _shakeManager.Shake();
        }
    }
}
