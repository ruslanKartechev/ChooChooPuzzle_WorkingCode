using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using System;
namespace PuzzleGame
{


    public class UnitSplineFollower : MonoBehaviour, IMovable, ISplineFollower
    {
        public SplineNode currentNode;

        private Coroutine nodeSnapping;
        protected FollowerSettings _settings;

        public delegate bool InputHandler(Vector2 input);
        //protected Action<Vector2> onMove;
        public InputHandler onMove;
        protected Segment currentSegment = null;
        [Space(10)]
        public bool OccupyNodes = true;
        #region ISplineFollower
        public virtual Transform GetTransform()
        {
            return transform;
        }
        public virtual bool PushFromNode(Vector2 dir)
        {
            return true;
        }
        #endregion


        #region IMovable
        public virtual void OnMoveStart()
        {
            onMove = SetSegment;
        }

        public virtual void OnMoveEnd()
        {
            StopNodeSnapping();
            SnapToClosest();
        }


        public virtual void TakeInput(Vector2 input)
        {


        }
        #endregion

        public virtual void InitSettings(FollowerSettings settings)
        {
            _settings = settings;
        }
        public virtual void SetCurrentNode(SplineNode node, bool snap = false)
        {
            if (node == null) return;
            currentNode = node;
            if (snap)
                transform.position = currentNode._position;
            OccupyNode();
        }

        protected void ResetCurrentNode(SplineNode node)
        {
            currentNode = node;
            OccupyNode();
            // score keeping logic
        }



        protected virtual bool SetSegment(Vector2 input)
        {
            if (input == Vector2.zero) { Debug.Log("zero input"); return false; }
            SplineNode res = FindNextNode(input);
            if (res == null)
            {
                DebugMovement("Next node NOT found");
                return false;
            }
            else
            {
                currentSegment = new Segment(currentNode, res);
                onMove = MoveAlongSegment;
                return true;
            }            
        }

        protected virtual bool MoveAlongSegment(Vector2 input)
        {
            if (currentSegment == null) return false;
            float proj = Vector2.Dot(input, (Vector2)currentSegment.Dir);
            float percent = currentSegment.currentPercent;
            if (proj > 0) // moving forward
            {
                percent += _settings.moveSpeed / 100;
                Mathf.Clamp01(percent);
                if(percent >= 0.9f)
                {
                    //DebugMovement("percent >= 50");
                    //SnapToNode(currentSegment.end);
                    ResetCurrentNode(currentSegment.end);
                    transform.position = currentNode._position;
                    currentSegment = null;
                    onMove = SetSegment;
                    return true;
                }
            }
            else // moving backward
            {
                percent -= _settings.moveSpeed / 100;
                Mathf.Clamp01(percent);
                if (percent <= 0f)
                {
                    DebugMovement("percent is 0");
                    onMove = SetSegment;
                    return true;
                }
            }

            Vector3 newPos = currentSegment.start.transform.position + currentSegment.Dir * percent;
            transform.position = newPos;
            currentSegment.currentPercent = percent;
            return true;
        }


        protected virtual void SnapToClosest()
        {
            if (currentSegment == null)
            {
                transform.position = currentNode.transform.position;
                return;
            }
            if (currentSegment.currentPercent >= 50)
                SnapToNode(currentSegment.end);
            else
                SnapToNode(currentSegment.start);
        }
        protected virtual void SnapToCurrent(bool slow)
        {
            onMove = null;
            if (slow)
            {
                float time = _settings.totalSnapTime;
                float dist = (transform.position - currentNode._position).magnitude;
                time *= Mathf.Clamp01(dist / currentSegment.Distance);
                StopNodeSnapping();
                if (time > 0)
                    nodeSnapping = StartCoroutine(SnappingToNode(currentNode.transform, time));
                else
                    onMove = SetSegment;
            }
            else
            {
                transform.position = currentSegment.start._position;
            }

        }
        protected virtual void SnapToNode(SplineNode node)
        {
            if(currentSegment == null)
            {
                DebugMovement("Segment is zero");
                return;
            }

            onMove = null;
            float time = _settings.totalSnapTime;
            float dist = (transform.position - node._position).magnitude;
            time *= Mathf.Clamp01(dist / currentSegment.Distance); 
            StopNodeSnapping();
            if(time > 0)
                nodeSnapping = StartCoroutine(SnappingToNode(node.transform,time));
            
           // currentNode = node;
            currentSegment = null; // setting segment to null
        }

        public void StopNodeSnapping()
        {
            if (nodeSnapping != null)
                StopCoroutine(nodeSnapping);
        }

        protected List<SplineNode> moveList = new List<SplineNode>();
        protected SplineNode nextNode = null;
        protected Coroutine movingAlongNodes;
        protected float listMoveTime = 0.2f;
        public virtual void MoveByDir(Vector2 input)
        {
            //SetSegment(input);
            SplineNode res = FindNextNode(input,GetLastNode());
            if (res == null) { Debug.Log("Next node not found"); return; }
            //if (moveList.Contains(res) == false)
            //    moveList.Add(res);
            //if (movingAlongNodes == null)
            //{
            //    StartCoroutine(MovingAlongNodes());
            //}

            currentSegment = null;
        }

        public virtual void MoveToNode(SplineNode node)
        {
            if (node == null) { Debug.Log("Null node passed"); return; }
            if (nodeSnapping != null) StopCoroutine(nodeSnapping);
            if (nextNode != null)
            {
                currentNode = nextNode;
                transform.position = nextNode._position;
            }
            currentNode.ReleaseNode();
            nextNode = node;
            currentNode = nextNode;
            OccupyNode();
            nodeSnapping = StartCoroutine(SnappingToNode(node.transform, listMoveTime));

        }
        public virtual void StopMovingToNode()
        {
            if (nodeSnapping != null) StopCoroutine(nodeSnapping);
            if(nextNode != null)
            {
                Debug.Log("next not null");
            }
        }
        //protected virtual IEnumerator MovingAlongNodes()
        //{
        //    while (moveList.Count > 0)
        //    {
        //        CustomSplineNode next = moveList[0];
             
        //        yield return SnappingToNode(next.transform, listMoveTime);
        //        currentNode = next;
        //        moveList.Remove(next);
        //        yield return null;
        //    }
        //}



        protected virtual IEnumerator SnappingToNode(Transform node,float time)
        {
            Vector3 start = transform.position;
            Vector3 end = node.position;
            float elapsed = 0f;
            while (elapsed <= time)
            {
                transform.position = Vector3.Slerp(start, end, elapsed / time);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.position = end;
            // ?? 
            nextNode = null;
        }

        public SplineNode GetLastNode()
        {
            if (moveList.Count == 0) return currentNode;
            else
                return moveList[moveList.Count - 1];
        }
        
        public virtual void OccupyNode()
        {
            if (currentNode == null) return;
            if(OccupyNodes)
                currentNode.SetCurrentFollower(this);
        }


        #region FindingNodes
        
        public SplineNode FindNextNode(Vector2 input, SplineNode from = null)
        {
            if (from == null)
                from = currentNode;

            if (from.linkedNodes.Count == 1)
                return from.linkedNodes[0];

            if (Mathf.Abs(input.x) >= Mathf.Abs(input.y))
            {
                var res = new List<SplineNode>();
                if (input.x >= 0)
                    res = GetHorNodes(true, from);
                else
                    res = GetHorNodes(false, from);
                if (res == null || res.Count == 0) { DebugMovement("no more right or left nodes"); return null; }

                SplineNode n = res[0];
                if (res.Count > 1)
                {
                    if (input.y >= 0)
                        n = res.Find(x => x.transform.position.y >= transform.position.y);
                    else
                        n = res.Find(x => x.transform.position.y < transform.position.y);
                }
                return n;
            }
            else
            {
                var res = new List<SplineNode>();
                if (input.x >= 0)
                    res = GetVertNodes(true, from);
                else
                    res = GetVertNodes(false, from);
                if (res == null || res.Count == 0) { DebugMovement("no more up or down nodes"); return null; }

                SplineNode n = res[0];
                if (res.Count > 1)
                {
                    if (input.x >= 0)
                        n = res.Find(x => x.transform.position.x >= transform.position.x);
                    else
                        n = res.Find(x => x.transform.position.x < transform.position.x);
                }
                return n;
            }
            return null;
  
        }

        public List<SplineNode> GetHorNodes(bool right, SplineNode from)
        {
            var res = new List<SplineNode>();
            if (right)
                res = from.linkedNodes.FindAll(x => x.transform.position.x >= transform.position.x);
            else
                res = from.linkedNodes.FindAll(x => x.transform.position.x < transform.position.x);
            return res;
        }
        public List<SplineNode> GetVertNodes(bool up, SplineNode from)
        {
            var res = new List<SplineNode>();
            if (up)
                res = from.linkedNodes.FindAll(x => x.transform.position.y >= transform.position.y);
            else
                res = from.linkedNodes.FindAll(x => x.transform.position.y < transform.position.y);
            return res;
        }

        #endregion




        protected void DebugMovement(string message)
        {
            if (_settings.DebugMessages == false) return;
            Debug.Log("<color=blue>" + message + "</color>");
        }

    }
}