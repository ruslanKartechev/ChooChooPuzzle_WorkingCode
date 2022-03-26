using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonGame.Sound
{
    public interface ISoundEffect
    {
        void PlayEffectOnce();
        void StartEffect();
        void StopEffect();
    }
}
