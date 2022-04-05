using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame {
    public class ConfettiManager : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> particles;
        public void InitParticles(List<ParticleSystem> _particles)
        {
            particles = _particles;
        }
        public void ShootOnce()
        {
            foreach(ParticleSystem p in particles)
            {
                if (p == null) continue;
                ParticleSystem.MainModule main = p.main;
                main.loop = false;
                p.gameObject.SetActive(true);
                p.Play();
            }
        }
        public void ShootLoop()
        {
            foreach (ParticleSystem p in particles)
            {
                if (p == null) continue;
                ParticleSystem.MainModule main = p.main;
                main.loop = true;
                p.gameObject.SetActive(true);
                p.Play();
            }
        }
        public void StopAll()
        {
            foreach (ParticleSystem p in particles)
            {
                if (p == null) continue;
                p.Stop();
            }
        }


        public void OnDisable()
        {
            StopAll();
        }
    }
}