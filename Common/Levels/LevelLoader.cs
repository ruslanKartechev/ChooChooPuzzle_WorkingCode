using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace CommonGame
{
    public class LevelLoader : MonoBehaviour
    {
        private LevelManager manager;
        public Transform levelPoint;
        public void Init(LevelManager _manager)
        {
            manager = _manager;
            if (levelPoint == null)
                levelPoint = transform;
        }
        public void Load(LevelData data)
        {
            StartCoroutine(Loading(data));
        }

        private IEnumerator Loading(LevelData data)
        {
            GameObject level = Instantiate(data.lvlPF, levelPoint);
            LevelInstance currentData = level.GetComponent<LevelInstance>();
            GameManager.Instance._data._currentLevel = currentData;

            yield return null;
            GameManager.Instance._events.LevelLoaded.Invoke();

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



