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

    public class TapControllManager : IControlls
    {
        [Header("Settings")]
        [SerializeField] private int MaxRad;
        [SerializeField] private float Sensitivity;

        private StickControllsPanel _ui;
        private Coroutine _controllStickHandler;
        private IInputHandler _inputHandler;

        #region Init
        public override void Init(object ui)
        {
            _ui = (StickControllsPanel)ui;
            _ui.MaxRad = MaxRad;
            _ui.Sensitivity = Sensitivity;
        }
        public override void SetMover(Object inpHandler)
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
            _ui.HidePanel(true);
        }
        public override void ShowControlls()
        {
            _ui.ShowPanel(true);
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
            switch (_ui.OnClick())
            {
                case InputTypes.ControllStick:
                    StartControllStickHandler();
                    break;
                case InputTypes.AttackButton:
                    OnAttackButton();
                    break;
            }
        }
        public override void OnRelease()
        {
            if (_ui != null)
                _ui.OnRelease();
        }
        private void OnAttackButton()
        {

        }
        #endregion

        #region ControllStick
        protected void StartControllStickHandler()
        {
            _ui.StartStickMovement();
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
                _inputHandler.OnInput(_ui.GetDirVector());
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