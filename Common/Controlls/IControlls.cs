using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonGame.UI;
namespace CommonGame.Controlls
{
    public abstract class IControlls : MonoBehaviour
    {
        protected Coroutine InputCheck;

        protected abstract IEnumerator InputChecking();


        protected abstract void StartInputCheck();
        //{
        //    if (InputCheck != null) StopCoroutine(InputCheck);
        //    InputCheck = StartCoroutine(InputChecking());
        //}
        protected abstract void StopInputCheck();
        //{
        //    if (InputCheck != null) StopCoroutine(InputCheck);

        //}
        public abstract void Init(object ui);
        public abstract void SetMover(Object mover);
        public abstract void EnableControlls();
        public abstract void DisableControlls();

        public abstract void ShowControlls();
        public abstract void HideControlls();
        public abstract void OnClick();
        public abstract void OnRelease();

    }
}
