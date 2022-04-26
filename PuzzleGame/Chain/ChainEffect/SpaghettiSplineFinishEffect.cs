using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using System;
using System.Threading;
using System.Threading.Tasks;
using CommonGame.Bezier;
namespace PuzzleGame
{
    public class SpaghettiSplineFinishEffect : ChainFinishEffectBase
    {
        public SplineComputer mySpline;
        [Space(10)]
        [SerializeField] private float _jumpSpeed = 1f;
        [SerializeField] private float _infPointHeight = 1.5f;
        public override async void ExecuteEffect(Vector3 finishPoint)
        {
            List<Task> moveTasks = new List<Task>();
            for(int i=0; i < mySpline.pointCount; i++)
            {
                moveTasks.Add( JumpQueue(i, _jumpSpeed, GetMoveQueue(finishPoint, i) ));
            }
            await Task.WhenAll(moveTasks);
            OnEffectEnd?.Invoke();
        }

        // index 0 == last point
        private List<Vector3> GetMoveQueue(Vector3 finish, int index)
        {
            List<Vector3> queue = new List<Vector3>();
            if(index == mySpline.pointCount-1)
            {
                queue.Add(finish);
                return queue;
            }
            for(int i = index+1; i < mySpline.pointCount - 1; i++)
            {
                queue.Add(mySpline.GetPointPosition(i));
            }
            queue.Add(finish);
            return queue;

        }
        private List<Vector3> GetMoveQueueInv(Vector3 finish, int index)
        {
            List<Vector3> queue = new List<Vector3>();
            if (index == 0)
            {
                queue.Add(finish);
                return queue;
            }
            for (int i = 0; i < index; i++)
            {
                queue.Add(mySpline.GetPointPosition(i));
            }
            queue.Add(finish);
            return queue;

        }


        private async Task JumpQueue(int index, float speed, List<Vector3> positions)
        {
            for(int i=0; i<positions.Count; i++)
            {
                float elapsed = 0f;
                Vector3 start = mySpline.GetPointPosition(index);
                Vector3 end = positions[i] + Vector3.up * _infPointHeight;
                Vector3 inf = Vector3.Lerp(start, end, 0.5f);
                float time = ( end-start ).magnitude / speed;
                while (elapsed <= time)
                {
                    Vector3 pos = Bezier.GetPointQuadratic(start, inf, end, elapsed/time);
                    mySpline.SetPointPosition(index,pos);
                    elapsed += Time.deltaTime;
                    await Task.Yield();
                }
            }

        }


    }
}