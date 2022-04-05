using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame
{
    public class ChainLineTester : MonoBehaviour
    {
        [SerializeField] private LineRenderer _rend;
        private void Start()
        {
            _rend.useWorldSpace = true;
        }
        public void Show()
        {
            _rend.enabled = true;
        }
        public void Hide()
        {
            _rend.enabled = false;
        }
        public void SetPositions(Vector3 start, Vector3 end)
        {
            Vector3[] positions = new Vector3[2] { start ,end};
            //_rend.SetPosition(0, (start));
            //_rend.SetPosition(1, (end));
            _rend.positionCount = 2;
            _rend.SetPositions(positions);

        }

    }
}