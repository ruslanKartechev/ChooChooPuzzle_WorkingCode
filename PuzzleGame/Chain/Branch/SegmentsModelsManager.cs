using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PuzzleGame
{
    public class SegmentsModelsManager : MonoBehaviour
    {
        public float Scale = 1f;
        public Vector3 EulerAngles;
        private List<Transform> models = new List<Transform>();

        public void GetAllModels()
        {
            models = new List<Transform>();
            for (int i = 0; i < transform.childCount; i++)
            {
                ChainLink link = transform.GetChild(i).gameObject.GetComponent<ChainLink>();
                if (link == null) continue;
                if (link.transform.childCount < 1) continue;
                Transform model = link.transform.GetChild(0);
                if (models.Contains(model) == false)
                {
                    models.Add(model);
                    if(model.gameObject.name.Contains("LinkModel") == false)
                        model.gameObject.name += "_" + "LinkModel_" + i.ToString();
                }
            }

        }

        public void ScaleModels()
        {
            foreach (Transform t in models)
            {
                t.localScale = Vector3.one * Scale;
            }
        }
        public void ApplyRotation()
        {
            foreach(Transform t in models)
            {
                t.localEulerAngles = EulerAngles;
            }
        }

    }


    [CustomEditor(typeof(SegmentsModelsManager))]
    public class SegmentsModelsManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SegmentsModelsManager me = target as SegmentsModelsManager;

            if (GUILayout.Button("GetModels"))
                me.GetAllModels();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Scale"))
                me.ScaleModels();
            if (GUILayout.Button("ApplyRotation"))
                me.ApplyRotation();
            GUILayout.EndHorizontal();

        }

    }
}