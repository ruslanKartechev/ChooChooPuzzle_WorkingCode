using System.Collections;
using UnityEngine;

namespace PuzzleGame
{
    public interface IConstrained
    {
        ConstraintResult CheckConstraint(ConstraintData data);
        
    }
}