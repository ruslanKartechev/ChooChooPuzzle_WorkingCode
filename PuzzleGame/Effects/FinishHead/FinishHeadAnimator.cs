
using UnityEngine;
namespace PuzzleGame
{
    [System.Serializable]
    public class FinishHeadAnimator : FinishAnimator
    {
        private enum HeadAnimState { active, passive, shaking, eating}
        public Animator _anim;
        private const string PassiveActive = "IdleOpen";
        private const string ActivePassive = "OpenIdle";
        private FinishHeadParticles _particles;

        public override void Init()
        {
            _anim.SetBool(PassiveActive, false);
            _anim.SetBool(ActivePassive, false);
        }

        public override void PlayIdle()
        {
            _anim.SetBool(ActivePassive, false);
            _anim.SetBool(PassiveActive, true);
        }

        public override void PlayActive()
        {
            _anim.SetBool(PassiveActive, false);
            _anim.SetBool(ActivePassive, true);
        }
        public override void Wrong()
        {
            _anim.Play("Wrong", 0, 0);
        }
        public override void Correct()
        {
            _anim?.Play("Eat", 0, 0);
        }

        public override void OnFinishEnd()
        {
            _anim?.Play("Idle");
        }

        public override void SetParticlesHandle(object particles)
        {
            _particles = (FinishHeadParticles)particles;
        }

        public void OneBiteEvent()
        {
            _particles.Play();
        }

    }
}
