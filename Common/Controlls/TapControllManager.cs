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

    public class TapControllManager : ControllsManager
    {
        [Header("Settings")]
        [SerializeField] private int MaxRad;
        [SerializeField] private float Sensitivity;

        private StickControllsPanel _ui;
        private Coroutine _controllStickHandler;
        private IInputHandler _inputHandler;
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
        #region Base

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
            base.HideControlls();
            StopInputCheck();
            _ui.HidePanel(true);
        }
        public override void ShowControlls()
        {
            base.ShowControlls();
            _ui.ShowPanel(true);

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

        private void OnAttackButton()
        {

        }
        public override void OnRelease()
        {
            if(_ui != null)
                _ui.OnRelease();
        }


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