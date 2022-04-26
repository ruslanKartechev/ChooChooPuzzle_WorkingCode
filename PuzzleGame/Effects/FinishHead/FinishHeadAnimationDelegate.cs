using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    public class FinishHeadAnimationDelegate : MonoBehaviour
    {
        public FinishHeadAnimator _EventReciever;

        public void OnBite()
        {
            _EventReciever.OneBiteEvent();
        }
    }
}