using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonGame;


namespace PuzzleGame
{
    public class ChainLinksPositioner : MonoBehaviour
    {
        private Coroutine movingHandler;
        [SerializeField] private bool LookAtDirection = true;

        public void StartMovement(ChainLinksInfo info, Transform end_1, Transform end_2)
        {
            StopMovement();
            if(gameObject.activeInHierarchy)
            StartCoroutine(MovingChain(info, end_1, end_2));
        }


        public void StopMovement()
        {
            if (movingHandler != null)
                StopCoroutine(movingHandler);
        }


        public void SetPositionsForced(ChainLinksInfo info, Transform end_1, Transform end_2)
        {
            List<Vector3> positions = GetPositions(end_1.position, end_2.position, info._links.Count);
            Vector3 dir = (end_2.position - end_1.position).normalized;
            for (int i = 0; i < positions.Count; i++)
            {
                info._links[i].transform.position = positions[i];
                if (LookAtDirection)
                    info._links[i].transform.rotation = Quaternion.LookRotation(dir);
            }
        }

        private IEnumerator MovingChain(ChainLinksInfo info, Transform end_1, Transform end_2)
        {
            while (true)
            {
                SetPositionsForced(info, end_1, end_2);
                yield return null;
            }
        }

        private List<Vector3>  GetPositions(Vector3 start, Vector3 end, int count)
        {
            List<Vector3> positions = new List<Vector3>(count);
            for(int i =0; i < count; i++)
            {
                Vector3 pos = Vector3.Lerp(start, end, (float)i/(count));
                positions.Add(pos);

            }
            return positions;
        }


        

    }
}
