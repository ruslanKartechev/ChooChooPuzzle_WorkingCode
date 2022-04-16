
using UnityEngine;
namespace PuzzleGame
{
    [System.Serializable]
    public class FinishHeadAnimator : FinishAnimator
    {
        public Animator _anim;
        private const string PassiveActive = "IdleOpen";
        private const string ActivePassive = "OpenIdle";
        public override void Init()
        {
            _anim.SetBool(PassiveActive, false);
            _anim.SetBool(ActivePassive, false);
        }

        public override void Activate()
        {
            _anim.SetBool(ActivePassive, false);
            _anim.SetBool(PassiveActive, true);
        }

        public override void Deactivate()
        {
            _anim.SetBool(PassiveActive, false);
            _anim.SetBool(ActivePassive, true);
        }

    }
}
