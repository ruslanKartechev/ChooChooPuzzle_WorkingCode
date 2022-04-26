
using UnityEngine;
using UnityEngine.Events;
namespace CommonGame.Events
{
    [CreateAssetMenu(fileName = "LevelFinishChannelSO", menuName = "EventChannels/LevelFinishChannelSO", order = 1)]
    public class LevelFinishChannelSO : ScriptableObject
    {
        public UnityAction OnLevelFinished;
        public void RaiseEvent()
        {
            if (OnLevelFinished != null)
                OnLevelFinished.Invoke();
            else
                Debug.Log("OnLevelFinished action is null");
        }
    }
}