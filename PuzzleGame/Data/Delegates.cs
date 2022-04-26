using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
namespace PuzzleGame
{
    public delegate void Notifier();
    public delegate void MouseMoveCallback(Vector2 input);
    public delegate void SplineChangeCallback(SplineComputer spline);

}