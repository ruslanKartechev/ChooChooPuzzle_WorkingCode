using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonGame.Events;
using CommonGame.Data;
using CommonGame.Controlls;
using CommonGame.Sound;
using CommonGame.UI;
using CommonGame.Server;
namespace CommonGame
{
    [DefaultExecutionOrder(-10)]
    public class GameManager : MonoBehaviour
    {
        [Header("Debugging")]
        public bool DoStartGame = true;
        public bool UseUI = true;
        public bool UseSound = true;
        public LevelManager levelManager;
        [Header("General")]
        public SoundEffectManager _sounds;
        public InputController _controlls;
        public UIManager _ui;

        private void Start()
        {
            if (UseSound)
            {
                if (_sounds == null)
                    _sounds = FindObjectOfType<SoundEffectManager>();
                _sounds.Init();
            }
            if (UseUI)
                _ui?.Init();
            if (levelManager == null) levelManager = FindObjectOfType<LevelManager>();
            levelManager.LoadLast();
            _controlls.Init();
            
        }
    }


}




public class SingletonMB<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<T>();
            return instance;
        }
        set
        {
            instance = value;
        }
    }



}
