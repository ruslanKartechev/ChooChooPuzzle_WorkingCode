using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISplineFollower
{
    Transform GetTransform();
    bool PushFromNode(Vector2 dir);
}