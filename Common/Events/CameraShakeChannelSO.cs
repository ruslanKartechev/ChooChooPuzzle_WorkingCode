using System;
using UnityEngine;
namespace CommonGame.Events
{
    [CreateAssetMenu(fileName = "CameraShakeChannelSO", menuName = "EventChannels/CameraShakeChannelSO", order = 1)]
    public class CameraShakeChannelSO : ScriptableObject
    {
        public Action ShakeCamera;
        public void RaiseEventCameraShake()
        {
            if (ShakeCamera != null)
                ShakeCamera.Invoke();
            else
                Debug.Log("CameraShake action is null");
        }
    }
}