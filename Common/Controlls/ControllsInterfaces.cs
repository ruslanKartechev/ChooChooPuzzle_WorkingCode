using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonGame.Controlls
{
    public interface IInputHandler
    {
        void OnInput(Vector2 input);
    }
    public interface IMovementHandler
    {
        void Move(Vector2 vector2);
        void AllowMovement();
        void DisallowMovement();

    }
}