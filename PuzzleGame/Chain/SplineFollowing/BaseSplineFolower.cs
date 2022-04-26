using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using System;
namespace PuzzleGame
{


    public class BaseSplineFolower : MonoBehaviour, IMovable, ISplineFollower
    {
        public SplineNode currentNode;

        private Coroutine nodeSnapping;
        protected FollowerSettings _settings;

        public delegate bool InputHandler(Vector2 input);
        public InputHandler onMove;
        protected Segment currentSegment = null;
        [Space(10)]
        public bool OccupyNodes = true;
        #region ISplineFollower
        public virtual Transform GetTransform()
        {
            return transform;
        }
        public virtual NodePushResult PushFromNode(ISplineFollower pusher)
        {
            return new NodePushResult();
        }
        public virtual bool CanBePushed()
        {
            return true;
        }
        #endregion


        #region IMovable
        public virtual void OnMoveStart()
        {
            onMove = SetupSegments;
        }

        public virtual void OnMoveEnd()
        {
            StopNodeSnapping();
            SnapToClosest();
            onMove = null;
        }


        public virtual void TakeInput(Vector2 input)
        {


        }
        #endregion


        public virtual void Prepare() { }
        public SplineNode GetCurrentNode()
        {
            return currentNode;
        }
        public void SetCurrentNode(SplineNode node)
        {
            currentNode = node;
        }

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
            OccupyCurrentNode();
        }

        protected virtual void ResetCurrentNode(SplineNode node, bool forced = false)
        {
            if (currentNode != null)
                currentNode.ReleaseNode();
            currentNode = node;
            if (forced)
                currentNode.SetCurrentFollowerForced(this);
            else
                currentNode.SetCurrentFollower(this);
        }



        protected virtual bool SetupSegments(Vector2 input)
        {
            if (input == Vector2.zero) { Debug.Log("zero input"); return false; }
            SplineNode res = NodeSeeker.FindNextNode(input, currentNode,null,null,true);
            if (res != null)
            {
                currentSegment = new Segment(currentNode, res);
                onMove = MoveOnSegment;
                return true;
            }
            else
            {
                DebugMovement("Next node NOT found");
                return false;
            }
        }

        protected virtual bool MoveOnSegment(Vector2 input)
        {
            if (currentSegment == null) return false;
            float proj = Vector2.Dot(input, (Vector2)currentSegment.Dir);
            float percent = currentSegment.currentPercent;
            if (proj > 0) // moving forward
            {
                percent += _settings.TesterSpeed / 100;
                Mathf.Clamp01(percent);
                if (percent >= 0.9f)
                {
                    //DebugMovement("percent >= 50");
                    //SnapToNode(currentSegment.end);
                    ResetCurrentNode(currentSegment.end);
                    transform.position = currentNode._position;
                    currentSegment = null;
                    onMove = SetupSegments;
                    return true;
                }
            }
            else // moving backward
            {
                percent -= _settings.TesterSpeed / 100;
                Mathf.Clamp01(percent);
                if (percent <= 0f)
                {
                    DebugMovement("percent is 0");
                    onMove = SetupSegments;
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
                float time = _settings.NodeMovingTime;
                float dist = (transform.position - currentNode._position).magnitude;
                time *= Mathf.Clamp01(dist / currentSegment.Distance);
                StopNodeSnapping();
                if (time > 0)
                    nodeSnapping = StartCoroutine(SnappingToNode(currentNode.transform, time));
                else
                    onMove = SetupSegments;
            }
            else
            {
                transform.position = currentSegment.start._position;
            }

        }
        protected virtual void SnapToNode(SplineNode node)
        {
            if (currentSegment == null)
            {
                DebugMovement("Segment is zero");
                return;
            }

            onMove = null;
            float time = _settings.NodeMovingTime;
            float dist = (transform.position - node._position).magnitude;
            time *= Mathf.Clamp01(dist / currentSegment.Distance);
            StopNodeSnapping();
            if (time > 0)
                nodeSnapping = StartCoroutine(SnappingToNode(node.transform, time));

            // currentNode = node;
            currentSegment = null; // setting segment to null
        }

        public virtual void StopNodeSnapping()
        {
            if (nodeSnapping != null)
                StopCoroutine(nodeSnapping);
        }

        protected SplineNode nextNode = null;
        protected float listMoveTime = 0.2f;
        public virtual bool MoveToNode(SplineNode node, Action onEnd = null)
        {
            if (node == null) { Debug.Log("Null node passed"); return false; }
            if (node == currentNode) return false;
            if (nodeSnapping != null) StopCoroutine(nodeSnapping);
            if (nextNode != null)
            {
                currentNode = nextNode;
                transform.position = nextNode._position;
            }
            currentNode.ReleaseNode();
            nextNode = node;
            currentNode = node;
            OccupyCurrentNode();
            nodeSnapping = StartCoroutine(SnappingToNode(nextNode.transform, listMoveTime, onEnd));
            return true;
        }

        protected virtual IEnumerator SnappingToNode(Transform node, float time, Action onEnd = null)
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
            onEnd?.Invoke();
        }

        public virtual SplineNode GetActualNode()
        {
            return currentNode;
        }

        public virtual bool OccupyCurrentNode()
        {
            if (currentNode == null) { Debug.LogError("CURRENT NODE IS NULL " + gameObject.name); return false; }
            if (OccupyNodes)
                currentNode.SetCurrentFollowerForced(this);
            return true;
        }
        public virtual Vector3 GetSegmentVector() { return Vector3.zero; }
        #region FindingNodes



        #endregion




        protected void DebugMovement(string message)
        {
            if (_settings.DebugMessages == false) return;
            Debug.Log("<color=blue>" + message + "</color>");
        }

 
    }

}