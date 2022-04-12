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
        private List<ChainSegmentData> _Links;
        private ChainEffect_Jump _settings;
        private Action<ChainSegmentData> onSegmentJump;
        private Action onEffectEnd;
        public JumpEffectHandler(ChainEffect_Jump settings, List<ChainSegmentData> links, Action<ChainSegmentData> onSegmentJump, Action onEffectEnd)
        {
            _settings = settings; _Links = links; this.onSegmentJump = onSegmentJump; this.onEffectEnd = onEffectEnd;
        }

        public override void ExecuteEffect()
        {
            Jump();
        }
        private async void Jump()
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < _Links.Count; i++)
            {
                tasks.Add( JumpSegment(_Links[i]) );
            }
            await Task.WhenAll(tasks);
            onEffectEnd?.Invoke();
        }

        private async Task JumpSegment(ChainSegmentData data)
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
                float t = elapsed / time;
                SetLinksPositions(t);
                elapsed += Time.deltaTime ;
                await Task.Yield();
            }
            SetLinksPositions(1);
            onSegmentJump?.Invoke(data);
            

            void SetLinksPositions(float t)
            {
                Vector3 pos = Bezier.GetPointQuadratic(start, inflection, end,t);
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
                factor = Mathf.Lerp(1f, endFactor, elapsed / time);
                target.localScale = startScale * factor;
                elapsed += Time.deltaTime;
                await Task.Yield();
            }
            target.localScale = startScale * endFactor;
        }

    }
}