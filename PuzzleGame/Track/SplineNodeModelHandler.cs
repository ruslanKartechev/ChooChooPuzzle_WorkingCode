using UnityEngine;

namespace PuzzleGame

{
    public class SplineNodeModelHandler
    {
        public Transform _node;
        public GameObject _currentModel;
        public SplineNodeModelHandler(Transform node)
        {
            _node = node;
            if (node.childCount > 0)
                _currentModel = node.GetChild(0).gameObject;
        }
        public void ShowModel()
        {
            if (_currentModel != null)
                _currentModel.SetActive(true);
        }
        public void HideModel()
        {
            if (_currentModel != null)
                _currentModel.SetActive(false);
        }
        public GameObject GetCurrentModel()
        {
            GameObject model = _currentModel;
            if (model == null && _node.childCount > 0)
                model = _node.GetChild(0).gameObject;
            return model;
        }
        public void SetNewModel(GameObject model)
        {
            if (_currentModel == null)
            {
                _currentModel = model;
                _currentModel.transform.parent = _node;
            }
            else
                Debug.Log("Current model is not Destroyed");
        }
        public void ScaleModel(float scale)
        {
            if (_currentModel != null)
                _currentModel.transform.localScale = Vector3.one * scale;
        }
        public void ScaleModel(Vector3 scale)
        {
            if (_currentModel != null)
                _currentModel.transform.localScale = scale;
        }
    }
}
