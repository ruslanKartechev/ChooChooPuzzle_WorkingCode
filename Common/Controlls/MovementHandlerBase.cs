using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonGame.Controlls
{
    public class MovementHandlerBase : MonoBehaviour, IMovementHandler
    {
        [SerializeField] protected Transform _targetBody;
        [SerializeField] protected Transform TargetBody { get { return _targetBody; } }
        public virtual void Init()
        {

        }
        public virtual void AllowMovement()
        {
        }

        public virtual void DisallowMovement()
        {
        }

        public virtual void Move(Vector2 vector2)
        {
        }
    }
}