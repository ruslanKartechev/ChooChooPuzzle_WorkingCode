using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace CommonGame.Events
{
    public class ArgEvent<T> : UnityEvent<T>
    {

    }
    public class EventsManager : MonoBehaviour
    {

        public ArgEvent<int> Input = new ArgEvent<int>();
        public UnityEvent MouseDown = new UnityEvent();
        public UnityEvent MouseUp = new UnityEvent();

        public UnityEvent DataLoaded = new UnityEvent();
        public UnityEvent LevelLoaded = new UnityEvent();
        public UnityEvent LevelStarted = new UnityEvent();

        public UnityEvent DisplayFinished = new UnityEvent();

        public UnityEvent LevelEndreached = new UnityEvent();

        public UnityEvent PlayerWin = new UnityEvent();
        public UnityEvent PlayerLose = new UnityEvent();

        public UnityEvent LevelFinished = new UnityEvent();

        public UnityEvent NextLevelCalled = new UnityEvent();

        public UnityEvent Impact = new UnityEvent();


        public UnityEvent MoveMade = new UnityEvent();
    }
}