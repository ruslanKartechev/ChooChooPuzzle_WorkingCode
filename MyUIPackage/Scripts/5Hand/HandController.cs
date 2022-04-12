using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CommonGame.UI
{
    public class HandController : MonoBehaviour
    {

        public bool DoUseHand = true;
        [Space(13)]
        [SerializeField] private HandManager _manager;
        private Coroutine inputCheck;
        
        private void Awake()
        {
            if (_manager) _manager = FindObjectOfType<HandManager>();
        }
        private void Start()
        {
            _manager.HideHand();
            if (DoUseHand)
                StartInputCheck();
        }
        public void StopInputCheck()
        {
            if (inputCheck != null) StopCoroutine(inputCheck);
        }
        public void StartInputCheck()
        {
            StopInputCheck();
            inputCheck = StartCoroutine(InputChecking());
        }
        private void OnClick()
        {
            _manager.StartPointerFollow();
            _manager.Show();
        }
        private void OnRelease()
        {
            _manager.Hide();
        }


        private IEnumerator InputChecking()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    OnClick();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    OnRelease();
                }
                yield return null;
            }
        }
        private void OnDestroy()
        {
            StopInputCheck();
        }

    }
}