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
        public StretchCurveManager(StretchSegmentData data)
        {
            this.data = data;
        }
  
        public void Init()
        {
            _spline = data.Spline;
            UpdateKnots();
        }

        public void UpdateKnots()
        {
            if(_spline == null) { Debug.LogError("Spline knots were not inited"); return; }
            for(int i=0; i< _spline.pointCount; i++)
            {
                _spline.SetPointPosition(i, data.Points[i].position);
            }
           
        }
    }
}
