using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;


namespace PuzzleGame
{
#if UNITY_EDITOR
    [CustomEditor(typeof(StretchSegmentManager))]
    public class StretchSegmentControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            StretchSegmentManager me = target as StretchSegmentManager;
            if (GUILayout.Button("Init")) { me.InitSegment(null,null); me.UpdateSegment(); }
        }
    }
#endif
    public class StretchSegmentManager : ChainSegmentManager
    {
        [SerializeField] private StretchSegmentData data;
        private StretchCurveManager manager;
        private StretchSegmentViewManager viewManager;
        private bool DoUpdate = false;

        private void Start()
        {
            InitSegment(null,null) ;
        }

        public override void InitSegment(Transform pivot1, Transform pivot2)
        {
            manager = new StretchCurveManager(data);
            manager.Init();
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
            throw new NotImplementedException();
        }

        public override void UpdateSegment()
        {
            manager?.UpdateKnots();
            viewManager?.Update();
        }

        private void Update()
        {
            if(DoUpdate == true)
                UpdateSegment();
        }
    }
    
}