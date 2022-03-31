﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    public interface ISplineFollower
    {

        Transform GetTransform();
        bool PushFromNode(Vector2 dir);
    }
}