using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace PuzzleGame
{
    public class ShakeEffectHandler : EffectCommandHandler
    {
        protected List<ChainSegmentData> segments;
        protected ChainEffect_Shaking _settings;
        public ShakeEffectHandler(List<ChainSegmentData> info, ChainEffect_Shaking settings)
        {
            segments = info;
            _settings = settings;
        }
        public override void ExecuteEffect()
        {
            Shake();
        }

        private async void Shake()
        {
            List<Task> tasks = new List<Task>();
            foreach (ChainSegmentData seg in segments)
            {
                foreach (ChainLink link in seg._links)
                {
                    tasks.Add(ShakeOne(link.Model.transform, _settings.ShakeTime, _settings.ShakeMagnitude));
                }
            }
            await Task.WhenAll(tasks);
        }

        private async Task ShakeOne(Transform target, float time, float magnitude)
        {
            Vector3 startpos = target.localPosition;
            float elapsed = 0;
            while (elapsed <= time)
            {
                target.localPosition = UnityEngine.Random.onUnitSphere * magnitude;
                elapsed += Time.deltaTime;
                await Task.Yield();
            }
            target.localPosition = startpos;
        }

    }

  
}
