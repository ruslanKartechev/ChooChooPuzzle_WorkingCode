using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace PuzzleGame
{
    public enum AddType { last,first}
    public class PathBranch : MonoBehaviour, IPointsSource
    {

        [Header("Inspector settings")]

        [Space(8)]
        public AddType addType = AddType.last;
        [Space(8)]
        [Header("Settings")]
        public List<Transform> _transforms = new List<Transform>();
        [Header("NodePF")]
        public GameObject NodePF;

        [Header("Spacing")]
        public float spacing = 2f;
        [Header("Connect on start")]
        public bool AutoConnect = true;
        private void OnEnable()
        {
            SetNodeNeighbours();

        }

        public void AddNode()
        {
            CleanList();
            if (addType == AddType.first)
                AddToFirst();
            else if (addType == AddType.last)
                AddToLast();

        }
        private void CleanList()
        {
            _transforms.RemoveAll(t => t == null);
        }
        private void AddToLast()
        {
            Transform n = ((GameObject)PrefabUtility.InstantiatePrefab(NodePF)).transform;
            n.parent = gameObject.transform;
            n.eulerAngles = Vector3.zero;
            if (_transforms.Count == 0)
            {
                ReName();
                _transforms.Add(n);
                return;
            }
            Transform prev = _transforms[_transforms.Count - 1];
            if (prev == null)
            { _transforms.Remove(prev); }
            else
                n.position = prev.position + prev.forward * UnityEngine.Random.Range(0.1f, 0.3f);
            _transforms.Add(n);
            ReName();
        }

        private void AddToFirst()
        {
            Transform n = ((GameObject)PrefabUtility.InstantiatePrefab(NodePF)).transform;
            ReName();
            n.parent = gameObject.transform;
            n.parent.localScale = Vector3.one;
            n.SetAsFirstSibling();
            if (_transforms.Count == 0)
            {
                _transforms.Add(n);
                ReName();
                return;
            }
            Transform prev = _transforms[0];
            if (prev == null) { _transforms.Remove(prev); }
            else
                n.position = prev.position - prev.right * UnityEngine.Random.Range(0.1f, 0.3f);
            _transforms.Insert(0, n);
            ReName();
        }

        public void ReName()
        {
            if (_transforms.Count == 0) return;
            for (int i = 0; i < _transforms.Count; i++)
            {
                if (_transforms[i] != null)
                    _transforms[i].name = "Node + " + (i + 1).ToString();
            }
        }

        public void SpaceFromLast()
        {
            if (_transforms.Count < 2) return;
            int ind = _transforms.Count - 1;
            List<Vector3> directions = new List<Vector3>(_transforms.Count - 1);
            for (int i = ind; i > 0; i--)
            {
                Vector3 dir = (_transforms[i - 1].position - _transforms[i].position).normalized;
                directions.Add(dir);
                //_transforms[i - 1].position = _transforms[i].position + dir * spacing;
            }
            for (int i = ind; i > 0; i--)
            {
                _transforms[i - 1].position = _transforms[i].position + directions[directions.Count - i] * spacing;
            }
        }
        public void SpaceFromFirst()
        {
            if (_transforms.Count < 2) return;
            int ind = 0;
            List<Vector3> directions = new List<Vector3>(_transforms.Count - 1);
            for (int i = ind; i < _transforms.Count - 1; i++)
            {
                Vector3 dir = (_transforms[i + 1].position - _transforms[i].position).normalized;
                directions.Add(dir);
            }
            for (int i = ind; i < _transforms.Count - 1; i++)
            {
                _transforms[i + 1].position = _transforms[i].position + directions[i] * spacing;
            }
        }
        public void CenterAround_F()
        {
            if (_transforms.Count == 0) return;
            foreach(Transform t in _transforms)
            {
                t.parent = null;
            }
            transform.position = _transforms[0].position;
            foreach (Transform t in _transforms)
            {
                t.parent = transform;
            }
            #region Old
            //if (_transforms.Count == 1)
            //{
            //    Debug.Log("1 case");
            //    _transforms[0].position = transform.position;
            //    return;
            //}
            //List<Vector3> directions = new List<Vector3>(_transforms.Count - 1);
            //for (int i = 1; i < _transforms.Count; i++)
            //{
            //    directions.Add(_transforms[i].position - _transforms[0].position);

            //}
            //_transforms[0].localPosition = Vector3.zero;
            //for (int i = 1; i < _transforms.Count; i++)
            //{
            //    _transforms[i].position = _transforms[0].position + directions[i - 1];
            //}
            #endregion
        }
        public void CenterAround_L()
        {
            if (_transforms.Count == 0) return;
            if (_transforms.Count == 0) return;
            foreach (Transform t in _transforms)
            {
                t.parent = null;
            }
            transform.position = _transforms[_transforms.Count-1].position;
            foreach (Transform t in _transforms)
            {
                t.parent = transform;
            }
            #region Old
            //if (_transforms.Count == 1)
            //{
            //    _transforms[0].position = transform.position;
            //    return;
            //}
            //List<Vector3> directions = new List<Vector3>(_transforms.Count - 1);
            //for (int i = 0; i < _transforms.Count - 1; i++)
            //{
            //    directions.Add(_transforms[i].position - _transforms[_transforms.Count - 1].position);
            //}
            //_transforms[_transforms.Count - 1].localPosition = Vector3.zero;
            //for (int i = 0; i < _transforms.Count - 1; i++)
            //{
            //    _transforms[i].position = _transforms[_transforms.Count - 1].position + directions[i];
            //}
            #endregion
        }

        public void SetDepth()
        {
            foreach (Transform t in _transforms)
            {
                t.localPosition = new Vector3(t.localPosition.x,
                    0, t.localPosition.z);

            }
        }


        public void UpdatePoints()
        {
            if (_transforms == null || _transforms.Count == 0) return;
            foreach (Transform t in _transforms)
            {
                if (t == null)
                    _transforms.Remove(t);
            }
        }

        public void SetNodeNeighbours()
        {
            GetAllPoints();
            List<SplineNode> nodes = new List<SplineNode>(_transforms.Count);
            
            foreach (Transform t in _transforms)
            {
                SplineNode node = t.GetComponent<SplineNode>();
                if (node != null) nodes.Add(node);
            }
            for (int i = 0; i < nodes.Count; i++)
            {
                if (i < nodes.Count - 1)
                    nodes[i].ConnectNode(nodes[i + 1]);
                if (i > 0)
                    nodes[i].ConnectNode(nodes[i - 1]);
            }
        }

        public void GetAllPoints()
        {
            _transforms = new List<Transform>(transform.childCount);
            for(int i =0; i< transform.childCount; i++)
            {
                _transforms.Add(transform.GetChild(i));
            }
        }

        public List<Transform> GetPoints()
        {
            return _transforms;

        }
    }


   


}