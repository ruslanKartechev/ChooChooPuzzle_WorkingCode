using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PuzzleGame
{
    public class FinishNode : SplineNode
    {
        private void Start()
        {
            InitFinishNode();
        }

        public void InitFinishNode()
        {
            _type = NodeType.Finish;
            gameObject.name = GONames.FinishNode;
        }


    }
}