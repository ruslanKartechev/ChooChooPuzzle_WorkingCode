using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{

    [CreateAssetMenu(fileName = "SegmentModels", menuName = "ScriptableObjects/SegmentModels", order = 1)]
    public class SegmentModels : ScriptableObject
    {
        public List<GameObject> ModelsInOrder = new List<GameObject>();
    }
}
