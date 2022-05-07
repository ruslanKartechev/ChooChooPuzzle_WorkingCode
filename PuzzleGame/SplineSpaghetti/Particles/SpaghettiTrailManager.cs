using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
namespace PuzzleGame
{
    public class SpaghettiTrailManager : SplineChainTrailBase
    {
        [SerializeField] private Transform _leftEnd;
        [SerializeField] private Transform _rightEnd;
        [Space(5)]
        [SerializeField] private ColorSO _colorSource;
        [Space(5)]
        [SerializeField] private ParticleSystem _leftParticles;
        [SerializeField] private ParticleSystem _rightParticles;



        [SerializeField] private SplineMoverBase _mover;
        private SegmentPointsCalculator _pointsCalculator;
        private MoveDirection _currentDir = MoveDirection.forward;
        private void Start()
        {
            _leftParticles.Stop();
            _rightParticles.Stop();
            if(_colorSource != null)
            {
                Debug.Log("Settings trail color from SO");
                ParticleSystem.MainModule mainL = _leftParticles.main;
                mainL.startColor = _colorSource.GetColor();
                ParticleSystem.ColorOverLifetimeModule colorL = _leftParticles.colorOverLifetime;
                colorL.color = _colorSource.GetLiftimeColor();

                ParticleSystem.MainModule mainR = _rightParticles.main;
                mainR.startColor = _colorSource.GetColor();
                ParticleSystem.ColorOverLifetimeModule colorR = _rightParticles.colorOverLifetime;
                colorR.color = _colorSource.GetLiftimeColor();
            }

        }
        public override void Disable()
        {
            _mover.PositionChanged -= UpdatePositions;
            _leftParticles.Stop();
            _rightParticles.Stop();
        }

        public override void Enable()
        {
            _mover.PositionChanged += UpdatePositions;
            _mover.MoveStarted += OnMoveStart;
            _mover.MoveStopped += OnMoveStop;
        }

        public override void Init()
        {

        }

        private void OnMoveStart()
        {
            _rightParticles.Play();
            _leftParticles.Play();
            UpdatePositions();
        }
        private void OnMoveStop()
        {
            _leftParticles.Stop();
            _rightParticles.Stop();
        }

        private void UpdatePositions()
        {
            _leftParticles.transform.position = _leftEnd.transform.position;
            _rightParticles.transform.position = _rightEnd.transform.position;
        }

        private void EnableLeft()
        {
            _rightParticles.Stop();
            _leftParticles.Play();
        }
        private void EnableRight()
        {
            _rightParticles.Play();
            _leftParticles.Stop();
        }

        public override void OnFinish(Vector3 finishPoint)
        {
            transform.parent = transform.parent.parent;
            Disable();
        }

    }
}