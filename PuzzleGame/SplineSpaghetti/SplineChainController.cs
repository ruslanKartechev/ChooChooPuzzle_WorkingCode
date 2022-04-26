using Dreamteck.Splines;
using UnityEngine;
using CommonGame;
using CommonGame.Events;
using CommonGame.Sound;
namespace PuzzleGame
{
    public class SplineChainController : MonoBehaviour
    {
        [Space(10)]
        public ChainNumber Number;
        [Space(10)]
        [SerializeField] private SoundFXChannelSO _soundFXChannel;
        [Header("Components")]
        [Space(10)]
        [SerializeField] private SplineChainMoveManager _moveManager;
        [SerializeField] private SpaghettiSegmentBase _segment;
        [SerializeField] private CollisionSegmentManager _collisions;
        [SerializeField] private SplineChainParticlesBase _particles;
        [SerializeField] private ChainFinishEffectBase _finishEffect;
        [Header("Settings")]
        [Space(10)]
        [SerializeField] private SplineComputer _startSpline;
        [SerializeField] private SplineChainMoveSettings _moveSettins;
        [SerializeField] private SpaghettiSegmentSettings _segmentSettings;

        private void Awake()
        {
            InitSelf();
        }
        private void Start()
        {
            FinishMatcherController.Instance.RegisterChain(Number);
        }
        private void InitSelf()
        {
            if (_moveManager == null)
            {
                Debug.LogError("MoveManager not assigned");
                return;
            }
        
            _segment?.Init(_segmentSettings);
            _collisions?.Init();
            _collisions?.Enable();
             _moveManager?.Init( _moveSettins);
            _moveManager?.SetStartPositions(_startSpline);
            _particles?.Init(null);
            _particles.Enable();
            _moveManager.MoveStarted += OnMoveStart;
            _moveManager.MoveStopped += OnMoveStop;
         
        }
        private void OnMoveStart()
        {
            FinishMatcherController.Instance.OnChainSelected(Number);
        }
        private void OnMoveStop()
        {
            FinishMatcherController.Instance.OnChainDeselected(Number);
        }

        public void OnOtherChainCollision(Vector3 position)
        {
            if (_moveManager.IsMoving == false)
                return;
            _moveManager.Bounce(position);
        }

        private IFinish _finish;
        public void OnFinish(IFinish finish)
        {
            if (finish == null)
            {
                Debug.Log("finish is null");
                return;
            }
            if (Number == finish.GetNumber())
            {
                _particles.Disable();
                _collisions?.Disable();
                _finish = finish;
                Debug.Log("<color=green> Matched </color>");
                FinishMatcherController.Instance.ChainFinished(Number);
                finish.OnReached();
                _finishEffect.OnEffectEnd = OnChainEaten;
                _finishEffect.ExecuteEffect(finish.GetFinishPoint());
                _moveManager.Disable();
                _soundFXChannel.RaiseEventPlay(SoundNames.SpaghettiEat.ToString());
             //   CameraController.Instance.MoveToFinishCP(Number);
                FinishMatcherController.Instance.ChainCompleted(Number);
            }
            else
            {
                Debug.Log("<color=red> Not Matched </color>");
                _soundFXChannel.RaiseEventPlay(SoundNames.FinishWrong.ToString());

                finish.OnWrong();
            }
        }
        

        public void OnChainEaten()
        {
            _finish.OnCompleted();
        //    CameraController.Instance.MoveToDefaultPos();
            _soundFXChannel.RaiseEventPlay(SoundNames.FinishCorrect.ToString());
            gameObject.SetActive(false);

        }


#if UNITY_EDITOR
        public void InitEditor()
        {
            InitSelf();
        }
#endif


    }
}