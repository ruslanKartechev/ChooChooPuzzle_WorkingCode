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
        }
        public void InitSpline(MegaSpline spline)
        {
            _spline = spline;
        }
        public void Update() 
        { 

        }

    }
}
