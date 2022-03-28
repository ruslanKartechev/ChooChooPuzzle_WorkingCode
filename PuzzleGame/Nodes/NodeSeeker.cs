using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    public static class NodeSeeker
    {
        public static SplineNode FindNextNode(Vector2 input, SplineNode from, List<SplineNode> nodes = null)
        {
            if (nodes == null)
                nodes = from.linkedNodes;

            if (nodes.Count == 1)
                return nodes[0];

            if (Mathf.Abs(input.x) >= Mathf.Abs(input.y))
            {
                var res = new List<SplineNode>();
                if (input.x >= 0)
                    res = GetHorNodes(true, from.transform, nodes);
                else
                    res = GetHorNodes(false, from.transform, nodes);
                if (res == null || res.Count == 0) { return null; }

                SplineNode n = res[0];
                if (res.Count > 1)
                {
                    if (input.y >= 0)
                        n = res.Find(x => x.transform.position.y >= from._position.y);
                    else
                        n = res.Find(x => x.transform.position.y < from._position.y);
                }
                return n;
            }
            else
            {
                var res = new List<SplineNode>();
                if (input.x >= 0)
                    res = GetVertNodes(true, from.transform, nodes);
                else
                    res = GetVertNodes(false, from.transform, nodes);
                if (res == null || res.Count == 0) { return null; }

                SplineNode n = res[0];
                if (res.Count > 1)
                {
                    if (input.x >= 0)
                        n = res.Find(x => x.transform.position.x >= from._position.x);
                    else
                        n = res.Find(x => x.transform.position.x < from._position.x);
                }
                return n;
            }
            return null;

        }

        public static List<SplineNode> GetHorNodes(bool right, Transform refPos, List<SplineNode> nodes)
        {
            var res = new List<SplineNode>();
            if (right)
                res = nodes.FindAll(x => x.transform.position.x >= refPos.position.x);
            else
                res = nodes.FindAll(x => x.transform.position.x < refPos.position.x);
            return res;
        }
        public static List<SplineNode> GetVertNodes(bool up, Transform refPos, List<SplineNode> nodes)
        {
            var res = new List<SplineNode>();
            if (up)
                res = nodes.FindAll(x => x.transform.position.y >= refPos.position.y);
            else
                res = nodes.FindAll(x => x.transform.position.y < refPos.position.y);
            return res;
        }
    }
}