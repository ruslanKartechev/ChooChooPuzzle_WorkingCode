using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PuzzleGame
{
    public class StretchSegmentViewManager
    {
        private StretchSegmentData data;
        private MegaSpline _spline;
        public StretchSegmentViewManager(StretchSegmentData data)
        {
            this.data = data;
           // this.data.Deformer.axis = MegaAxis.Y;
        }
        public void InitSpline(MegaSpline spline)
        {
            _spline = spline;
            //data.Deformer.path = data.SplineShape;
            //data.Deformer.axis = MegaAxis.Y;
        }
        public void Update()
        {
            UpdatePosition();
            UpdateRotation();
          //  UpdateLength();
        }
        public void UpdatePosition()
        {
         //   data.Deformer.transform.position = data.Points[0].transform.position;
        }
        public void UpdateRotation()
        {
            Vector3 lookVector = (data.Points[1].transform.position) - (data.Points[0].transform.position);
         //   data.Deformer.transform.rotation = Quaternion.LookRotation(-lookVector);
        }
        public void UpdateLength()
        {
            float free = 1;
         //   data.Deformer.stretch = _spline.length / free;
        }

        public Vector3 DeformerLocal(Vector3 v)
        {
            return Vector3.zero;
          //  return data.Deformer.transform.InverseTransformPoint(v);
        }


    }
}
