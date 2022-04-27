using System;
using UnityEngine;
using Dreamteck.Splines;

namespace PuzzleGame
{
    public class SplineChainMoveNode : MonoBehaviour, IMovable
    {
        [SerializeField] private GameObject _model;
        [SerializeField] private SplineChainMoveManager _moveManager;
        public SplineComputer _currentSpline { get; set; }
        private Camera _cam;
        public Action<IFinish> OnFinishEnter;

        public void Init(SplineChainMoveManager moveManager)
        {
            _cam = Camera.main;
            _moveManager = moveManager;
        }
        public void OnMoveEnd()
        {
            _moveManager.OnReleased();
        }

        public void OnMoveStart()
        {
            Debug.Log("CLICKED ");
            _moveManager.OnClicked();
        }

        public void TakeInput(Vector2 input)
        {
           // _moveManager.OnInput(input);
        }

        public bool Move(float percent, bool forward)
        {
            if (forward)
            {
                float p = GetPercent() + percent;
                if (p >= 1)
                    return false;
                transform.position = _currentSpline.Evaluate(p).position;
            }
            else
            {
                float p = GetPercent() - percent;
                if (p <= 0)
                    return false;
                transform.position = _currentSpline.Evaluate(p).position;
            }
            return true;
        }
        public void SetPosition(float percent)
        {
            transform.position = _currentSpline.Evaluate(percent).position;
        }
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public float GetPercent()
        {
            if (_currentSpline == null)
            {
                Debug.LogError("current spline is null"); return 0;
            }
            return (float)_currentSpline.Project(transform.position).percent;
        }
        public Vector2 ScreenPos()
        {
            return _cam.WorldToScreenPoint(transform.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(PuzzleTags.Finish == other.tag)
            {
                IFinish finish = other.gameObject.GetComponent<IFinish>();
                OnFinishEnter?.Invoke(finish);
            }
        }

    }
}