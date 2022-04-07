
using UnityEngine;

namespace PuzzleGame
{
    [System.Serializable]
    public class FinishArrowAnimator
    {
        public string IdleState;
        public string ActiveState;
        public string paramName;
        public Animator _anim;
        public void Init()
        {
            Deactivate();
        }
        public void Activate()
        {
            // _anim.SetBool(paramName, true);
            _anim.Play(ActiveState,0,0);
        }
        public void Deactivate()
        {
            _anim.Play(IdleState, 0, 0);
            // _anim.SetBool(paramName, false);
        }

    }
}
