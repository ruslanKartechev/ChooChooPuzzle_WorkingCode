using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace PuzzleGame
{
    public class LeanCommanExecutioner : EffectCommandExecutioner
    {
        protected List<ChainSegmentData> segments;
        protected ChainEffect_Lean _settings;
        protected ChainPositionInfo followerPositions;
        bool _isMoving = false;
        
        public LeanCommanExecutioner(List<ChainSegmentData> info, ChainEffect_Lean settings, ChainPositionInfo currentPositions)
        {
            segments = info;
            _settings = settings;
            followerPositions = currentPositions;
        }
        public override void ExecuteEffect()
        {
            Lean();
        }

        private async void Lean()
        {
            if (_isMoving == true) return;
            _isMoving = true;
            bool forward = GetDirection();
            await Forward(_settings.LeanTime,_settings.Amount, forward);
            await Relapse(_settings.RelapseTime, _settings.Amount, forward);
            _isMoving = false;

        }

        private async Task Forward(float time,float amount,bool forward)
        {
      
            float elapsed = 0f;
            for (int i = 0; i < segments.Count; i++)
            {
                segments[i]._positioner.PauseMovement();
            }
            while (elapsed <= time)
            {
                for (int i = 0; i < segments.Count; i++)
                {
                    float offset = Mathf.Lerp(0, amount, elapsed / time);
                    if (!forward)
                        offset *= -1;
                    segments[i]._positioner.SetPositionsForcedOffset(segments[i], offset);
                }
                elapsed += Time.deltaTime;
                await Task.Yield();
            }
            for (int i = 0; i < segments.Count; i++)
            {
                segments[i]._positioner.ResumeMovement();
            }
      
        }

        private async Task Relapse(float time, float amount, bool forward)
        {
            _isMoving = true;
            float elapsed = 0f;
            for (int i = 0; i < segments.Count; i++)
            {
                segments[i]._positioner.PauseMovement();
            }
            while (elapsed <= time)
            {
                for (int i = 0; i < segments.Count; i++)
                {
                    float offset = Mathf.Lerp(amount, 0, elapsed / time);
                    if (!forward)
                        offset *= -1;
                    segments[i]._positioner.SetPositionsForcedOffset(segments[i], offset);
                }
                elapsed += Time.deltaTime;
                await Task.Yield();
            }
            for (int i = 0; i < segments.Count; i++)
            {
                segments[i]._positioner.ResumeMovement();
            }
            _isMoving = false;
        }

        public bool GetDirection()
        {
            if (followerPositions.leadingNode == followerPositions.chainNodes[0])
                return false;
            else if (followerPositions.leadingNode == followerPositions.chainNodes[followerPositions.chainNodes.Count - 1])
                return true;
            else
                return true;
        }
        //private List<Vector3> GetDirections()
        //{
        //    List<Vector3> directions = new List<Vector3>();
        //    if(followerPositions.leadingNode == followerPositions.chainNodes[0])
        //    {
        //        Debug.Log("Backwards");
        //        foreach (ChainSegmentData seg in segments)
        //        {
        //            directions.Add((seg.end_1.position - seg.end_2.position).normalized);
        //        }

        //    }
        //    else if (followerPositions.leadingNode == followerPositions.chainNodes[followerPositions.chainNodes.Count-1])
        //    {
        //        Debug.Log("Forward");
        //        foreach (ChainSegmentData seg in segments)
        //        {
        //            directions.Add((seg.end_2.position - seg.end_1.position).normalized);
        //        }
        //    }

        //    return directions;
        //}



    }
}
