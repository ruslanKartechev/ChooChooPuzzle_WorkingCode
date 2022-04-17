using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonGame.Controlls
{
    public abstract class InputHandler : MonoBehaviour
    {
        public abstract void Init();
        public abstract void AllowMovement();

        public abstract void DisallowMovement();

        public abstract void Move(Vector2 vector2);
    }
}