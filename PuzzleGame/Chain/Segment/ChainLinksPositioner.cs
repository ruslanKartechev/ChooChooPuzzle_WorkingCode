using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonGame;


namespace PuzzleGame
{
    public class ChainLinksPositioner : MonoBehaviour
    {
        protected Coroutine movingHandler;
        [SerializeField] protected bool LookAtDirection = true;

        protected bool IsPaused = false;
        [Header("Vertical spacing of the links")]
        [SerializeField] protected float VerticalSpacing = 0.2f;
        public virtual void StartMovement(ChainSegmentData info)
        {
            if (movingHandler != null) return;
            IsPaused = false;
            if(gameObject.activeInHierarchy)
            StartCoroutine(MovingChain(info));
        }
        public virtual void StopMovement()
        {
            if (movingHandler != null)
                StopCoroutine(movingHandler);
            movingHandler = null;
        }
        public virtual void PauseMovement()
        {
            IsPaused = true;
        }
        public virtual void ResumeMovement()
        {
            IsPaused = false;
        }
        public virtual void SetPositionsForced(ChainSegmentData info)
        {
            List<Vector3> positions = GetPositions(info.end_1.position, info.end_2.position, info._links.Count);
            Vector3 dir = (info.end_2.position - info.end_1.position).normalized;
            for (int i = 0; i < positions.Count; i++)
            {
                info._links[i].transform.position = positions[i];
                if (LookAtDirection)
                    info._links[i].transform.rotation = Quaternion.LookRotation(dir);
            }
        }
        public virtual void SetPositionsForced(ChainSegmentData info, List<Vector3> positions, bool controllRotation,Vector3 lookVector)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                info._links[i].transform.position = positions[i];
                if (controllRotation)
                    info._links[i].transform.rotation = Quaternion.LookRotation(lookVector);
            }
        }

        public virtual void SetPositionsForcedOffset(ChainSegmentData info,float percentOffset)
        {
            List<Vector3> positions = GetPositionsOffsetPercent(info.end_1.position, info.end_2.position, info._links.Count, percentOffset);
            for (int i = 0; i < positions.Count; i++)
            {
                info._links[i].transform.position = positions[i];
                if (LookAtDirection)
                {
                    Vector3 dir = (info.end_2.position - info._links[i].transform.position).normalized;
                    info._links[i].transform.rotation = Quaternion.LookRotation(dir);
                }
            }
        }

        protected virtual IEnumerator MovingChain(ChainSegmentData info)
        {
            while (true)
            {
                if (IsPaused == false)
                    SetPositionsForced(info);
                yield return null;
            }
        }

        public virtual List<Vector3>  GetPositions(Vector3 start, Vector3 end, int count)
        {
            List<Vector3> positions = new List<Vector3>(count);
            Vector3 center = Vector3.LerpUnclamped(start, end, 0.5f);
            for (int i = 0; i < count; i++)
            {
                float percent = (float)i / count;
                percent = Mathf.Clamp(percent, 0.1f, 0.9f);
                Vector3 pos = center + Vector3.up * VerticalSpacing*i;
                positions.Add(pos);
            }
            return positions;
        }

        public virtual List<Vector3> GetPositions(Vector3 center, int count)
        {
            List<Vector3> positions = new List<Vector3>(count);
            for (int i = 0; i < count; i++)
            {
                float percent = (float)i / count;
                percent = Mathf.Clamp(percent, 0.1f, 0.9f);
                Vector3 pos = center + Vector3.up * VerticalSpacing * i;
                positions.Add(pos);
            }
            return positions;
        }

        public virtual List<Vector3> GetPositionsOffsetPercent(Vector3 start, Vector3 end, int count, float offset)
        {
            List<Vector3> positions = new List<Vector3>(count);
            Vector3 center = Vector3.LerpUnclamped(start, end, 0.5f + offset);
            for (int i = 0; i < count; i++)
            {
                float percent = (float)i / count;
                percent = Mathf.Clamp(percent, 0.1f, 0.9f);
                Vector3 pos = center + Vector3.up * VerticalSpacing * i;
                positions.Add(pos);
            }
            return positions;
        }
    }
}
