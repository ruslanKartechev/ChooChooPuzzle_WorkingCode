using System;
using UnityEngine;


namespace CommonGame.Events
{
    [CreateAssetMenu(fileName = "SoundFXChannelSO", menuName = "EventChannels/SoundFXChannelSO", order = 1)]

    public class SoundFXChannelSO : ScriptableObject
    {
        public Action<string> OnPlayFX;
        public Action<string> OnPlayFXLoop;
        public Action<string> OnStopFXLoop;

        public void RaiseEventPlay(string name)
        {
            if (OnPlayFX != null)
                OnPlayFX?.Invoke(name);
            else
                Debug.Log("OnPlayFX action is null");
        }
        public void RaiseEventStartLoop(string name)
        {
            if (OnPlayFXLoop != null)
                OnPlayFXLoop?.Invoke(name);
            else
                Debug.Log("OnPlayFXLoop action is null");
        }
        public void RaiseEventStopLoop(string name)
        {
            if (OnStopFXLoop != null)
                OnStopFXLoop?.Invoke(name);
            else
                Debug.Log("OnStopFXLoop action is null");
        }

    }
}