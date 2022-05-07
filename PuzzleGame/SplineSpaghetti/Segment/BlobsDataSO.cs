using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    [CreateAssetMenu(fileName = "BlobsDataSO", menuName = "SO/BlobsDataSO", order = 1)]
    public class BlobsDataSO : ScriptableObject
    {
        public List<BlobPositionData> BlobPositions = new List<BlobPositionData>();
    }
}