using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Dreamteck.Splines;
using CommonGame.Sound;
using System.Threading.Tasks;
using CommonGame.Events;
using CommonGame.UI;
namespace PuzzleGame
{

    public class SplineChainMoveManager : SplineMoverBase
    {
        [SerializeField] private SoundFXChannelSO _soundFXChannel;
        [SerializeField] private CameraShakeChannelSO _camShakeChannel;
        [Space(10)]
        [SerializeField] private SplineChainMoveNode _leftNode;
        [SerializeField] private SplineChainMoveNode _rightNode;
        [Space(10)]
        [SerializeField] private LayerMask _raycastMask;
        public bool IsMoving { get { return _isMoving; } }
        private bool _isMoving = false;

        private SplineChainMoveSettings _settings;
        private Camera _cam;

        private SplineComputer _currentSpline;
        private float _currentSplineSpeed;

        private Action<Vector2> _InputHandler;
        private bool IsActive;
        private Coroutine _raycasting;

        public void Init(SplineChainMoveSettings settings)
        {
            _settings = settings;
            _cam = Camera.main;
            _leftNode.Init(this);
            _rightNode.Init(this);
            IsActive = true;
        }
        public void SetStartPositions(SplineComputer startSpline)
        {
            if (_leftNode == null || _rightNode == null)
            {
                Debug.LogError("Left or Right node not assinged");
                return;
            }
            if (startSpline == null)
            {
                Debug.LogError("Passing null start spline");
                return;
            }
            Vector3 leftStart = startSpline.Project(_leftNode.transform.position).position;
            Vector3 rightStart = startSpline.Project(_rightNode.transform.position).position;
            _leftNode.transform.position = leftStart;
            _rightNode.transform.position = rightStart;
            SetCurrentSpline(startSpline);
            OnPositionChaned();
        }
        public void Disable()
        {
            StopAllCoroutines();
            _InputHandler = null;
            IsActive = false;
        }


        private void SetCurrentSpline(SplineComputer spline)
        {
            if(spline == null)
            {
                Debug.LogError("Trying to set null spline"); return;
            }
            _currentSpline = spline;
            _leftNode._currentSpline = spline;
            _rightNode._currentSpline = spline;
            _currentSplineSpeed = _settings.MoveSpeed / 100;
            OnSplineChange(_currentSpline);
        }


        public void OnClicked()
        {
            if (IsActive == false) return;
            _isMoving = true;
            OnMoveStarted();
            if (_raycasting != null)
                StopCoroutine(_raycasting);
            _raycasting = StartCoroutine(MouseRaycasting());
        }
        public void OnReleased()
        {
            if (IsActive == false) return;
            _isMoving = false;
            OnMoveStopped();
            if (_raycasting != null)
                StopCoroutine(_raycasting);
        }

        private IEnumerator MouseRaycasting()
        {
            while (true)
            {
                Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray,out RaycastHit hit, 100, _raycastMask))
                {
                    HandleCast(hit.point);
                }

                yield return null;
            }
        }

        private void HandleCast(Vector3 hitPos)
        {
            SplineComputer nearest = GetClosestSpline(hitPos);
            if(nearest != null && _currentSpline != nearest)
            {
                if(CheckSplineProximity(nearest) == true)
                {
                    Vector3 currentProj = _currentSpline.Project(hitPos).position;
                    Vector3 otherProj = nearest.Project(hitPos).position;
                    if(CheckSwitchNeed(currentProj,otherProj) == true)
                    {
                        SetCurrentSpline(nearest);
                    }
                    else
                    {
                        Debug.Log("No need to change spline");
                    }
                }
            }
            double hitPercent = _currentSpline.Project(hitPos).percent;
            float right = _rightNode.GetPercent();
            float left = _leftNode.GetPercent();
            if (hitPercent > right)
            {
                MoveNodes(right, left, _currentSplineSpeed, true);
            } else if (hitPercent < left)
            {
                MoveNodes(right, left, _currentSplineSpeed, false);
            }
        }
        #region FindingClosestSpline
        private SplineComputer GetClosestSpline(Vector3 hitPos)
        {
            List<SplineComputer> splineList = _settings.AvailableSplines;
            if (splineList == null || splineList.Count == 0)
            {
                Debug.LogError("Null list or no splines");
                return null;
            }
            SplineComputer nearest = splineList[0];
            float shortestDistance = (hitPos - nearest.Project(hitPos).position).magnitude;
            for (int i = 0; i <  splineList.Count; i++)
            {
                float distance = (hitPos - splineList[i].Project(hitPos).position).magnitude;
                if(distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearest = splineList[i];
                }
            }
            return nearest;
        }
        private bool CheckSplineProximity(SplineComputer spline)
        {
            float distLeft = (spline.Project(_leftNode.transform.position).position - _leftNode.transform.position).magnitude;
            if(distLeft < _settings.SplineMaxDist)
            {
                float distRight = (spline.Project(_rightNode.transform.position).position - _rightNode.transform.position).magnitude;
                if (distRight < _settings.SplineMaxDist)
                {
                    Debug.Log("max dist ok");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private bool CheckSwitchNeed(Vector3 projection_1, Vector3 projection_2)
        {
            //SplineChangeMinDist
            if((projection_1 - projection_2).magnitude >= _settings.SplineChangeMinDist)
                return true;
            else 
                return false;
        }
        #endregion

        private void MoveNodes(float rightPercent, float leftPercent, float percentDelta, bool forward)
        {
            if (forward)
            {
                _moveDirection = MoveDirection.forward;

                rightPercent += percentDelta * Time.deltaTime * 10;
                leftPercent += percentDelta * Time.deltaTime * 10;
                if(rightPercent <= 1)
                {
                    _rightNode.SetPosition(rightPercent);
                    _leftNode.SetPosition(leftPercent);
                    OnPositionChaned();
                }
            }
            else
            {
                _moveDirection = MoveDirection.backward;

                rightPercent -= percentDelta * Time.deltaTime * 10;
                leftPercent -= percentDelta * Time.deltaTime * 10;
                if (leftPercent >= 0)
                {
                    _rightNode.SetPosition(rightPercent);
                    _leftNode.SetPosition(leftPercent);
                    OnPositionChaned();

                }
            }
        }


        private bool _isLeaning = false;
        public async void Bounce(Vector3 hitPos)
        {
            if (_isLeaning == true)
                return;
            _isLeaning = true;
            if (_raycasting != null)
                StopCoroutine(_raycasting);
            float otherPercent = (float)_currentSpline.Project(hitPos).percent;
            float leanDist = 0.7f;
            float leanPercent = leanDist / _currentSpline.CalculateLength();
            if (otherPercent >= _rightNode.GetPercent())
            {
                await LeanForward(leanPercent * 0.3f);
                OnBounceMiddle(hitPos);
                await LeanBackward(leanPercent);
            } else 
            {
                await LeanBackward(leanPercent * 0.3f);
                OnBounceMiddle(hitPos);
                await LeanForward(leanPercent );
            }

            if(IsMoving == true)
            {
               // _raycasting = StartCoroutine(MouseRaycasting());
            }
            _isLeaning = false;
        }
        private void OnBounceMiddle(Vector3 pos)
        {
            HitCrossManager.Instance.ShowCross(pos);
            _soundFXChannel?.RaiseEventPlay(SoundNames.FinishWrong.ToString());
            _camShakeChannel?.RaiseEventCameraShake();
        }
        private async Task LeanForward(float percent)
        {
            float startPercent = _rightNode.GetPercent();
            while (_rightNode.GetPercent() <= startPercent + percent)
            {
                if (_rightNode.Move(_currentSplineSpeed * Time.deltaTime * 10, true) == false)
                    break;
                else
                    _leftNode.Move(_currentSplineSpeed * Time.deltaTime * 10, true);
                OnPositionChaned();
                await Task.Yield();
            }
        }
        private async Task LeanBackward(float percent)
        {
            float startPercent = _leftNode.GetPercent();
            while (_leftNode.GetPercent() >= startPercent - percent)
            {
                if (_leftNode.Move(_currentSplineSpeed * Time.deltaTime * 10, false) == false)
                    break;
                else
                    _rightNode.Move(_currentSplineSpeed * Time.deltaTime * 10, false);
                OnPositionChaned();
                await Task.Yield();
            }
        }





    }
}