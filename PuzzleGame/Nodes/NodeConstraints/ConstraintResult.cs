using System.Collections;
using UnityEngine;
using System.Collections.Generic;
namespace PuzzleGame
{
    public class ConstraintResult 
    {
        public bool Allowed;
        public List<SplineNode> Options;
        public string _message;
      
    }
}