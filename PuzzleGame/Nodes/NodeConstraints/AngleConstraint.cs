using System.Collections;
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
            ConstraintResult res = base.CheckConstraint(data);
            if (res.Allowed == false || res.Options == null) return res;

            int lead = data.chainPositions.chainNodes.IndexOf(data.chainPositions.leadingNode);
            int preLead = data.chainPositions.chainNodes.Count - 2;
            if (lead == 0)
                preLead = 1;
            Vector3 inner = data.chainPositions.chainNodes[preLead]._position - data.chainPositions.chainNodes[lead]._position;

            SplineNode node = res.Options[0];
            Vector3 outer = node._position - data.chainPositions.chainNodes[lead]._position;
            Debug.DrawRay(data.chainPositions.chainNodes[preLead]._position, inner, Color.blue, 2f);
            Debug.DrawRay(data.chainPositions.chainNodes[lead]._position, outer, Color.blue, 2f);
            float angle = GetAngle(inner, outer, Vector3.forward);
            if (angle < 90 && angle != 0)
                res.Allowed = false;
            else
                res.Allowed = true;
            Debug.Log("Angle is: " + angle);

            return res;

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