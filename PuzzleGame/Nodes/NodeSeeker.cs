using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    public static class NodeSeeker
    {
        public static SplineNode FindNextNode(Vector2 input, SplineNode from, List<SplineNode> nodes = null, bool chooseFirst=true)
        {
            if (nodes == null)
                nodes = from.linkedNodes;

            if (nodes.Count == 1 && chooseFirst == true)
            {
                if (nodes[0] != from)
                    return nodes[0];
                else
                    return null;
            }
            if(Mathf.Abs(input.x) >= Mathf.Abs(input.y))
                return FindFromHor(input, from.transform,nodes);
            else
                return FindFromVert(input, from.transform, nodes);
        }

        public static SplineNode FindFromVert(Vector2 input, Transform refPos, List<SplineNode> nodes)
        {
            List<SplineNode> _options = new List<SplineNode>();
            SplineNode next = null;
            if (nodes == null) Debug.Log("nodes are null");
            if (refPos == null) Debug.Log("ref pos is null");

            if (input.y >= 0)
                _options = nodes.FindAll(t => (t != null)
                && GetScreenPos(t.transform.position).y >= GetScreenPos(refPos.position).y);
            else
                _options = nodes.FindAll(t => (t != null) 
                && GetScreenPos(t.transform.position).y <= GetScreenPos(refPos.position).y);
          
            if (input.x >= 0)
                _options = _options.FindAll(t => (t != null) && GetScreenPos(t.transform.position).x >= GetScreenPos(refPos.position).x);
            else
                _options = _options.FindAll(t => (t != null) && GetScreenPos(t.transform.position).x <= GetScreenPos(refPos.position).x);
            
            if (_options == null) return null;
            else if (_options.Count == 1) return _options[0];
            else if (_options.Count == 0) return null;

            //if (input.y >= 0)
            //{
            //    next = FindLongestDistance(res, refPos.position);
            //}
            //else
            //{
            //  next = FindShortestDistance(_options, refPos.position);
            //}

            next = FindByProjection(_options, refPos.position, input);
            return next;
        }

        public static SplineNode FindFromHor(Vector2 input, Transform refPos, List<SplineNode> nodes)
        {
            List<SplineNode> _options = new List<SplineNode>();
            SplineNode next = null;
            if (nodes == null) Debug.Log("nodes are null");
            if (refPos == null) Debug.Log("ref pos is null");
            //foreach(SplineNode n in nodes)
            //{
            //    if (n == null) continue;
            //    Debug.Log("options: !!: " + n.transform.parent.parent.name
            //   + "  " + n.name);
            //}

            if (input.x >= 0)
                _options = nodes.FindAll(t => (t != null)
                && GetScreenPos(t.transform.position).x >= GetScreenPos(refPos.position).x);
            else
                _options = nodes.FindAll(t => (t != null) 
                && GetScreenPos(t.transform.position).x <= GetScreenPos(refPos.position).x);
            if (input.y >= 0)
            {
                _options = _options.FindAll(t => (t != null) && GetScreenPos(t.transform.position).y >= GetScreenPos(refPos.position).y);
            }
            else
            {
                _options = _options.FindAll(t => (t != null) && GetScreenPos(t.transform.position).y <= GetScreenPos(refPos.position).y);
            }

            if (_options == null) return null;
            else if (_options.Count == 1) return _options[0];
            else if (_options.Count == 0) return null;

            //if(input.x >= 0)
            //{
            //    next = FindLongestDistance(res,refPos.position);
            //}
            //else
            //{
            //    next = FindShortestDistance(res,refPos.position);
            //}
            next = FindByProjection(_options, refPos.position, input);
            return next;
        }

        private static SplineNode FindByProjection(List<SplineNode> options, Vector3 refPos, Vector2 input)
        {
            SplineNode res = options[0];
            if (options.Count == 0)
                return res;
            Vector2 screenRef = GetScreenPos(refPos);
            Vector2 dir = GetScreenPos(res._position) - screenRef;
            float largest = Vector2.Dot(input, dir);
            foreach (SplineNode n in options)
            {
                if (n == null) continue;
                dir = (GetScreenPos(n._position) - screenRef);
                float proj = Vector2.Dot(input, dir);
                if(proj >= largest)
                {
                    largest = proj;
                    res = n;
                }
            }
            return res;
        }





        private static SplineNode FindLongestDistance(List<SplineNode> options, Vector3 refPos)
        {
            SplineNode res = options[0];
            Vector2 screenRef = GetScreenPos(refPos);
            float longest = (GetScreenPos(options[0]._position) - screenRef).magnitude;
            foreach (SplineNode n in options)
            {
                if (n == null) continue;
                float dist = (GetScreenPos(n._position) - screenRef).magnitude;
                if (dist >= longest)
                {
                    res = n;
                    longest = dist;
                }
            }
            return res;
        }
        private static SplineNode FindShortestDistance(List<SplineNode> options, Vector3 refPos)
        {
            SplineNode res = options[0];
            Vector2 screenRef = GetScreenPos(refPos);
            float shortest = (GetScreenPos(options[0]._position) - screenRef).magnitude;
            foreach(SplineNode n in options)
            {
                if (n == null) continue;
                float dist = (GetScreenPos(n._position) - screenRef).magnitude;
                if ( dist<= shortest){
                    res = n;
                    shortest = dist;
                }
            }
            return res;
        }

        private static Vector2 GetScreenPos(Vector3 worldPos)
        {
            return (Vector2)Camera.main.WorldToScreenPoint(worldPos);
        }
    }
}