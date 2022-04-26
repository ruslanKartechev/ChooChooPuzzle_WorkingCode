using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    public class FinishHeadParticles : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> _particles = new List<ParticleSystem>();
        public bool loop = false;
        private void Start()
        {
            foreach (ParticleSystem p in _particles)
            {
                ParticleSystem.MainModule m = p.main;
                m.loop = loop;
                m.playOnAwake = false;
                p.gameObject.SetActive(true);
            }
        }
        
        public void Play()
        {
            foreach (ParticleSystem p in _particles)
            {
                p.Play();
            }
        }


        public void ShowParticles()
        {
            foreach (ParticleSystem p in _particles)
            {
                p.gameObject.SetActive(true);
            }
        }
        public void HideParticles()
        {
            foreach(ParticleSystem p in _particles)
            {
                p.gameObject.SetActive(false);
            }
        }
    }
}