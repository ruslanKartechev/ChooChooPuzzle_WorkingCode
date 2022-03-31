
using System.Collections.Generic;


namespace PuzzleGame
{
    public class ChainSegmentInfo
    {
        public List<ChainLink> _links;
        public ChainSegmentInfo() { }
        public ChainSegmentInfo(List<ChainLink> links)
        {
            _links = links;
        }
    }
}
