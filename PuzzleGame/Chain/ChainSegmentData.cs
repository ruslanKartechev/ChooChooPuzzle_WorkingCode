
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    public enum ChainSegmentType { Chain, Spline}
    public struct ChainSegmentData
    {
        public ChainSegmentType type;
        public ChainLinksPositioner _positioner;
        public List<ChainLink> _links;
        public Transform end_1;
        public Transform end_2;
        public ChainSegmentData(List<ChainLink> links, Transform end_1, Transform end_2, ChainLinksPositioner positioner, ChainSegmentType type = ChainSegmentType.Chain)
        {
            _links = links;
            this.end_1 = end_1;
            this.end_2 = end_2;
            _positioner = positioner;
            this.type = type;
        }
    }
}
