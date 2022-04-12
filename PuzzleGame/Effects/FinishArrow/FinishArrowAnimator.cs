
using UnityEngine;

namespace PuzzleGame
{

    public abstract class FinishAnimator
    {
        public abstract void Init();
        public abstract void Activate();
        public abstract void Deactivate();
    }

  




    [System.Serializable]
    public class FinishArrowAnimator : FinishAnimator
    {
        public Animator _anim;
        public override void Init()
        {
            _anim.Play("Idle");
        }
        public override void Activate()
        {
            _anim.Play("Active");
        }
        public override void Deactivate()
        {
            _anim.Play("Idle");

        }

    }
}
