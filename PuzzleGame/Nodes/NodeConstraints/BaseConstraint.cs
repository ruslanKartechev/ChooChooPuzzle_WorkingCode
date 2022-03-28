using System.Collections;
using UnityEngine;
using System.Collections.Generic;
namespace PuzzleGame
{

    [System.Serializable]
    public class BaseConstraint : IConstrained
    {
        //public NodeConstaintType type;

        public virtual ConstraintResult CheckConstraint(ConstraintData data)
        {
            SplineNode current = data.chainPositions.leadingNode;

            ConstraintResult result = new ConstraintResult();
            SplineNode next = NodeSeeker.FindNextNode(data.ScreenDirection, current, current.linkedNodes);
            if(next == null)
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