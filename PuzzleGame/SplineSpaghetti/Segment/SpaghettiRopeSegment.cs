using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
namespace PuzzleGame
{
    public class SpaghettiRopeSegment : SpaghettiSegmentBase
    {
        [SerializeField] private Transform _pivot_l;
        [SerializeField] private Transform _pivot_c;
        [SerializeField] private Transform _pivot_r;
        [SerializeField] private float _EndVericalOffset;
        [SerializeField] private float _CenterVerticalOffset;
        [SerializeField] private SplineMoverBase _mover;

        private Vector3 _offsetVector;
        private SplineComputer _currentSpline;
        private SegmentPointsCalculator _pointsCalculator;
        private void Start()
        {
            _offsetVector = Vector3.up * _EndVericalOffset;
        }
        public override void Init(object settings)
        {
            _mover.SplineChanged += SetSpline;
            _mover.PositionChanged += UpdateSegment;
            _pointsCalculator = new SegmentPointsCalculator();
            _pointsCalculator.EndPointsVertOffset = _EndVericalOffset;
            _pointsCalculator.PointCount = 3;

            Debug.Log("Rope Segment Inited");
        }
        public override void SetSpline(SplineComputer _spline)
        {
            _pointsCalculator.CurrentSpline = _spline;
        }
        public override void UpdateSegment()
        {
            List<Vector3> positions = _pointsCalculator.GetSplinePositions(_segPivot_1, _segPivot_2);
            _pivot_l.position = positions[0];
            _pivot_c.position = positions[1] + Vector3.up * _CenterVerticalOffset;
            _pivot_r.position = positions[2];
        }
    }
}