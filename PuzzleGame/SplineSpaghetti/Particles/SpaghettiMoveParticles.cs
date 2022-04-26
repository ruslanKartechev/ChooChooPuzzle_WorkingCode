
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleGame;
public class SpaghettiMoveParticles : SplineChainParticlesBase
{
    [SerializeField]  private SplineMoverBase _mover;
    public override void Init(object settings)
    {
       
        foreach(ParticleSystem p in _particles)
        {
            if (p == null) continue;
            ParticleSystem.MainModule main = p.main;
            main.playOnAwake = false;
            main.loop = false;
        }
    }
    public override void Enable()
    {
        if (_mover != null)
            _mover.PositionChanged += OnMove;
        else
            Debug.Log("Mover not assigned");
    }
    public override void Disable()
    {
        if (_mover != null)
            _mover.PositionChanged += OnMove;
        else
            Debug.Log("Mover not assigned");
    }
    private void OnMove()
    {
        ParticleSystem p = GetRandomParticle();
        p?.Play();
    }
    private ParticleSystem GetRandomParticle()
    {
        int rand =(int) UnityEngine.Random.Range(0, _particles.Count);
        return _particles[rand];
    }

}
