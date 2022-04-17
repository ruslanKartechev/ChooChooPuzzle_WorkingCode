using System;
using System.Collections.Generic;
using UnityEngine;
using Obi;
namespace PuzzleGame
{
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
                for (int i = 0; i < countDiff; i++)
                    _rope.path.RemoveControlPoint(_rope.path.ControlPointCount - 1);
            }
            for (int i = 0; i < positions.Count; i++)
            {
                Vector3 pos = _rope.transform.InverseTransformPoint(positions[i]);
                if (i >= _rope.path.m_Points.Count)
                {

                    _rope.path.AddControlPoint(pos, Vector3.zero, Vector3.zero, Vector3.up, 1, 1, 1, 1, Color.white, "added " + i.ToString());
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
