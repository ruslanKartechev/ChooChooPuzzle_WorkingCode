using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using Obi;
using UnityEditor;
namespace PuzzleGame
{
#if UNITY_EDITOR
    [CustomEditor(typeof(SplineSegmentController))]
    public class SplineSegmentControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SplineSegmentController me = target as SplineSegmentController;
            if(GUILayout.Button("Init rope"))
            {
                me.InitEditor();
            }
        }
    }
#endif

    public class SplineSegmentController : ChainSegmentManager
    {
        private StretchCurveManager manager;
        private PointsPosCalculator positionCalculator;
        private PointsPlacer placer;

        private ObiRopePositioner ropePositioner;
        [SerializeField] StretchSegmentData data;

        private bool DoUpdate = false;
        private List<ChainLink> _links = new List<ChainLink>();

        private void Start()
        {
            InitRunTime();
            placer.UpdatePositions();
        }

        private List<ChainLink> InitLinks()
        {
            _links = new List<ChainLink>();
            for (int i = 0; i < data.ControllPoints.Count; i++)
            {
                ChainLink l = data.ControllPoints[i].GetComponent<ChainLink>();
                if (l == null)
                    l = data.ControllPoints[i].gameObject.AddComponent<ChainLink>();
                _links.Add(l);
            }
            return _links;
        }

        public void Update()
        {
            if (DoUpdate)
            {
                manager.UpdateKnots();
                placer.UpdatePositions();
            }
        }

        // run-time
        public void InitRunTime()
        {
            if (manager == null)
                manager = new StretchCurveManager(data);
            manager.Init();
            if (positionCalculator == null)
                positionCalculator = new PointsPosCalculator(data.Spline);
            if (placer == null)
                placer = new PointsPlacer(positionCalculator);
            placer.SetPoints(data.ControllPoints);
            placer.UpdatePositions();
        }
        // editor
        public void InitEditor()
        {
            if (manager == null)
                manager = new StretchCurveManager(data);
            manager.Init();
            if (positionCalculator == null)
                positionCalculator = new PointsPosCalculator(data.Spline);
            if (placer == null)
                placer = new PointsPlacer(positionCalculator);
            placer.SpawnPoints(data.PivotPointsCount, transform.parent, data.ControllPoints);
            placer.UpdatePositions();
        }

        public override void InitSegment(Transform pivot1, Transform pivot2)
        {

        }

        public override void StartChainMovement()
        {
            DoUpdate = true;
        }

        public override void StopChainMovement()
        {
            DoUpdate = false;
        }

        public override ChainSegmentData GetChainInfo()
        {
            ChainSegmentData data = new ChainSegmentData();
            data.type = ChainSegmentType.Spline;
            data._links = InitLinks();
            return data;
        }

        public override void UpdateSegment()
        {
            InitRunTime();
            manager.UpdateKnots();
            placer.UpdatePositions();
        }
    }


}