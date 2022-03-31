using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleGame {
   
    public class PathBranchPointModels : MonoBehaviour
    {
        public GameObject CurrentModel;
        public float CurrentScale;
        public bool UseVectorScale = false;
        public Vector3 VectorScale = Vector3.one;

        private List<SplineNodeModelHandler> modelHandlers = new List<SplineNodeModelHandler>();
        private IPointsSource source;
        public void GetModels()
        {
            modelHandlers = new List<SplineNodeModelHandler>();
            source = GetComponent<IPointsSource>();
            List<Transform> points = source.GetPoints();
            foreach(Transform t in points)
            {
                modelHandlers.Add(new SplineNodeModelHandler(t));
            }
        }

        public void ShowPoints()
        {
            foreach(SplineNodeModelHandler m in modelHandlers)
            {
                m.ShowModel();
            }
        }
        public void HidePoints()
        {
            foreach (SplineNodeModelHandler m in modelHandlers)
            {
                m.HideModel();
            }
        }
        public void ResetModel()
        {
            if(CurrentModel == null) { Debug.Log("Current model is null");return; }
            foreach (SplineNodeModelHandler m in modelHandlers)
            {
                GameObject current = m.GetCurrentModel();
                if (current != null)
                    DestroyImmediate(current);
                GameObject newModel = Instantiate(CurrentModel);
                m.SetNewModel(newModel);
                if (UseVectorScale == false)
                    m.ScaleModel(CurrentScale);
                else
                    m.ScaleModel(VectorScale);
            }

        }

        public void ScaleModels()
        {
            foreach (SplineNodeModelHandler m in modelHandlers)
            {
                if (m != null)
                    m.ScaleModel(CurrentScale);
            }
        }
    }
}