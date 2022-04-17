using System.Collections;
using UnityEngine;

namespace PuzzleGame
{
    public abstract class ChainSegmentManager : MonoBehaviour
    {
        public abstract void InitSegment(Transform pivot1, Transform pivot2);
        public abstract void StartChainMovement();
        public abstract void StopChainMovement();
        public abstract ChainSegmentData GetChainInfo();
        public abstract void UpdateSegment();
    }
}