
using UnityEngine;

namespace PuzzleGame
{

    public abstract class FinishAnimator : MonoBehaviour
    {
        public abstract void Init();
        public abstract void SetParticlesHandle(object particles);
        public abstract void PlayIdle();
        public abstract void PlayActive();
        public abstract void OnFinishEnd();
        public abstract void Wrong();
        public abstract void Correct();
    }

  




    [System.Serializable]
    public class FinishArrowAnimator : FinishAnimator
    {
        public Animator _anim;
        public override void Init()
        {
            _anim.Play("Idle");
        }
        public override void PlayIdle()
        {
            _anim.Play("Active");
        }
        public override void PlayActive()
        {
            _anim.Play("Idle");

        }
        public override void Wrong()
        {
        }
        public override void Correct()
        {
        }
        public override void SetParticlesHandle(object particles)
        {

        }

        public override void OnFinishEnd()
        {
            throw new System.NotImplementedException();
        }
    }
}
