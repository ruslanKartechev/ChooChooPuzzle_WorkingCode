using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonGame.Bezier;
namespace PuzzleGame
{
    public class JumpEffectHandler : EffectCommandHandler
    {
        private List<ChainSegmentData> _Segments;
        private ChainEffect_Jump _settings;
        private Action<ChainSegmentData> onSegmentJump;
        private Action onEffectEnd;

        private bool Stop = false;
        public void StopEffect()
        {
            Stop = true;
        }

        public JumpEffectHandler(ChainEffect_Jump settings, List<ChainSegmentData> segments, Action<ChainSegmentData> onSegmentJump, Action onEffectEnd)
        {
            _settings = settings; _Segments = segments; this.onSegmentJump = onSegmentJump; this.onEffectEnd = onEffectEnd;
        }

        public override void ExecuteEffect()
        {
            JumpByType();
        }

        private async void JumpByType()
        {
            List<Task> tasks = new List<Task>();
            foreach (ChainSegmentData data in _Segments)
            {
                switch (data.type)
                {
                    case ChainSegmentType.Chain:
                        tasks.Add( JumpChain(data) );
                        break;
                    case ChainSegmentType.Spline:
                        tasks.Add( JumpSpline(data) );
                        break;
                }
            }
            await Task.WhenAll(tasks);
            onEffectEnd?.Invoke();
        }


        private async Task JumpSpline(ChainSegmentData data)
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < data._links.Count; i++)
            {
                tasks.Add(JumpSegmentSpline(data._links[i].transform) );
            }
            await Task.WhenAll(tasks);
            onSegmentJump?.Invoke(data);
        }
        private async Task JumpSegmentSpline(Transform controllPoint)
        {
            Vector3 start = controllPoint.position;
            Vector3 end = _settings.Target.transform.position;
            Vector3 inflection = Vector3.Lerp(start, end, 1);
            inflection.y = _settings.BezierHeight;
            float time = (end - start).magnitude / _settings.JumpSpeed;
            float elapsed = 0;
            while (elapsed <= time)
            {
                if (Stop == true)
                    return;
                float t = elapsed / time;
                Vector3 pos = Bezier.GetPointQuadratic(start, inflection, end, t);
                controllPoint.transform.position = pos;
                elapsed += Time.deltaTime;
                await Task.Yield();
            }
            controllPoint.transform.position = Bezier.GetPointQuadratic(start, inflection, end, 1);
        }

        private async Task JumpChain(ChainSegmentData data)
        {
            await JumpSegmentChain(data);
            onSegmentJump?.Invoke(data);
        }

        private async Task JumpSegmentChain(ChainSegmentData data)
        {
            Vector3 start = data._links[0].transform.position;
            Vector3 end = _settings.Target.transform.position;
            Vector3 inflection = Vector3.Lerp(start, end, 0.5f);
            data._positioner.StopAllCoroutines();
            inflection.y = _settings.BezierHeight;
            float time = (end - start).magnitude / _settings.JumpSpeed;
            float elapsed = 0;
            for(int i=0; i < data._links.Count; i++)
            {
                ScaleDecrease(time, data._links[i].transform);
            }
            while (elapsed <= time)
            {
                if (Stop == true)
                    return;
                float t = elapsed / time;
                SetLinksPositions(t);
                elapsed += Time.deltaTime ;
                await Task.Yield();
            }
            SetLinksPositions(1);
            
            void SetLinksPositions(float t)
            {
                if (data._positioner == null) { Debug.Log("No links positioner");return; }
                Vector3 pos = Bezier.GetPointQuadratic(start, inflection, end, t);
                List<Vector3> linksPos = data._positioner.GetPositions(pos, data._links.Count);
                data._positioner.SetPositionsForced(data, linksPos, false, Vector3.one);
            }
        }

        private async void ScaleDecrease(float time, Transform target)
        {
            float elapsed = 0f;
            Vector3 startScale = target.localScale;
            float endFactor = 0.15f;
            float factor = 1f;
            while(elapsed <= time)
            {
                if (Stop == true)
                    return;
                factor = Mathf.Lerp(1f, endFactor, elapsed / time);
                target.localScale = startScale * factor;
                elapsed += Time.deltaTime;
                await Task.Yield();
            }
            target.localScale = startScale * endFactor;
        }

       

    }
}