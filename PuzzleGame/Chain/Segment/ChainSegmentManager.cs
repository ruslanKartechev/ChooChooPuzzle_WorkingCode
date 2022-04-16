using System.Collections;
using UnityEngine;

namespace PuzzleGame
{
    public abstract class ChainSegmentManager : MonoBehaviour
    {
        public abstract void InitSegment();
        public abstract void StartChainMovement();
        public abstract void StopChainMovement();
        public abstract ChainSegmentData GetChainInfo();
    }
}