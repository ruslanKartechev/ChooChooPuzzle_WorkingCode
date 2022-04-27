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
        [Space(6)]
        [SerializeField] private TrailRenderer _leftTrail;
        [SerializeField] private TrailRenderer _rightTrail;
        [SerializeField] private ParticleSystem _leftParticles;
        [SerializeField] private ParticleSystem _rightParticles;



        [SerializeField] private SplineMoverBase _mover;
        private SegmentPointsCalculator _pointsCalculator;
        private MoveDirection _currentDir = MoveDirection.forward;
        private void Start()
        {
            _leftParticles.Stop();
            _rightParticles.Stop();
        }
        public override void Disable()
        {
            _mover.PositionChanged -= UpdatePositions;
            _leftParticles.Stop();
            _rightParticles.Stop();
        }

        public override void Enable()
        {
            EnableLeft();
        }

        public override void Init()
        {
            _mover.PositionChanged += UpdatePositions;
            _mover.MoveStarted += OnMoveStart;
            _mover.MoveStopped += OnMoveStop;
            //EnableLeft();
            //_mover.SplineChanged += SetSpline;
        }

        private void OnMoveStart()
        {
            if (_currentDir == MoveDirection.forward)
                EnableLeft();
            else
                EnableRight();
        }
        private void OnMoveStop()
        {
            _leftParticles.Stop();
            _rightParticles.Stop();
        }

        private void UpdatePositions()
        {
            //_leftTrail.transform.position = _leftEnd.transform.position;
            //_rightTrail.transform.position = _rightEnd.transform.position;
            //_leftTrail.transform.rotation = Quaternion.LookRotation(Vector3.up);
            //_rightTrail.transform.rotation = Quaternion.LookRotation(Vector3.up);

            _leftParticles.transform.position = _leftEnd.transform.position;
            _rightParticles.transform.position = _rightEnd.transform.position;
            if (_mover.MovingDirection != _currentDir)
            {
                _currentDir = _mover.MovingDirection;
                if (_mover.MovingDirection == MoveDirection.backward)
                {
                    EnableRight();              
                }
                else
                {
                    EnableLeft();
                }
            }
        }

        public void SetSpline(SplineComputer _spline)
        {
            _pointsCalculator.CurrentSpline = _spline;
            UpdatePositions();
        }

        private void EnableLeft()
        {
            //_leftTrail.enabled = true;
            //_rightTrail.Clear();
            //_rightTrail.enabled = false;

            _rightParticles.Stop();
            _leftParticles.Play();
        }
        private void EnableRight()
        {
            //_leftTrail.enabled = false;
            //_leftTrail.Clear();
            //_rightTrail.enabled = true;

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