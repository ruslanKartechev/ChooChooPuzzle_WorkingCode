
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    public struct ChainSegmentData
    {

        public ChainLinksPositioner _positioner;
        public List<ChainLink> _links;
        public Transform end_1;
        public Transform end_2;
        public ChainSegmentData(List<ChainLink> links, Transform end_1, Transform end_2, ChainLinksPositioner positioner)
        {
            _links = links;
            this.end_1 = end_1;
            this.end_2 = end_2;
            _positioner = positioner;
        }
    }
}
