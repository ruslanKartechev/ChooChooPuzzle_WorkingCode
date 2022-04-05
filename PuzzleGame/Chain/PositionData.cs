using System;
using System.Collections.Generic;

namespace PuzzleGame
{
    public class PositionData
    {
        public int num;
        public Dictionary<int, SplineNode> priorPosition = new Dictionary<int, SplineNode>();
        public PositionData() { }
        public PositionData(List<ChainFollower> followers, int num)
        {
            this.num = num;
            for (int i = 0; i < followers.Count; i++)
            {
                priorPosition.Add(i, followers[i].currentNode);
            }
        }
    }
}
