using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace CommonGame.UI
{
    public class HandManager : MonoBehaviour
    {
        [SerializeField] private Transform Hand;
        [Space(10)]
        [SerializeField]  private float MaxSpeed = 1f;
        [Space(10)]
        [SerializeField] private float ScalingTime = 0.5f;
        [SerializeField] private float NormalScale = 1f;
        [SerializeField] private float PressedScale = 0.8f;
        [Space(10)]
        private Action onScaleChange = null;
        private Coroutine pointerFollowing = null;
        public void Show()
        {
            Hand.gameObject.SetActive(true);
            onScaleChange = null;
            StartCoroutine(ScaleChanging(PressedScale));
        }
        public void Hide()
        {
            StopPointerFollow();
            StartCoroutine(ScaleChanging(NormalScale));
            onScaleChange = HideHand;
        }
        public void HideHand()
        {
            Hand.gameObject.SetActive(false);
        }
        private IEnumerator ScaleChanging(float endScale)
        {
            float startScale = Hand.transform.localScale.x;
            float elapsed = 0f;
            while(elapsed <= ScalingTime)
            {
                Hand.transform.localScale = Vector3.one * Mathf.Lerp(startScale, endScale, elapsed/ScalingTime);
                elapsed += Time.deltaTime;
                yield return null;
            }
            Hand.transform.localScale = Vector3.one * endScale;

            onScaleChange?.Invoke();
        }



        public void StartPointerFollow()
        {
            StopPointerFollow();
            pointerFollowing = StartCoroutine(PointerFollowing());
        }
        public void StopPointerFollow()
        {
            if (pointerFollowing != null)
                StopCoroutine(pointerFollowing);
        }

        public void SetHandPos(Vector2 pos)
        {
            Hand.transform.position = pos;
        }

        private Vector2 virtualPosition = new Vector2();
        private IEnumerator PointerFollowing()
        {
            virtualPosition = Hand.transform.position;
            Vector2 oldPos = virtualPosition;
            Vector2 newPos = virtualPosition;
            while (true)
            {
                if (Input.GetMouseButton(0))
                {
                    Hand.transform.position = Input.mousePosition;
                }

                newPos = Input.mousePosition;
                Vector2 d = (newPos - virtualPosition);
                Vector2 delta = (newPos - virtualPosition).normalized;
                float distance = (newPos - virtualPosition).magnitude;
                if (distance >= 1)
                {
                    Vector2 move = delta * MaxSpeed * Time.deltaTime * 100f;
                    if (distance < move.magnitude)
                        move = d;
                    virtualPosition += move;
                }
                SetHandPos(virtualPosition);
                oldPos = newPos;
                yield return new WaitForFixedUpdate(); 
            }
        }
    }
}