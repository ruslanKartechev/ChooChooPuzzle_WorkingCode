using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace PuzzleGame
{

    public class CameraMover : MonoBehaviour
    {
        private CameraMoverViewPoints _settings;

        private Coroutine _cpMoving;
        private Vector3 DefaultPosition;
        private Quaternion DefaultRotation;
        public void Init(object settings)
        {
            _settings = (CameraMoverViewPoints)settings;
        }
        public void SetDefault(Vector3 pos, Quaternion rot)
        {
            DefaultPosition = pos;
            DefaultRotation = rot;
        }
        public void MoveToDefaultPosition()
        {
            MoveToPos(DefaultPosition, DefaultRotation);
        }
        public void MoveToFinishPos(ChainNumber number)
        {
            CamContollPoint cp = _settings.pointByNumber.Find(t => t._number == number);
            if (cp == null) { Debug.Log("Camera controll point not found: " + number.ToString());return; }
            MoveToPos(cp._point);
        }

        public void MoveToPos(Transform target)
        {
            if (_cpMoving != null) StopCoroutine(_cpMoving);
            _cpMoving = StartCoroutine( MovingToPos(target.position, target.rotation));
        }
        public void MoveToPos(Vector3 position, Quaternion rotation)
        {
            if (_cpMoving != null) StopCoroutine(_cpMoving);
            _cpMoving = StartCoroutine(MovingToPos(position, rotation));
        }

        private IEnumerator MovingToPos(Vector3 targetPos, Quaternion targetRot, Action onEnd = null)
        {
            Quaternion startRot = transform.rotation;
            Quaternion endRot = targetRot;
            Vector3 startPos = transform.position;
            Vector3 endPos = targetPos;
            float elapsed = 0f;
            float time = _settings.ControllPointsMoveTime;

            while(elapsed <= time)
            {
                transform.position = Vector3.Lerp(startPos, endPos, elapsed/time );
                transform.rotation = Quaternion.Lerp(startRot, endRot, elapsed / time);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.position = endPos;
            transform.rotation = endRot;
            onEnd?.Invoke();
        }
    }
}