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
        [SerializeField] private SplineChainTrailBase _trails;
        [Header("Settings")]
        [Space(10)]
        private SplineComputer _startSpline;
        [SerializeField] private SplineChainMoveSettings _moveSettins;
        [SerializeField] private SpaghettiSegmentSettings _segmentSettings;
        [Space(10)]
        [SerializeField] private TutorLevelEffect _tutor;
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

            _particles?.Init(null);
            _particles.Enable();
            _trails?.Init();
            _trails?.Enable();

            _moveManager?.Init( _moveSettins);
            _startSpline = _moveSettins.AvailableSplines[0];
            _moveManager?.SetStartPositions(_startSpline);

            _moveManager.MoveStarted += OnMoveStart;
            _moveManager.MoveStopped += OnMoveStop;
         
        }
        private void OnMoveStart()
        {
            FinishMatcherController.Instance.OnChainSelected(Number);
            _tutor?.OnClick();
        }
        private void OnMoveStop()
        {
            FinishMatcherController.Instance.OnChainDeselected(Number);
        }

        public void OnOtherChainCollision(Vector3 position)
        {
            if (_moveManager.IsMoving)
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
                Vector3 fnishPoint = finish.GetFinishPoint();
                _particles.Disable();
                _collisions?.Disable();
                _finish = finish;
                _trails.OnFinish(fnishPoint);
                Debug.Log("<color=green> Matched </color>");
                FinishMatcherController.Instance.ChainFinished(Number);
                finish?.OnReached();
                _finishEffect.OnEffectEnd = OnChainEaten;
                _finishEffect.ExecuteEffect(fnishPoint);
                _moveManager.Disable();
                _soundFXChannel.RaiseEventPlay(SoundNames.SpaghettiEat.ToString());
             //   CameraController.Instance.MoveToFinishCP(Number);
                FinishMatcherController.Instance.ChainCompleted(Number);
            }
            else
            {
                Debug.Log("<color=red> Not Matched </color>");
                _soundFXChannel?.RaiseEventPlay(SoundNames.FinishWrong.ToString());

                finish?.OnWrong();
            }
        }
        

        public void OnChainEaten()
        {
            if (this)
            {
                _finish?.OnCompleted();
                _soundFXChannel?.RaiseEventPlay(SoundNames.FinishCorrect.ToString());
                gameObject?.SetActive(false);
            }
        }


#if UNITY_EDITOR
        public void InitEditor()
        {
            InitSelf();
        }
#endif


    }
}