using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace PuzzleGame
{
    public enum ChainEffects { Start,Stop,Shake,Break, LeanForward}
    public class ChainEffectsManager
    {
        private ChainEffectsSettings _Settings;
        private List<ChainSegmentData> _Links;
        private ChainPositionInfo _ChainPositions;
        public ChainEffectsManager(ChainEffectsSettings settings, List<ChainSegmentManager> managers)
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

        public ChainEffectsManager(ChainEffectsSettings settings, List<ChainSegmentData> Info)
        {
            _Settings = settings;
            _Links = Info;
        }

        public void ExecuteEffect(ChainEffects effect, ChainPositionInfo currentPositions)
        {
            switch (effect)
            {
                case ChainEffects.Shake:
                    EffectCommandHandler shake = new ShakeEffectHandler(_Links, _Settings.shaking);
                    shake.ExecuteEffect();
                    break;
                case ChainEffects.LeanForward:
                    EffectCommandHandler lean = new LeanEffectHadnler(_Links, _Settings.leaning,_ChainPositions);
                    lean.ExecuteEffect();
                    break;
            }
            _ChainPositions = currentPositions;
        }

        public void JumpTo(Transform targetPoint, Action<ChainSegmentData> onSegmentEnd, Action onEffectFinish)
        {
            _Settings.jumping.Target = targetPoint;
            EffectCommandHandler jump = new JumpEffectHandler(_Settings.jumping, _Links, onSegmentEnd, onEffectFinish);
            jump.ExecuteEffect();
        }

    }

    public abstract class EffectCommandHandler
    {
        public abstract void ExecuteEffect();
    }

    
}