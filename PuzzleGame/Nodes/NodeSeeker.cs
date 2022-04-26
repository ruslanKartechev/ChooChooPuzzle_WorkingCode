using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    public static class NodeSeeker
    {
        public static SplineNode FindNextNode(Vector2 input, SplineNode from, List<SplineNode> nodes, List<SplineNode> exclude, bool chooseFirst = true)
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

            List<SplineNode> clearedOptions = ClearOptions(nodes, exclude);
            SplineNode result = null;
            if (Mathf.Abs(input.x) >= Mathf.Abs(input.y))
            {
                result = FindFromHor(input, from.transform, nodes);
                //result = FindFromHor(input, from.transform, clearedOptions);
                //if (result == null)
                //{
                //    DebugSeeker("DID NOT FIND FROM CLEARED LIST");
                //    result = FindFromHor(input, from.transform, nodes);
                //}
            }
            else
            {
                result = FindFromVert(input, from.transform, nodes);
                //result = FindFromVert(input, from.transform, clearedOptions);
                //if (result == null)
                //{
                //    DebugSeeker("DID NOT FIND FROM CLEARED LIST");
                //    result = FindFromVert(input, from.transform, nodes);
                //}
            }

            if(result == null)
            {
                result = FindByProjection(nodes, from.transform.position, input);
               // result = FindByProjection(clearedOptions, from.transform.position,input);
                if(result == null)
                {
                    result = FindByProjection(nodes, from.transform.position, input);
                }
            }

            return result;
        }

        public static SplineNode FindFromVert(Vector2 input, Transform refPos, List<SplineNode> nodes)
        {
            DebugSeeker("Finding from vert");
            List<SplineNode> _options = new List<SplineNode>();
            SplineNode next = null;
            if (nodes == null) DebugSeeker("nodes are null");
            if (refPos == null) DebugSeeker("ref pos is null");
            if (nodes.Count == 1) { DebugSeeker("only one res return"); return nodes[0]; }
            if (input.y >= 0)
                _options = nodes.FindAll(t => (t != null)
                && GetScreenPos(t.transform.position).y >= GetScreenPos(refPos.position).y);
            else
                _options = nodes.FindAll(t => (t != null)
                && GetScreenPos(t.transform.position).y <= GetScreenPos(refPos.position).y);

            FindFromOptions();
            void FindFromOptions()
            {
                if (_options == null) { DebugSeeker("<color=red> Stage_1 NULL VERT </color>"); next = null; }
                else if (_options.Count == 1) {next = _options[0]; } 
                else if (_options.Count == 0) { DebugSeeker("<color=red> Stage_1 0 VERT </color>"); next =  null; }
                DebugSeeker("<color=red> TRYING TO FIND STAGE 2 VERT: </color>");

                List<SplineNode> stage_2 = new List<SplineNode>();
                if (input.x >= 0)
                    stage_2 = _options.FindAll(t => (t != null) && GetScreenPos(t.transform.position).x >= GetScreenPos(refPos.position).x);
                else
                    stage_2 = _options.FindAll(t => (t != null) && GetScreenPos(t.transform.position).x <= GetScreenPos(refPos.position).x);

                if (stage_2.Count == 1)
                    next = stage_2[0];

                else if (stage_2 == null || stage_2.Count == 0)
                {
                    DebugSeeker("<color=red> STAGE 2  NOT FOUND VERT: </color>");
                    next = FindByProjection(_options, refPos.position, input);
                    return;
                }
                next = FindByProjection(stage_2, refPos.position, input);
            }

            return next;
        }
        public static SplineNode FindFromHor(Vector2 input, Transform refPos, List<SplineNode> nodes)
        {
            DebugSeeker("Finding from hor");
            List<SplineNode> _options = new List<SplineNode>();
            SplineNode next = null;
            if (nodes == null) DebugSeeker("nodes are null");
            if (refPos == null) DebugSeeker("ref pos is null");
            if (nodes.Count == 1) { DebugSeeker("only one res return HOR"); DebugSeeker("only one res return"); return nodes[0]; }

            if (input.x >= 0)
                _options = nodes.FindAll(t => (t != null)
                && GetScreenPos(t.transform.position).x >= GetScreenPos(refPos.position).x);
            else
                _options = nodes.FindAll(t => (t != null)
                && GetScreenPos(t.transform.position).x <= GetScreenPos(refPos.position).x);

            FindFromOptions(); // will determine next;

            void FindFromOptions()
            {
                if (_options == null) { DebugSeeker("<color=red> Stage_1 NULL HOR </color>"); next = null; }
                else if (_options.Count == 1) { next = _options[0]; }
                else if (_options.Count == 0) { DebugSeeker("<color=red> Stage_1 NULL HOR </color>"); next = null; }
                DebugSeeker("<color=red> TRYING TO FIND STAGE 2 HOR: </color>");
                List<SplineNode> stage_2 = new List<SplineNode>();
                if (input.y >= 0)
                {
                    stage_2 = _options.FindAll(t => (t != null) && GetScreenPos(t.transform.position).y >= GetScreenPos(refPos.position).y);
                }
                else
                {
                    stage_2 = _options.FindAll(t => (t != null) && GetScreenPos(t.transform.position).y <= GetScreenPos(refPos.position).y);
                }

                if (stage_2.Count == 1)
                    next = stage_2[0];

                else if (stage_2 == null || stage_2.Count == 0)
                {
                    DebugSeeker("<color=red> STAGE 2  NOT FOUND HOR: </color>");
                    next = FindByProjection(_options, refPos.position, input);
                    return;
                }
                next = FindByProjection(stage_2, refPos.position, input);
            }


            return next;
        }
        private static SplineNode FindByProjection(List<SplineNode> options, Vector3 refPos, Vector2 input)
        {
  
            if (options.Count == 0)
                return null;

            SplineNode res = options[0];
            Vector2 screenRef = GetScreenPos(refPos);
            Vector2 dir = GetScreenPos(res._position) - screenRef;
            float largest = Vector2.Dot(input, dir);
            foreach (SplineNode n in options)
            {
                if (n == null) continue;
                dir = (GetScreenPos(n._position) - screenRef);
                float proj = Vector2.Dot(input, dir);
                if (proj >= largest)
                {
                    largest = proj;
                    res = n;
                }
            }
            return res;
        }
        public static List<SplineNode> ClearOptions(List<SplineNode> options, List<SplineNode> exclude)
        {
            if (exclude == null || exclude.Count == 0) { return options; }
            foreach (SplineNode n in exclude)
            {
                if (n == null) continue;
                if (options.Contains(n))
                    options.Remove(n);
            }

            return options;
        }

        private static Vector2 GetScreenPos(Vector3 worldPos)
        {
            return (Vector2)Camera.main.WorldToScreenPoint(worldPos);
        }

        public static void DebugSeeker(string message)
        {
           // Debug.Log(message);
        }
    }
}