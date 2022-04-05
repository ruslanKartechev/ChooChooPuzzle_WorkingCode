using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PuzzleGame
{
    public struct GONames
    {
        public const string FinishNode = "FinishNode";
    }

    public struct ConstraintMessages
    {
        public const string OK = "Movement allowed";
        public const string WrongAngle = "Wrong Angle";
        public const string Blocked = "Blocked";
        public const string End = "No options";

    }
}