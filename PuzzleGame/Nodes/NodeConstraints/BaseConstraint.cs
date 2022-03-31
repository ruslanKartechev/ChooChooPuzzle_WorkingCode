using System.Collections;
using UnityEngine;
using System.Collections.Generic;
namespace PuzzleGame
{

    [System.Serializable]
    public class BaseConstraint : IConstrained
    {
        public virtual ConstraintResult CheckConstraint(ConstraintData data)
        {
            SplineNode current = data.chainPositions.testerPosition; // check tester or lead
            ConstraintResult result = new ConstraintResult();
            SplineNode next = NodeSeeker.FindNextNode(data.ScreenDirection, current, current.linkedNodes,false);
            if (next == null || next == current)
            {
                result.Allowed = false;
                result.Options = null;
            }
            else
            {
                result.Allowed = true;
                result.Options = new List<SplineNode>();
                result.Options.Add(next);
            }

            return result;
        }
    }
}