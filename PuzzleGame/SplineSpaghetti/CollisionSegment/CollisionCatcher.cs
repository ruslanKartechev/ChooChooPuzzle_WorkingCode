using UnityEngine;
using System;
namespace PuzzleGame
{
    public class CollisionCatcher : MonoBehaviour
    {
        public Action<Collider> OnCollision;
        private void OnTriggerEnter(Collider other)
        {
            OnCollision?.Invoke(other);   
        }
    }
}