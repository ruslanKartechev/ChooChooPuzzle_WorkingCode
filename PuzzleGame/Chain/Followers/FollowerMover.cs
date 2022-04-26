using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace PuzzleGame
{
    public delegate SplineNode NodeGetter();
    public delegate bool CheckNode(SplineNode n);

    public class FollowerMover : MonoBehaviour
    {
        private FollowerSettings _settings;
        private Queue<SplineNode> _moveQueue ;
        private Coroutine _moving;
        private Coroutine _twoNodeMoving;
        private bool AcceptNodes = true;
        private bool _isBusy = false;
        private float currentMod = 1f;
        private Coroutine _accelerating;


        public Action<SplineNode> StartNotifier;
        public Action<SplineNode> NodeReachedNotifier;
        public Action StopNotifier;
        public Action OnBounceHit = null;

        public bool IsBusy { get { return _isBusy; } }
        public bool DoAccelerateOnStart = false;
        public bool UseAdditionalModifier = false;
        public void Init(FollowerSettings settings)
        {
            _settings = settings;
            _moveQueue = new Queue<SplineNode>();
        }
        public void ReEnable()
        {
            AcceptNodes = true;
        }
        public bool AddNode(SplineNode node)
        {
            if (AcceptNodes == false) return false;

            if(_moveQueue.Contains(node) == false)
            {
                _moveQueue.Enqueue(node);
                TryStart();
                //DebugMessage("<color=red>" + "Queue count: " + _moveList.Count + "</color>");
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool TryStart()
        {
            if (_moving != null) { /*DebugMessage("Already moving"); */ return false; }
            _moving = StartCoroutine(QueueMover());
            return true;
        }

        public void StopMoving()
        {
            if (_moving != null) { StopCoroutine(_moving); _moving = null; }
            _isBusy = false;
            DoAccelerateOnStart = false;
            StopNotifier.Invoke();
        }

        public bool EnableAcceleration()
        {
            if (_settings.UseAcceleration == false) return false;
            DoAccelerateOnStart = true;
            return true;
        }
        public void DisableAcceleration()
        {
            if (_accelerating != null)
            {
                StopCoroutine(_accelerating);
                _accelerating = null;
            }
            currentMod = _settings.startModifier;
            DoAccelerateOnStart = false;
        }
        private IEnumerator QueueMover()
        {
            while (true)
            {
                if (_moveQueue.Count > 0)
                {
                    yield return StartCoroutine(NodeMover(_moveQueue.Peek()) );
                    if (_moveQueue.Count > 0)
                        _moveQueue.Dequeue();
                }
                else
                {
                    StopMoving();
                    yield break;
                }
            }
        }
        private IEnumerator NodeMover(SplineNode node, Action onEnd = null)
        {
            if (node == null) { Debug.LogError("Null node passed"); yield break; }
            StopBouncing();
            Vector3 start = transform.position;
            Vector3 end = node._position;
            float elapsed = 0f;
            float time = (end - start).magnitude / _settings.TesterSpeed;
            if (time == 0) yield break;
            _isBusy = true;
            HandleAcceleration();
            while (elapsed <= time)
            {
                transform.position = Vector3.Lerp(start, end, elapsed / time);
                elapsed += Time.deltaTime * currentMod;
                yield return null;
            }
            transform.position = end;
            NodeReachedNotifier?.Invoke(node);
            onEnd?.Invoke();
        }


        private IEnumerator AccelerationHandler(float totalTime, float startVal, float endVal)
        {
            float elapsed = 0f;
            float changeSpeed = 1f;
            while(elapsed <= totalTime)
            {
                currentMod = Mathf.Lerp(startVal, endVal, elapsed/totalTime);
                elapsed += Time.deltaTime* changeSpeed;
                yield return null;
            }
        }

        private Coroutine _bouncing;
        public void Bounce(Vector3 target,  float percent, float time)
        {
            if (_bouncing == null)
                _bouncing = StartCoroutine(Bouncing(target, percent, time));
        }
        private void StopBouncing()
        {
            if (_bouncing != null) StopCoroutine(_bouncing);
            _bouncing = null;
        }

        private IEnumerator Bouncing(Vector3 to, float percent, float time)
        {
            float elapsed = 0f;
            Vector3 start = transform.position;
            Vector3 target = Vector3.Lerp(start, to, percent);
            while(elapsed <= time / 2)
            {

                transform.position = Vector3.Lerp(start, target, elapsed*2/time);
                elapsed += Time.deltaTime;
                yield return null;
            }
            elapsed = 0f;
            OnBounceHit?.Invoke();
            while (elapsed <= time / 2)
            {

                transform.position = Vector3.Lerp( target, start,elapsed * 2 / time);
                elapsed += Time.deltaTime;
                yield return null;
            }
            _bouncing = null;
        }


        
        private void HandleAcceleration()
        {
            if (DoAccelerateOnStart)
            {
                //Debug.Log("<color=red> Did start acceleration </color>");   
                float targetMod = _settings.startModifier;
                if (UseAdditionalModifier)
                    targetMod = _settings.LeadModifier;
                _accelerating = StartCoroutine(AccelerationHandler(_settings.accelerationTime, targetMod, 1));
                DoAccelerateOnStart = false;
            }
        }


        public void ClearHistory()
        {
            _moveQueue.Clear();
        }

        public void FullStop()
        {
            StopAccepting();
            StopAllCoroutines();
        }
        public void StopAccepting()
        {
            AcceptNodes = false;

        }
    }
}