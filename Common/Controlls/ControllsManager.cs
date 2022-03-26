using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonGame.UI;
namespace CommonGame.Controlls
{
    public class ControllsManager : MonoBehaviour
    {
        protected Coroutine InputCheck;

        protected virtual IEnumerator InputChecking()
        {
            yield return null;
        }

        protected virtual void StartInputCheck()
        {
            if (InputCheck != null) StopCoroutine(InputCheck);
            InputCheck = StartCoroutine(InputChecking());
        }
        protected virtual void StopInputCheck()
        {
            if (InputCheck != null) StopCoroutine(InputCheck);

        }


        public virtual void Init(object ui)
        {

        }
        public virtual void SetMover(Object mover)
        {

        }
        public virtual void EnableControlls()
        {
            StartInputCheck();
        }
        public virtual void DisableControlls()
        {
            StopInputCheck();
        }

        public virtual void ShowControlls()
        {

        }
        public virtual void HideControlls()
        {

        }
        
        public virtual void OnClick()
        {

        }
        public virtual void OnRelease()
        {

        }

    }
}
