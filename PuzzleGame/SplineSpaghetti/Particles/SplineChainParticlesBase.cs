using System.Collections.Generic;
using UnityEngine;
namespace PuzzleGame
{
    public abstract class SplineChainParticlesBase : MonoBehaviour
    {
        [SerializeField] protected List<ParticleSystem> _particles;
        public abstract void Init(object settings);
        public abstract void Enable();
        public abstract void Disable();
        
    }
}