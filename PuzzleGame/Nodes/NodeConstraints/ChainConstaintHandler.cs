using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PuzzleGame
{
    public class ChainConstaintHandler
    {
        public IChainMovable chain;
        public ConstraintResult CheckConstraint(List<IConstrained> constraints, Vector2 input)
        {
            ConstraintData data = new ConstraintData();
            data.ScreenDirection = input;
            data.chainPositions = chain.GetChainPosition();
            return CheckConstraint(constraints,data);
        }
        public ConstraintResult CheckConstraint(List<IConstrained> constraints, ConstraintData data)
        {
            return constraints[constraints.Count - 1].CheckConstraint(data);
        }

        
    }


  


}