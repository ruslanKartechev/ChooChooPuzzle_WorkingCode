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
    public class GameManager : SingletonMB<GameManager>
    {
        [Header("Debugging")]
        public bool DoStartGame = true;
        public bool UseUI = true;
        public bool UseSound = true;
        public LevelManager levelManager;
        [Header("General")]
        public SoundEffectManager _sounds;
        public EventsManager _events;
        public DataManager _data;
        public ServerDataLoader _dataLoader;
        public GameControllsMaster _controlls;
        public GameUIController _ui;

        private void Start()
        {
            if(_ui == null) { _ui = FindObjectOfType<GameUIController>(); }
            if (UseUI)
            {
                if (_ui == null) Debug.Log("UI controller was not found");
                _ui.Init();
            }
            if (UseSound)
            {
                if (_sounds == null)
                    _sounds = FindObjectOfType<SoundEffectManager>();
                _sounds.Init();
            }
            if (levelManager == null) levelManager = FindObjectOfType<LevelManager>();


            _controlls.Init();

  
            GameManager.Instance._events.DataLoaded.AddListener(OnDataLoaded);
            GameManager.Instance._events.LevelLoaded.AddListener(OnLevelLoaded);
            GameManager.Instance._events.NextLevelCalled.AddListener(OnNextLevelCalled);
            _dataLoader.StartLoading();
        }
        public void OnDataLoaded()
        {
            _events.LevelLoaded.Invoke();
            if (DoStartGame)
            {
                //levelManager.InitLevel(levelManager.CurrentLevelIndex);
            }
        }

        private void OnLevelLoaded()
        {
            if(UseUI == false)
            GameManager.Instance._events.LevelStarted.Invoke();
        }
        private void OnNextLevelCalled()
        {
            levelManager.NextLevel();
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
