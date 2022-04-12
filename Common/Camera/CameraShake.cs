using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonGame
{
    [System.Serializable]
    public class CamShakeSettings
    {
        public float Duration;
        public float Magnitude;
    }
    public class CameraShake : MonoBehaviour
    {
        private CamShakeSettings _settings;
        private Coroutine _shaking;
        private Vector3 startLocalPosition;

        public void Init(CameraController controller, CamShakeSettings settings)
        {
            _settings = settings;
        }
        public void Shake()
        {
            if (_shaking != null) StopCoroutine(_shaking);
            _shaking = StartCoroutine(Shaking(_settings.Duration, _settings.Magnitude));
        }
        public void StopShaking()
        {
            transform.localPosition = startLocalPosition;
            if (_shaking != null) StopCoroutine(_shaking);
            _shaking = null;
        }
        private IEnumerator Shaking(float duration, float magnitude)
        {
            float elapsed = 0f;
            startLocalPosition = transform.localPosition;
            while(elapsed <= duration)
            {
                transform.localPosition = startLocalPosition + Random.onUnitSphere * magnitude;
                elapsed += Time.deltaTime;
                yield return null;
            }
            StopShaking();
        }


    }
}