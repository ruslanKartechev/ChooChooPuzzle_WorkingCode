using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    public class ChainPositionInfo
    {
        public List<SplineNode> chainNodes;
        public SplineNode leadingNode;
    }

    public class ConstraintData
    {
        public ChainPositionInfo chainPositions;
        public Vector2 ScreenDirection;
    }

    public enum NodeConstaintType { Default, Angle }








}