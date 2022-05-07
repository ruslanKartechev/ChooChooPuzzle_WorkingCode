using UnityEngine;
using UnityEngine.Events;
using System;
namespace CommonGame.Events
{
    [CreateAssetMenu(fileName = "LevelStartChannelSO", menuName = "EventChannels/LevelStartChannelSO", order = 1)]
    public class LevelStartChannelSO : ScriptableObject
    {
        public UnityEvent OnLevelStarted;
        
        public void RaiseEvent()
        {
            if (OnLevelStarted != null)
                OnLevelStarted.Invoke();
            else
                Debug.Log("StartLevel action is null");
        }
    }
}
