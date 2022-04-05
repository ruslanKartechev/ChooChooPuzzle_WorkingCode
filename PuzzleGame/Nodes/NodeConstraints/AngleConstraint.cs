using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    [System.Serializable]
    public class AngleConstraint : BaseConstraint
    {
        public float minAngle = 90;
        public float maxAngle = 360;
        public override ConstraintResult CheckConstraint(ConstraintData data)
        {
            ConstraintResult res = new ConstraintResult();
            res.Allowed = true;
            res.Options = new List<SplineNode>();
            res.Options.AddRange( data.chainPositions.leadingNode.linkedNodes);

            int lead = data.chainPositions.chainNodes.IndexOf(data.chainPositions.leadingNode);
            int preLead = data.chainPositions.chainNodes.Count - 2;
            if (lead == 0)
                preLead = 1;
            List<SplineNode> removeNodes = new List<SplineNode>();
            Vector3 _inner = data.chainPositions.chainNodes[preLead]._position - data.chainPositions.chainNodes[lead]._position;
            foreach (SplineNode n in res.Options)
            {
                if (n == null) { Debug.Log("null problem"); }
                if (n == data.chainPositions.leadingNode) continue;
                Vector3 _outer = n._position - data.chainPositions.chainNodes[lead]._position;
                Debug.DrawRay(data.chainPositions.chainNodes[lead]._position,_inner,Color.blue,1f);
                Debug.DrawRay(data.chainPositions.chainNodes[lead]._position, _outer, Color.red,1f);
                if (CheckAngle(_inner, _outer) == false)
                {
                    //Debug.Log("<color=red> Removing node: </color>" + n.gameObject.name); 
                    removeNodes.Add(n);
                }
            }
            //Debug.Log("<color=red> Remove nodes count: </color>" + removeNodes.Count);

            res.Options.RemoveAll(x => removeNodes.Contains(x));
            res.Options.RemoveAll(x => x == data.chainPositions.leadingNode);
            if(res.Options.Count == 0)
            {
                res.Allowed = false;
                res.Options = null;
                res._message = ConstraintMessages.WrongAngle;
            }
            else
            {
                res._message = ConstraintMessages.OK;
                //Debug.Log("options: " + res.Options.Count);
            }
            return res;

        }
        private bool CheckAngle(Vector3 _inner, Vector3 _outer)
        {
            float angle = GetAngle(_inner, _outer, Vector3.forward);
            //Debug.Log("<color=green>Angle: </color>" + angle);
            if (angle == 0 || angle == 180)
            {
                return true;
            }
            else if (angle >= minAngle && angle <= maxAngle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public float GetAngle(Vector3 v1, Vector3 v2, Vector3 axis)
        {
            // angle in [0,180]
            float angle = Vector3.Angle(v1, v2);
            float sign = Mathf.Sign(Vector3.Dot(axis, Vector3.Cross(v1, v2)));
            float signed_angle = angle * sign;
            //float angle360 =  (signed_angle + 360) % 360;

            return angle;
        }

    }
}