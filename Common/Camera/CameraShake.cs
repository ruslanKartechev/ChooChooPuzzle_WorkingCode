using System.Collections;
using UnityEngine;

namespace CommonGame
{

    public class CameraShake : MonoBehaviour
    {
        [SerializeField] private CamShakeSettings _settings;
        private Coroutine _shaking;
        private Vector3 startLocalPosition;

        public void Init(CamShakeSettings settings)
        {
            _settings = settings;
        }
        public void Shake()
        {
            if (_settings == null) { Debug.Log("shaking settings not assigned");return; }
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