using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace PuzzleGame
{
    public enum ChainEffects { Start,Stop,Shake,Break}
    public class ChainEffectsController
    {
        private ChainEffectsSettings _Settings;
        private List<ChainSegmentInfo> _Links;
        private List<ChainLinksPositioner> _Positioners;
        public ChainEffectsController(ChainEffectsSettings settings, List<ChainSegmentManager> managers)
        {
            _Settings = settings;
            List<ChainSegmentInfo> segmentsLinks = new List<ChainSegmentInfo>();
            List<ChainLinksPositioner> positioners = new List<ChainLinksPositioner>();
            foreach (ChainSegmentManager manager in managers)
            {
                if (manager != null)
                {
                    segmentsLinks.Add(manager.GetChainInfo());
                    positioners.Add(manager._Positioner);
                }
            }
            _Links = segmentsLinks;
            _Positioners = positioners;

        }
        public ChainEffectsController(ChainEffectsSettings settings, List<ChainSegmentInfo> Info, List<ChainLinksPositioner> positioners)
        {
            _Settings = settings;
            _Links = Info;
            _Positioners = positioners;
        }
        public void ExecuteEffect(ChainEffects effect)
        {
            switch (effect)
            {
                case ChainEffects.Shake:
                    EffectCommandExecutioner ex = new ShakeCommandExecutioner(_Links, _Settings.shaking,_Positioners);
                    ex.ExecuteEffect();

                    break;
                case ChainEffects.Start:
                    break;
                case ChainEffects.Stop:
                    break;
                case ChainEffects.Break:
                    break;

            }
        }
    }
    

    public class EffectCommandExecutioner
    {
        public virtual void ExecuteEffect()
        {

        }
    }

    public class ShakeCommandExecutioner: EffectCommandExecutioner
    {
        protected List<ChainSegmentInfo> segments;
        protected ChainEffect_Shaking _settings;
        protected List<ChainLinksPositioner> _positioners;
        public ShakeCommandExecutioner(List<ChainSegmentInfo> info, ChainEffect_Shaking settings, List<ChainLinksPositioner> positioners)
        {
            segments = info;
            _settings = settings;
            _positioners = positioners;
        }
        public override void ExecuteEffect()
        {
            Shake();
        }

        private async void Shake()
        {
            float elapsed = 0f;
            //foreach (ChainLinksPositioner pos in _positioners)
            //    pos.PauseMovement();
            while (elapsed <= _settings.ShakeTime)
            {
                foreach (ChainSegmentInfo seg in segments)
                {
                    foreach (ChainLink link in seg._links)
                    {
                        link.Components._model.transform.localPosition = UnityEngine.Random.onUnitSphere*_settings.ShakeMagnitude;
                    }
                }
                elapsed += Time.deltaTime;
                await Task.Yield();
            }
            foreach (ChainSegmentInfo seg in segments)
            {
                foreach (ChainLink link in seg._links)
                {
                    link.Components._model.transform.localPosition = Vector3.zero;
                }
            }
            //foreach (ChainLinksPositioner pos in _positioners)
            //    pos.ResumeMovement();

        }
    }


}