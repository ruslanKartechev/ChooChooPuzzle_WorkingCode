using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BomberGame.Data;

namespace CommonGame.Sound {
    public class EventEffectManager : MonoBehaviour
    {

        public SoundNames levelStart;
        public SoundNames levelEnd;
        public SoundNames win;
        public SoundNames loose;
        public SoundNames lazerApproached;
        public SoundNames portalApproached;

        private void Start()
        {
            GameManager.Instance._events.LevelStarted.AddListener(OnLevelStart);
            GameManager.Instance._events.PlayerWin.AddListener(OnPlayerWin);
            GameManager.Instance._events.PlayerLose.AddListener(OnPlayerLoose);
            GameManager.Instance._events.LevelEndreached.AddListener(OnLevelEnd);
        }


        private void OnLevelStart()  
        {
            GameManager.Instance._sounds.StopLoopedEffect(levelEnd.ToString());
            GameManager.Instance._sounds.PlayMusic();
        }
        private void OnLevelEnd()
        {
            GameManager.Instance._sounds.StopMusic();
        }
        private void OnPlayerWin()
        {
            GameManager.Instance._sounds.PlaySingleTime(win.ToString());
            GameManager.Instance._sounds.StartSoundEffect(levelEnd.ToString());
        }
        private void OnPlayerLoose()
        {
            GameManager.Instance._sounds.PlaySingleTime(loose.ToString());
        }



    }
}