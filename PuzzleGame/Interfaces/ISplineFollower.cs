using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    public enum NodePushMessage { PushSucess, PushFail, SideBlock, AngleBlock, ContactBlock}
    public struct NodePushResult
    {
        public string message;
        public bool success;
    }
    public interface ISplineFollower
    {
        Transform GetTransform();
        //Vector3 GetLengthVector();
        NodePushResult PushFromNode(ISplineFollower pusher);
        Vector3 GetSegmentVector();
    }
}