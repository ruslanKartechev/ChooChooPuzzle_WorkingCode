using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
namespace PuzzleGame
{ 
    public class SpaghettiExtrudeSegment : SpaghettiSegmentBase
    {
        [SerializeField] private SplineComputer m_spline;
        [SerializeField] private SplineMesh _mesh;
        [SerializeField] private float _endPointsHeight = 0.2f;
        [SerializeField] private float _elevation = 0.2f;
        [SerializeField] private SpaghettiSegmentSettings _settings;
        [SerializeField] private SplineMoverBase _mover;
        [SerializeField] public List<Transform> ControllPoints = new List<Transform>();
        private SegmentPointsCalculator _pointsCalculator;

        private void Start()
        {
            if (m_spline == null)
                m_spline = GetComponent<SplineComputer>();
            if (m_spline == null)
            {
                Debug.LogError("MYSplineComputer is null and not found");
                return;
            }
        }
        public override void  Init(object settings)
        {
            _pointsCalculator = new SegmentPointsCalculator();
            _pointsCalculator.EndPointsVertOffset = _endPointsHeight;
            _pointsCalculator.AllPointsVertOffset = _elevation;
            _pointsCalculator.PointCount = m_spline.pointCount;
            _settings = (SpaghettiSegmentSettings)settings;
            _mover.PositionChanged += UpdateSegment;
            _mover.SplineChanged += SetSpline;
            if (ControllPoints.Count == 0)
                Debug.Log("Controll points are not assigned");
        }

        public override void SetSpline(SplineComputer _spline)
        {
            _pointsCalculator.CurrentSpline = _spline;
            UpdateSegment();
        }


        public override void UpdateSegment()
        {
            List<Vector3> positions = _pointsCalculator.GetSplinePositions(_segPivot_1, _segPivot_2);
            for (int i = 0; i < positions.Count; i++)
            {
                m_spline.SetPointPosition(i, positions[i]);
                if( i < ControllPoints.Count)
                {
                    if(ControllPoints[i] != null)
                        ControllPoints[i].transform.position = positions[i];
                }
            }
        }

        private List<Vector3> GetRandomizedPositions(List<Vector3> exact)
        {
            if (_settings == null) { Debug.Log("Segment settings not assigned");return exact; }
            for(int i=0; i < exact.Count; i++)
            {
                float x = UnityEngine.Random.Range(-_settings.RandomizeRange, _settings.RandomizeRange);
                float z = UnityEngine.Random.Range(-_settings.RandomizeRange, _settings.RandomizeRange);
                exact[i] += new Vector3(x,0,z);
            }
            return exact;
        }


    }
}