using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Dreamteck.Splines;
namespace PuzzleGame
{
    public class StretchCurveManager
    {
        public StretchSegmentData data;
        private SplineComputer _spline;
        public SplineComputer Spline { get { return _spline; } }
        public StretchCurveManager(StretchSegmentData data)
        {
            this.data = data;
        }
  
        public void Init()
        {
            _spline = data.Spline;
            UpdateKnots();
        }
        
        private List<MegaKnot> InitKnots()
        {
            List<MegaKnot> result = new List<MegaKnot>(data.Points.Count);
            for (int i = 0; i < data.Points.Count; i++)
            {
                MegaKnot knot = new MegaKnot();
                knot.id = i;
                knot.p = GetShapeLocal(data.Points[i].position);
                Vector3 dist = data.Points[i].transform.position;
                if (i < data.Points.Count-1)
                {
                    dist = GetShapeLocal( data.Points[i + 1].transform.position) - GetShapeLocal(data.Points[i].transform.position);
                    knot.invec = -dist; knot.outvec = dist;
                }
                else
                {
                    dist = GetShapeLocal(data.Points[i].transform.position) - GetShapeLocal(data.Points[i - 1].transform.position);
                    knot.invec = dist; knot.outvec = -dist;
                }

                result.Add(knot);
            }
            return result;
        }

        public void UpdateKnots()
        {
            if(_spline == null) { Debug.LogError("Spline knots were not inited"); return; }
            for(int i=0; i< _spline.pointCount; i++)
            {
                _spline.SetPointPosition(i, data.Points[i].position);
            }
           
        }

        private Vector3 GetShapeLocal(Vector3 v)
        {
            return v;
         //   return data.SplineShape.transform.InverseTransformPoint(v);
        }
    }
}
