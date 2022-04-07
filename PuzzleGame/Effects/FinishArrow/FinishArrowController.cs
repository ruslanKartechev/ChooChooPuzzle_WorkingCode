using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace PuzzleGame
{
    public class FinishArrowController : MonoBehaviour
    {
        public FinishArrowAnimator _animController;
        public FinishArrowLights _lights;
        [Space(10)]
        public bool HideOnStart = true;
        public GameObject AimPlane;
        private void Start()
        {

            _animController.Init();
            _lights.Init();
            if (HideOnStart)
                Deactivate();
        }

        public void Activate()
        {
            _animController.Activate();
            _lights.Activate();
            if (AimPlane != null)
                AimPlane.SetActive(true);
        }

        public void Deactivate()
        {
            _animController.Deactivate();
            _lights.Deactivate();
            if(AimPlane != null)
                AimPlane.SetActive(false);

        }

        public void OnlyLight()
        {
            _animController.Deactivate();
            _lights.Activate();
            if (AimPlane != null)
                AimPlane.SetActive(false);
        }
    }
}