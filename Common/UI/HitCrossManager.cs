using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonGame.Helpers;
//using System;
//using System.Threading;
//using System.Threading.Tasks;
namespace CommonGame.UI
{
    public class HitCrossManager : SingletonMB<HitCrossManager>
    {
        [Header("Components")]
        public Transform Cross;
        public Animator mAnim;
        public float ShowTime = 0.3f;
        //
        private Coroutine _coundown;
        private void Start()
        {
            HideCross();
        }
        public void ShowCross(Vector3 worldPos)
        {
            Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            Cross.transform.position = screenPos;
            Cross.transform.localEulerAngles = UtilityMethods.GetRandomUILocalEulers();
            Cross.gameObject.SetActive(true);
            if (_coundown != null) StopCoroutine(_coundown);
            _coundown = StartCoroutine(DisplayingCross());
        }


        public void HideCross()
        {
            Cross.gameObject.SetActive(false);
            Cross.transform.position = Vector2.zero;
        }

        private IEnumerator DisplayingCross()
        {
            yield return new WaitForSeconds(ShowTime);
            HideCross();
            _coundown = null;
        }


    }
}