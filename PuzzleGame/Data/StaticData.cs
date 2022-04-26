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
        public const string WrongAngle = "Wrong Angle";
        public const string Blocked = "Blocked";
        public const string CloseContanctBlock = "Close Contanct Block";

    }

    public struct PuzzleTags
    {
        public const string Junction = "Junction";
        public const string Finish = "Finish";
        public const string Chain = "Chain";
    }
}