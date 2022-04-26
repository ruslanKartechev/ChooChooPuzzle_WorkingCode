using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonGame.Server {
    public class ServerDataLoader : MonoBehaviour
    {
        public void StartLoading()
        {
            StartCoroutine(Load());
        }

        private IEnumerator Load()
        {
            // 
            //
            yield return null;

        }
    }
}