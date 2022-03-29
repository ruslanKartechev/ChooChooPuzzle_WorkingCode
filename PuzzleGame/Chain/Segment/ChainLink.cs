using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace PuzzleGame
{



    public class ChainLink : MonoBehaviour, IChainLink
    {
        [SerializeField] private ChainLinkComponents _components;
        private ChainLinkCollisionDetector detector;
        public Action<ChainLink> OnCutCall;

        private void Start()
        {
            InitCuttable();
            detector.onCut = OnCutDetected;
        }
        public void InitComponents(ChainLinkComponents components)
        {
            _components = components;
        }

        public void InitCuttable()
        {
            if (_components == null || _components._model == null)
            { Debug.Log("ChainLink Components not assigned"); return; }
            detector = _components._model.GetComponent<ChainLinkCollisionDetector>();
            if(detector == null)
                detector = _components._model.AddComponent<ChainLinkCollisionDetector>();
        }
        public void OnCutDetected()
        {
            OnCutCall.Invoke(this);
        }
        public void BreakLink()
        {
            _components._rb.isKinematic = false;
            _components._rb.constraints = RigidbodyConstraints.None;
            _components._coll.isTrigger = false;
            Destroy(this);
        }

    }
}