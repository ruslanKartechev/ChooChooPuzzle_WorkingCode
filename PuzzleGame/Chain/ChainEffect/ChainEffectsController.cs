using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace PuzzleGame
{
    public enum ChainEffects { Start,Stop,Shake,Break, LeanForward}
    public class ChainEffectsController
    {
        private ChainEffectsSettings _Settings;
        private List<ChainSegmentData> _Links;
        private ChainPositionInfo _ChainPositions;
        public ChainEffectsController(ChainEffectsSettings settings, List<ChainSegmentManager> managers)
        {
            _Settings = settings;
            List<ChainSegmentData> segmentsLinks = new List<ChainSegmentData>();
            foreach (ChainSegmentManager manager in managers)
            {
                if (manager != null)
                {
                    segmentsLinks.Add(manager.GetChainInfo());
                }
            }
            _Links = segmentsLinks;
            
        }

        public ChainEffectsController(ChainEffectsSettings settings, List<ChainSegmentData> Info)
        {
            _Settings = settings;
            _Links = Info;
        }

        public void ExecuteEffect(ChainEffects effect, ChainPositionInfo currentPositions)
        {
            switch (effect)
            {
                case ChainEffects.Shake:
                    EffectCommandExecutioner shake = new ShakeCommandExecutioner(_Links, _Settings.shaking);
                    shake.ExecuteEffect();
                    break;
                case ChainEffects.LeanForward:
                    EffectCommandExecutioner lean = new LeanCommanExecutioner(_Links, _Settings.leaning,_ChainPositions);
                    lean.ExecuteEffect();
                    break;
                //case ChainEffects.Start:
                //    break;
                //case ChainEffects.Stop:
                //    break;
                //case ChainEffects.Break:
                //    break;

            }
            _ChainPositions = currentPositions;
        }
    }

    public abstract class EffectCommandExecutioner
    {
        public abstract void ExecuteEffect();
    }


}