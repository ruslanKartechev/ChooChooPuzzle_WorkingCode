using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PuzzleGame
{
    public class SegmentsModelsManager : MonoBehaviour
    {
#if UNITY_EDITOR
        public float Scale = 1f;
        [Space(5)]
        public bool UseScale3 = false;
        public Vector3 Scale3 = new Vector3();
        [Space(10)]
        public Vector3 EulerAngles;
        private List<Transform> models = new List<Transform>();
        public SegmentModels ModelsList;

        private void Start()
        {
            GetAllModels();
            InitModels();
        }

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
                    if(model.gameObject.name.Contains("LinkMod") == false)
                        model.gameObject.name += "_" + "LinkMod" + i.ToString();
                }
            }

        }
        public void InitModels()
        {
            foreach(Transform model in models)
            {
                if (model != null)
                {
                    InitModel(model.gameObject);
                }
                else
                {
                    models.Remove(model);
                }
            }
        }

        private void InitModel(GameObject model)
        {
            Rigidbody rb = model.GetComponent<Rigidbody>();
            if (rb == null)
                rb = model.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = false;
            Collider c = model.GetComponent<Collider>();
            if (c == null)
                c = model.gameObject.AddComponent<BoxCollider>();
          //  c.isTrigger = true;
            IChainLink link = model.GetComponentInParent<IChainLink>();
            if (link != null)
                link.InitComponents(new ChainLinkComponents(model.gameObject, rb, c));
        }


        public void ScaleModels()
        {
            foreach (Transform t in models)
            {
                if (UseScale3 == false)
                    t.localScale = Vector3.one * Scale;
                else
                    t.localScale = Scale3;
            }
        }
        public void ApplyRotation()
        {
            foreach(Transform t in models)
            {
                t.localEulerAngles = EulerAngles;
            }
        }


        public void SetNewModels()
        {
            if (ModelsList == null) { Debug.LogError("Models not assigned");return; }
            if(ModelsList.ModelsInOrder.Count == 0) { Debug.LogError("Models list is 0"); return; }

            models = new List<Transform>();

            for (int i=0; i< transform.childCount; i++)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
            List<GameObject> spawnedLinks = new List<GameObject>();
            for (int i = 0; i < ModelsList.ModelsInOrder.Count; i++)
            {
                GameObject link = SpawnLink("Link " + i.ToString(), ModelsList.ModelsInOrder[i]);
                spawnedLinks.Add(link);
            }
            PlaceNewLinks(spawnedLinks);

        }
        private void PlaceNewLinks(List<GameObject> spawnedLinks)
        {
            float spacing = 0.1f;
            if (spawnedLinks == null || spawnedLinks.Count == 0) { return; }
            for(int i=0; i< spawnedLinks.Count; i++)
            {
                spawnedLinks[i].transform.localPosition = Vector3.zero + transform.forward * spacing * i / transform.localScale.x;
            }
        }


        private GameObject SpawnLink(string name, GameObject _model)
        {
            if (_model == null) { Debug.LogError("Null model given");return null; }
            GameObject link = new GameObject(name);
            link.transform.parent = transform;
            link.transform.localPosition = Vector3.zero;
            ChainLink l = link.AddComponent<ChainLink>();

            GameObject mod = PrefabUtility.InstantiatePrefab(_model) as GameObject;
            mod.transform.parent = link.transform;
            mod.transform.localEulerAngles = EulerAngles;
            mod.transform.localPosition = Vector3.zero;
            models.Add(mod.transform);
            InitModel(mod);

            return link;
        }

#endif
    }



}