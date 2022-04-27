using UnityEngine;
using System.Collections;
using CommonGame.Events;
namespace CommonGame
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private LevelLoadChannelSO _levelLoadedSO;
        [SerializeField] private GameStateSO _gameStateSO;
        public Transform levelPoint;
        public void Init()
        {
            if (levelPoint == null)
                levelPoint = transform;
        }
        public void Load(LevelData data, int index)
        {
            StartCoroutine(Loading(data,index));
        }

        private IEnumerator Loading(LevelData data, int index)
        {
            GameObject level = Instantiate(data.lvlPF, levelPoint);
            //LevelInstance currentData = level.GetComponent<LevelInstance>();
            yield return null;
            _gameStateSO?.SetLevelIndex(index);
            _levelLoadedSO?.OnLevelLoaded.Invoke(index);
        }

        public void ClearLevel()
        {

            for (int i = 0; i < levelPoint.childCount; i++)
            {
                GameObject destroyObject = levelPoint.GetChild(i).gameObject;
                DestroyImmediate(destroyObject);
            }
            
        }
    }
}



