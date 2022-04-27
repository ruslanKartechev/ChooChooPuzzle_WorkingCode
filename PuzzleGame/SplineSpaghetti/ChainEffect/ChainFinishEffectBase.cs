using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace PuzzleGame
{

    public abstract class ChainFinishEffectBase : MonoBehaviour
    {
        public Action OnEffectEnd;
        public abstract void ExecuteEffect(Vector3 finishPoint);   
    }
}