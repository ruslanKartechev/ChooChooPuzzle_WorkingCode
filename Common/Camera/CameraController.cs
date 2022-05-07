
using CommonGame.Events;
using UnityEngine;
using PuzzleGame;

namespace CommonGame
{
    public class CameraController : MonoBehaviour
    {
        [Header("Event Channels")]
        public CameraShakeChannelSO _camShakeChannel;
        public CameraMoverViewPoints _moverSettings;
        [Header("Components")]
        public CameraShake _shakeManager;
        public CameraMover _mover;
        private void Awake()
        {
            _camShakeChannel.ShakeCamera = Shake;
        }

        private void Start()
        {
            _mover.Init(_moverSettings);
            _mover.SetDefault(transform.position, transform.rotation);
        }

        public void Shake()
        {
            _shakeManager?.Shake();
        }
        public void SetCameraCP(Transform target, ChainNumber number)
        {
            if (target == null) { Debug.Log("trying to assing null target point");return; }
            _moverSettings.pointByNumber.Add(new CamContollPoint(target, number));
        }
        public void MoveToFinishCP(ChainNumber number)
        {
            _mover.MoveToFinishPos(number);
        }
        public void MoveToDefaultPos()
        {
            _mover?.MoveToDefaultPosition();
        }

    }
}
