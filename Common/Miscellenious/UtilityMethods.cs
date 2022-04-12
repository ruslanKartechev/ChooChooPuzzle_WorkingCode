using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonGame.Helpers
{
    public static class UtilityMethods
    {
        public static List<int> GetRandomIndices(int start, int end, int count)
        {
            if ((end - start) < count) { Debug.Log("Wrong start/end input"); return null; }
            List<int> Pool = new List<int>();
            for (int i = start; i <= end; i++)
            {
                Pool.Add(i);
            }
            List<int> chosenInd = new List<int>();
            while (chosenInd.Count < count)
            {
                int ind = (int)Random.Range(0, Pool.Count);
                int rand = Pool[ind];
                Pool.RemoveAt(ind);
                chosenInd.Add(rand);
            }

            return chosenInd;
        }
        public static Transform FindByName(Transform parent, string name)
        {
            Transform result = null;
            foreach (Transform child in parent)
            {
                if (child.transform.name == name)
                    result = child;
            }

            return result;
        }
        public static Transform FindByTag(Transform parent, string tag)
        {
            Transform result = null;
            foreach (Transform child in parent)
            {
                if (child.transform.tag == tag)
                    result = child;
            }

            return result;
        }

        public static Vector3 GetRandomUILocalEulers()
        {
            return new Vector3(0, 0, Random.Range(0, 180));
        }   
    }
}