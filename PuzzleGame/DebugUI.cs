using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace PuzzleGame
{
    public class DebugUI : SingletonMB<DebugUI>
    {
        [SerializeField] private Text _text;

        public void Debug(string message)
        {
            _text.text += "\n" + message;
            
        } 
    }
}