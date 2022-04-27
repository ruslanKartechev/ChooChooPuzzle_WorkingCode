using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
namespace PuzzleGame
{
    public enum MoveDirection { forward, backward }
    public class SplineMoverBase : MonoBehaviour
    {
        #region Events
        public event Notifier MoveStarted;
        public event Notifier MoveStopped;
        public event Notifier PositionChanged;
        public event SplineChangeCallback SplineChanged;
        #endregion
        protected MoveDirection _moveDirection;
        public MoveDirection MovingDirection { get { return _moveDirection; } }
        protected virtual void OnMoveStarted()
        {
            MoveStarted?.Invoke();
        }
        protected virtual void OnMoveStopped()
        {
            MoveStopped?.Invoke();
        }
        protected virtual void OnPositionChaned()
        {
            PositionChanged?.Invoke();
        }
        protected virtual void OnSplineChange(SplineComputer spline)
        {
            SplineChanged.Invoke(spline);
        }
    }
}