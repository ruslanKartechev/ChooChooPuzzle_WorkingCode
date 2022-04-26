using CommonGame;
using UnityEngine;
namespace PuzzleGame
{

    public class FinishHeadView : MonoBehaviour, IFinish
    {
        public ChainNumber _number;
        [Space(10)]
        [SerializeField] private Transform _finishPoint;
        [SerializeField] private Transform _cameraCP;
        [Space(10)]
        public FinishAnimator HeadAnimator;
        public FinishHeadParticles Particles;
        private FinishAnimator _animController;
        private void Start()
        {
            _animController = HeadAnimator;
        //    CameraController.Instance.SetCameraCP(_cameraCP,_number);
            _animController.Init();
            _animController.SetParticlesHandle(Particles);
            FinishMatcherController.Instance.RegisterFinishView(this);
            
        }

        public void OnReached()
        {
            _animController?.Correct();
        }
        public void OnCompleted()
        {
            _animController?.OnFinishEnd();
        }
        public void OnWrong()
        {
            HeadAnimator?.Wrong();
        }
        public ChainNumber GetNumber()
        {
            return _number;
        }

        public void Activate()
        {
            _animController.PlayIdle();
          //  _lights.Activate();
        }

        public void Deactivate()
        {
            _animController.PlayActive();
           // _lights.Deactivate();

        }

        public Vector3 GetFinishPoint()
        {
            if(_finishPoint == null)
            {
                Debug.Log("Finish point not assigned");
                return transform.position;
            }    
            return _finishPoint.position;
        }
    }
}