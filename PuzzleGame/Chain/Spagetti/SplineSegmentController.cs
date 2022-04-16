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

    public class SplineSegmentController : MonoBehaviour
    {
        private StretchCurveManager manager;
        private PointsPosCalculator positionCalculator;
        private PointsPlacer placer;

        private ObiRopePositioner ropePositioner;
        [SerializeField] StretchSegmentData data;

        private void Start()
        {
            InitRunTime();
            placer.UpdatePositions();

        }
        public void Update()
        {
            manager.UpdateKnots();
            placer.UpdatePositions();
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
            placer.SpawnPoints(data.PivotPointsCount, transform.parent,data.ControllPoints);
            placer.UpdatePositions();
        }

    }




    public class PointsPosCalculator
    {
        private SplineComputer _spline;
        public PointsPosCalculator(SplineComputer spline)
        {
            _spline = spline;
        }
        public void SetSpline(SplineComputer spline)
        {
            _spline = spline;
        }
        public List<Vector3> GetPointPositions(int num)
        {
            if (_spline == null) { Debug.LogError("Spline was not assigned"); return null; }
            List<Vector3> result = new List<Vector3>(num);
            double percent = 0f;
            float spacing = (float)1 / (num-1);
            for(int i=0; i <= num-1; i++)
            {
                Vector3 pos = _spline.Evaluate(percent).position;
                percent += spacing;
                result.Add(pos);
            }
            return result;
        }
    }



    public class PointsPlacer
    {
        private PointsPosCalculator _calculator;
        private List<Transform> points;
        public List<Transform> Points { get { return points; } }
        private int PointsCount { get { if (points != null) return points.Count; else return 0; } }

        float Yoffset = 0.15f;
        public PointsPlacer(PointsPosCalculator calculator)
        {
            _calculator = calculator;
            points = new List<Transform>();
        }
        public void SetPoints(List<Transform> _points)
        {
            points = _points;
        }
        public void SpawnPoints(int n, Transform parent, List<Transform> container)
        {
            for(int i=0; i< n; i++)
            {
                Transform p = new GameObject(i.ToString()).transform;
                points.Add(p);
                p.parent = parent;
                container.Add(p);
            }
        }
 
        public void UpdatePositions()
        {
            if (_calculator == null) { Debug.LogError("Calculator not assigned");return; }
            List<Vector3> positions = _calculator.GetPointPositions(PointsCount);
            for(int i=0; i<points.Count; i++)
            {
                if(points[i] != null)
                    points[i].position = positions[i] + new Vector3(0,Yoffset,0);
            }
        }
    }



    public class ObiRopePositioner
    {
        private ObiRope _rope;
        public ObiRopePositioner(ObiRope rope)
        {
            _rope = rope;
        }
        public void InitPositions(List<Vector3> positions)
        {
            if (_rope.path.m_Points.Count > positions.Count)
            {
                int countDiff = _rope.path.m_Points.Count - positions.Count;
                for(int i =0; i<countDiff; i++)
                    _rope.path.RemoveControlPoint(_rope.path.ControlPointCount-1);
            }
            for (int i=0; i < positions.Count; i++)
            {
                Vector3 pos = _rope.transform.InverseTransformPoint(positions[i]);
                if(i >= _rope.path.m_Points.Count)
                {
                  
                    _rope.path.AddControlPoint(pos, Vector3.zero, Vector3.zero,Vector3.up,1,1,1,1,Color.white,"added " + i.ToString());
                } 
                else
                {
                    ObiWingedPoint p = _rope.path.m_Points.data[i];
                    p.position = pos;
                    _rope.path.m_Points.data[i] = p;
                }
            }
        }
    }

}