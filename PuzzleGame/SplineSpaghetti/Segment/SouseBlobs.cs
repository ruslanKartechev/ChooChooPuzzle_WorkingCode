using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
namespace PuzzleGame
{
    [System.Serializable]
    public class BlobPositionData
    {
        public float percent;
        public Vector3 offset;
    }
    public class SouseBlobs : MonoBehaviour
    {
        [SerializeField] private List<Transform> _blobs = new List<Transform>();
        [Space(5)]
        [SerializeField] private BlobsDataSO _data;
        [Space(5)]
        [SerializeField] private SplineComputer _spline;
        [SerializeField] private bool DoDebug = false;
        public void Init()
        {
            for(int i =0; i<transform.childCount; i++)
            {
                if (i < _data.BlobPositions.Count)
                    _blobs.Add( transform.GetChild(i) );
                else
                    Debug.Log("Too many child blobs, positions not found");
            }
        }

        public void SavePositions()
        {
            _data.BlobPositions.Clear();
            _data.BlobPositions = new List<BlobPositionData>(transform.childCount);
            for (int i = 0; i < transform.childCount; i++)
            {
                BlobPositionData blobData = new BlobPositionData();
                Transform blob = transform.GetChild(i);
                _blobs.Add(blob);
                SplineSample projection = _spline.Project(blob.position);
                blobData.percent = (float)projection.percent;
                blobData.offset = (blob.position - projection.position);
                _data.BlobPositions.Add(blobData);
            }
        }
        public void UpdatePositions()
        {
            for (int i = 0; i < _data.BlobPositions.Count; i++)
            {
                _blobs[i].position = _spline.EvaluatePosition(_data.BlobPositions[i].percent) + _data.BlobPositions[i].offset;
            }
        }
        private void Update()
        {
            UpdatePositions();
        }
    }
}