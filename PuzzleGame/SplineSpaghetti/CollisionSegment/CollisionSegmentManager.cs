using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
namespace PuzzleGame
{
    public class CollisionSegmentManager : MonoBehaviour
    {
        [SerializeField] private SplineMoverBase _moveManager;
        [SerializeField] private SplineChainController _chainController;

        [Header("Settings")]
        [Space(10)]
        [SerializeField] private Transform _leftEnd;
        [SerializeField] private Transform _rightEnd;
        [SerializeField] private int _count;
        [SerializeField] private int _colliderLayer;
        [SerializeField] private string _childTag;
        [SerializeField] private float _colliderSize;
        [SerializeField] private string _childName;
 
 

        private SegmentPointsCalculator _pointsCalculator;
        private List<CollisionCatcher> _catchers;
        public void Init()
        {
            if(_pointsCalculator == null)
                _pointsCalculator = new SegmentPointsCalculator();
            if(Application.isPlaying)
                SpawnColliders();

        }
        public void Enable()
        {
            _moveManager.SplineChanged += SetCurrentSpline;
            _moveManager.PositionChanged += UpdatePositions;
            for (int i = 0; i < _catchers.Count; i++)
            {
                _catchers[i].gameObject.SetActive(true);
            }
        }
        public void Disable()
        {
            _moveManager.SplineChanged -= SetCurrentSpline;
            _moveManager.PositionChanged -= UpdatePositions;
            for (int i = 0; i < _catchers.Count; i++)
            {
                _catchers[i].gameObject.SetActive(false);
            }
        }
        private void SetCurrentSpline(SplineComputer spline)
        {
            _pointsCalculator.CurrentSpline = spline;
            UpdatePositions();
        }
        private void SpawnColliders()
        {
            _catchers = new List<CollisionCatcher>(_count);
            for (int i=0; i < _count; i++)
            {
                GameObject go = new GameObject(_childName + " " + i.ToString());
                go.transform.parent = transform;
                SphereCollider c = go.AddComponent<SphereCollider>();
                Rigidbody r = go.AddComponent<Rigidbody>();
                r.isKinematic = true;
                r.useGravity = false;
                r.constraints = RigidbodyConstraints.FreezeAll;
                c.radius = _colliderSize;
                c.isTrigger = true;
                go.layer = _colliderLayer;
                go.tag = _childTag;
                CollisionCatcher catcher = go.AddComponent<CollisionCatcher>();
                catcher.OnCollision = OnCollision;
                _catchers.Add(catcher);
            }
            _pointsCalculator.PointCount = _count;
        }
        private void UpdatePositions()
        {
            if (_pointsCalculator == null || _catchers == null || _catchers.Count == 0)
                return;
            List<Vector3> positions = _pointsCalculator.GetSplinePositions(_leftEnd, _rightEnd);
            for(int i = 0; i< _catchers.Count; i++)
            {
                _catchers[i].transform.position = positions[i];
            }

        }

        private void OnCollision(Collider other)
        {
            if (_catchers.Find(t => t.transform == other.transform) == true)
                return;
            switch (other.tag)
            {
                case PuzzleTags.Chain:
                    _chainController.OnOtherChainCollision(other.transform.position);
                    break;
                case PuzzleTags.Finish:
                    _chainController.OnFinish(other.GetComponent<IFinish>());
                    break;

            }

        }
    }
}