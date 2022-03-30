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
        public void InitOnCutReciver(Action<ChainLink> onCut)
        {
            OnCutCall = onCut;
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
            OnCutCall?.Invoke(this);
        }

        public void Drop()
        {
            _components._rb.isKinematic = false;
            _components._rb.useGravity = true;
            _components._coll.isTrigger = false;
            _components._rb.AddForce(UnityEngine.Random.onUnitSphere * 15, ForceMode.Impulse);
            detector.Deactivate();

        }

    }
}