using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame
{
    public class SegmentIndeces
    {
        public int SegmentIndex;
        public int LinksCount;
    }

    public class SegmentIndecesCalculator
    {
        public List<SegmentIndeces> ConvertAll(List<LinksSegmentManager> segments)
        {
            List<SegmentIndeces> output = new List<SegmentIndeces>();
            foreach (LinksSegmentManager s in segments)
            {
                if(s != null)
                    output.Add(Convert(segments, s));
            }
            return output;
        }
        public SegmentIndeces Convert(List<LinksSegmentManager> segments, LinksSegmentManager target)
        {
            SegmentIndeces result = new SegmentIndeces();
            result.LinksCount = target._Links.Count();
            result.SegmentIndex = segments.IndexOf(target);

            return result;
        }

    }
}
