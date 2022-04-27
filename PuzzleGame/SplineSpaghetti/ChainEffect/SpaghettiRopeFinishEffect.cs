using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using CommonGame.Bezier;
namespace PuzzleGame
{
    public class SpaghettiRopeFinishEffect : ChainFinishEffectBase
    {
        [SerializeField] private List<Transform> _controllPoints = new List<Transform>();
        [SerializeField] private float _jumpTime = 0.5f;
        [SerializeField] private float _infPointHeight = 0.5f;
        public override async void ExecuteEffect(Vector3 finishPoint)
        {
            for (int i = 0; i < _controllPoints.Count; i++)
            {
                if(i == _controllPoints.Count - 1)
                {
                    JumpPoint(_controllPoints[i], _jumpTime, finishPoint, OnLast);
                    await Task.Delay(ToMs(_jumpTime));
                }
                else
                {
                    JumpPoint(_controllPoints[i], _jumpTime, finishPoint, null);
                    await Task.Delay(ToMs(_jumpTime * 0.65f));
                }
            

    
            }
      
        }
        private async void JumpPoint( Transform target, float time, Vector3 endPos, Action onEnd = null)
        {
            float elapsed = 0f;
            Vector3 start = target.position;
            Vector3 inflection = Vector3.Lerp(start, endPos, 0.5f) + Vector3.up * _infPointHeight;
            while (elapsed <= time)
            {
                Vector3 t = Bezier.GetPointQuadratic(start, inflection, endPos, elapsed / time);
                target.transform.position = t;
                elapsed += Time.deltaTime;
                await Task.Yield();
            }
            target.transform.position = endPos;
            onEnd?.Invoke();
        }
        private void OnLast()
        {
            OnEffectEnd?.Invoke();
        }

        private int ToMs(float sec)
        {
            return (int)(sec * 1000);
        }

    }
}