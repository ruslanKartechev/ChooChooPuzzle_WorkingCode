using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;


namespace PuzzleGame
{
#if UNITY_EDITOR
    [CustomEditor(typeof(StretchSegmentController))]
    public class StretchSegmentControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            StretchSegmentController me = target as StretchSegmentController;
            if (GUILayout.Button("Init")) { me.InitSegment();me.UpdateSegment(); }
        }
    }
#endif
    public class StretchSegmentController : ChainSegmentManager
    {
        [SerializeField] private StretchSegmentData data;
        private StretchCurveManager manager;
        private StretchSegmentViewManager viewManager;

        private void Start()
        {
            InitSegment();
        }

        public override void InitSegment()
        {
            manager = new StretchCurveManager(data);
            manager.Init();
         //   viewManager = new StretchSegmentViewManager(data);
            //viewManager.InitSpline(manager.Spline);
        }

        public override void StartChainMovement()
        {

        }

        public override void StopChainMovement()
        {

        }
        public override ChainSegmentData GetChainInfo()
        {
            throw new NotImplementedException();
        }

        public void UpdateSegment()
        {
            manager?.UpdateKnots();
            viewManager?.Update();
        }

        private void Update()
        {
            UpdateSegment();
        }
    }
    
}