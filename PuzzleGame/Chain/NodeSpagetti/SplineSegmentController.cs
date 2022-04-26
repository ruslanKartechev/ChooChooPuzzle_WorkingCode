using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using Obi;
using UnityEditor;
namespace PuzzleGame
{
    public class SplineSegmentController : ChainSegmentManager
    {
        private StretchCurveManager manager;
        [SerializeField] StretchSegmentData data;

        private bool DoUpdate = false;

        private void Start()
        {
            Init();
        }

        public void Update()
        {
            if (DoUpdate)
            {
                manager.UpdateKnots();
                //placer.UpdatePositions();
            }
        }

        // run-time
        public void Init()
        {
            if (manager == null)
                manager = new StretchCurveManager(data);
            manager.Init();
            manager.UpdateKnots();
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
            ChainSegmentData chainData = new ChainSegmentData();
            chainData.type = ChainSegmentType.Spline;
            chainData._links = data._Links;
            return chainData;
        }

        public override void UpdateSegment()
        {
            Init();
            manager.UpdateKnots();
        }
    }


}