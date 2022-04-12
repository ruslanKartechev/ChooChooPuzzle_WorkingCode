using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PuzzleGame
{
    public class FinishViewController : MonoBehaviour
    {
        public ChainNumber number;
        [Space(10)]
        public FinishHeadAnimator HeadAnimator;
        public FinishArrowLights _lights;
        [Space(10)]
        public bool HideOnStart = true;

        private FinishAnimator _animController;
        private void Start()
        {
            _animController = HeadAnimator;

            _animController.Init();
            _lights.Init();
            if (HideOnStart)
                Deactivate();
            FinishMatcherController.Instance.RegisterFinishView(this);
        }

        public void OnFinishReched()
        {

        }
        public void SetClosed()
        {

        }

        public void Activate()
        {
            _animController.Activate();
            _lights.Activate();
        }

        public void Deactivate()
        {
            _animController.Deactivate();
            _lights.Deactivate();

        }

        public void OnlyLight()
        {
            _animController.Deactivate();
            _lights.Activate();
        }
    }
}