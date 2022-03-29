using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PuzzleGame
{
    public class KnifeManager : MonoBehaviour
    {
        
        private void OnTriggerEnter(Collider other)
        {
            ICuttable c = other.GetComponent<ICuttable>();
            if (c != null)
                c.Cut();
        }
    }

}