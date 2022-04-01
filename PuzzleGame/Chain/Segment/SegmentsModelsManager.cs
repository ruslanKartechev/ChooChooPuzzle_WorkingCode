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
        public bool UseSingleModel = false;
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
        public void InitModels()
        {
            foreach(Transform t in models)
            {
                if (t != null)
                {
                    Rigidbody rb = t.GetComponent<Rigidbody>();
                    if (rb == null)
                        rb = t.gameObject.AddComponent<Rigidbody>();
                    rb.isKinematic = true;
                    Collider c = t.GetComponent<Collider>();
                    if (c == null)
                        c = t.gameObject.AddComponent<BoxCollider>();
                    c.isTrigger = true;
                    IChainLink link = t.GetComponentInParent<IChainLink>();
                    if (link != null)
                        link.InitComponents(new ChainLinkComponents(t.gameObject,rb, c));
                }
                else
                {
                    models.Remove(t);
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

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("GetModels"))
                me.GetAllModels();
            if (GUILayout.Button("InitModels"))
            {
                me.GetAllModels();
                me.InitModels();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Scale"))
            {
                me.GetAllModels();
                me.ScaleModels();
            }
              
            if (GUILayout.Button("ApplyRotation"))
            {
                me.GetAllModels();
                me.ApplyRotation();
            }
              
            GUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }

    }
}