using UnityEngine;
namespace PuzzleGame
{
    [System.Serializable]
    public class ChainLinkComponents
    {
        public Rigidbody _rb;
        public Collider _coll;
        public GameObject _model;
        public ChainLinkComponents() { }
        public ChainLinkComponents(GameObject model,Rigidbody rb, Collider coll)
        {
            _model = model;
            _rb = rb;
            _coll = coll;
        }
    }
}
