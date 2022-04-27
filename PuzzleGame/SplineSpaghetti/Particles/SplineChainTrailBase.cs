using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    public abstract class SplineChainTrailBase : MonoBehaviour
    {
        public abstract void Init();
        public abstract void Enable();
        public abstract void Disable();
        public abstract void OnFinish(Vector3 finishPoint);
    }
}