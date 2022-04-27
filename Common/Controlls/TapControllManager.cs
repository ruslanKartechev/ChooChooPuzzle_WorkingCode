using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonGame.UI ;
namespace CommonGame.Controlls
{
    public enum InputTypes
    {
        ControllStick,
        AttackButton,
        None
    }

    public class TapControllManager : IControllsManager
    {
        [Header("Settings")]
        [SerializeField] private int MaxRad;
        [SerializeField] private float Sensitivity;

        private Coroutine _controllStickHandler;
        private IInputHandler _inputHandler;

        #region Init
        public override void Init(object ui)
        {
        }
        public override void SetInputHandler(Object inpHandler)
        {
            _inputHandler = (IInputHandler)inpHandler;
        }
        #endregion

        #region OnOff
        public override void EnableControlls()
        {
            StartInputCheck();
        }
        public override void DisableControlls()
        {
            StopInputCheck();
        }
        public override void HideControlls()
        {
            StopInputCheck();
      
        }
        public override void ShowControlls()
        {
      
        }
        protected override void StartInputCheck()
        {
            if (InputCheck != null) StopCoroutine(InputCheck);
            InputCheck = StartCoroutine(InputChecking());
        }
        protected override void StopInputCheck()
        {
            if (InputCheck != null) StopCoroutine(InputCheck);
            InputCheck = null;
        }


        protected override IEnumerator InputChecking()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                    OnClick();
                else if (Input.GetMouseButtonUp(0))
                    OnRelease();

                yield return null;
            }
        }
        #endregion

        #region Events
        public override void OnClick()
        {

        }
        public override void OnRelease()
        {

        }
        private void OnAttackButton()
        {

        }
        #endregion

        #region ControllStick
        protected void StartControllStickHandler()
        {
            StopControllStickHandler();
            _controllStickHandler = StartCoroutine(ControllStickHandler());
        }
        protected void StopControllStickHandler()
        {
            if (_controllStickHandler != null) StopCoroutine(_controllStickHandler);
        }
        protected IEnumerator ControllStickHandler()
        {
            while (true)
            {

                yield return null;
            }
        }
        #endregion

        private void OnDisable()
        {
            OnRelease();
            StopInputCheck();
            StopAllCoroutines();
        }


    }
}