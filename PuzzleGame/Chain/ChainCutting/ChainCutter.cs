using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace PuzzleGame
{

    public class ChainCutResult
    {
        public List<int> SegmentsAway;

        public int SegmentCut;
        public List<int> LinksToCut;
    }




    public class ChainCutter
    {
        private SegmentsCutter segmentsCutter;
        private IndecesCalculator linksCutter;
        public ChainCutter()
        {
            segmentsCutter = new SegmentsCutter();
            linksCutter = new IndecesCalculator();
        }
        
        public ChainCutResult Cut(List<SegmentIndeces> segments, SegmentIndeces callerSegment, int callerLinkIndex )
        {
            ChainCutResult result = new ChainCutResult();
            if(segments.Count%2 != 0 && callerSegment.SegmentIndex == segments.Count / 2)
            {
                result = SegmentsCutter.CutSegmentMiddle(segments.Count,callerSegment.LinksCount,callerLinkIndex);
                return result;
            }
            result = SegmentsCutter.GetSegmentsCut(segments.Count, callerSegment.SegmentIndex, callerSegment.LinksCount,callerLinkIndex);
            return result;

        }
    }

    public class SegmentsCutter
    {
        public static ChainCutResult GetSegmentsCut(int totalSegments, int segmentTarget, int callerLinksTotal, int callerLinkTarget)
        {
            Debug.Log("NOT MIDDLE");
            List<int> cutSegments = null;
            if (segmentTarget == 0 || segmentTarget == totalSegments - 1)
                cutSegments = null;
            else
                cutSegments = new List<int>();
            
            List<int> linksToCut = new List<int>();
            ChainCutResult result = new ChainCutResult() ;
            if (totalSegments%2 == 0)
            {
                if(segmentTarget < totalSegments / 2 - 1)
                {
                    for(int i = 0; i< segmentTarget; i++)
                        cutSegments.Add(i);
                    linksToCut = IndecesCalculator.CloserToStart(callerLinksTotal, callerLinkTarget);
                }
                else
                {
                    for (int i = totalSegments-1; i < segmentTarget; i--)
                        cutSegments.Add(i);
                    linksToCut = IndecesCalculator.CloserToEnd(callerLinksTotal, callerLinkTarget);
                }
            }
            else
            {
                if (segmentTarget == totalSegments / 2)
                {
                    Debug.Log("Middle case, exception thrown");
                    throw new Exception("middle case");
                }
                else if(segmentTarget < totalSegments / 2 )
                {
                    for (int i = 0; i < segmentTarget; i++)
                        cutSegments.Add(i);
                    linksToCut = IndecesCalculator.CloserToStart(callerLinksTotal, callerLinkTarget);
                }
                else
                {
                    for (int i = totalSegments - 1; i > segmentTarget; i--)
                        cutSegments.Add(i);
                    linksToCut = IndecesCalculator.CloserToEnd(callerLinksTotal, callerLinkTarget);
                }
            }

            result.LinksToCut = linksToCut;
            result.SegmentsAway = cutSegments;
            result.SegmentCut = segmentTarget;

            return result;
        }


        public static ChainCutResult CutSegmentMiddle(int segmentsTotal, int linksTotal, int linkInd)
        {
            Debug.Log("middle");
            ChainCutResult result = new ChainCutResult();
            List<int> segments = new List<int>();
            List<int> cutLinks = new List<int>();
            int middleSegment = segmentsTotal / 2;
            if (linkInd <= (linksTotal - linkInd))
            {
                for (int i = 0; i < middleSegment; i++)
                    segments.Add(i);
                for (int i = 0; i <= linkInd; i++)
                    cutLinks.Add(i);
            }
            else
            {
                for (int i = segmentsTotal-1; i > middleSegment; i--)
                    segments.Add(i);
                for (int i = linkInd; i < linksTotal; i++)
                    cutLinks.Add(i);
            }
            result.LinksToCut = cutLinks;
            result.SegmentCut = middleSegment;
            result.SegmentsAway = segments;

            return result;
        }

    }
  
    
    public class IndecesCalculator
    {
        public static List<int> CloserToStart(int totalCount, int target, bool include = true)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < target; i++)
                result.Add(i);
            if (include)
                result.Add(target);

            return result;
        }
        public static List<int> CloserToEnd(int totalCount, int target, bool include = true)
        {
            List<int> result = new List<int>();
            if (include)
                result.Add(target);
            for (int i = target + 1; i < totalCount; i++)
                result.Add(i);

            return result;
        }
    }




}
