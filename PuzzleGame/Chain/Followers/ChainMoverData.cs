using System.Collections;
using UnityEngine;
using System;
namespace PuzzleGame
{
    public class ChainMoverData
    {
        public SplineNode node;
        public Action<SplineNode> onEnd = null;
        public ChainMoverData() { }
        public ChainMoverData(SplineNode node, Action<SplineNode> onEnd)
        {
            this.node = node;
            this.onEnd = onEnd;
        }
    }
}