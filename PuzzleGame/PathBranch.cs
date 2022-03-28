using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace PuzzleGame
{
    public enum AddType { last,first}
    public class PathBranch : MonoBehaviour
    {
       
        [Header("Inspector settings")]
        [Range(0f,1f)]
        public float DrawSphereSize = 1f;
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


        private void Start()
        {
           
        }
        private void OnEnable()
        {
            if (AutoConnect)
                SetNodeNeighbours();
        }

        public void AddPoint()
        {
            if (addType == AddType.first)
                AddToFirst();
            else if (addType == AddType.last)
                AddToLast();
        }
        private void AddToLast()
        {
            Transform n = ((GameObject)PrefabUtility.InstantiatePrefab(NodePF)).transform;
            n.name = gameObject.name + " " + _transforms.Count.ToString();
            n.parent = gameObject.transform;
            n.parent.localScale = Vector3.one * DrawSphereSize;
            if (_transforms.Count == 0)
            {
                _transforms.Add(n);
                return;
            }
            Transform prev = _transforms[_transforms.Count - 1];
            n.position = prev.position + prev.right * UnityEngine.Random.Range(0.1f, 0.3f);
            _transforms.Add(n);
        }
        private void AddToFirst()
        {
            Transform n = ((GameObject)PrefabUtility.InstantiatePrefab(NodePF)).transform;
            n.name = gameObject.name +  " 0";
            ReName();
            n.parent = gameObject.transform;
            n.parent.localScale = Vector3.one * DrawSphereSize;
            n.SetAsFirstSibling();
            if (_transforms.Count == 0)
            {
                _transforms.Add(n);
                return;
            }
            Transform prev = _transforms[0];
            n.position = prev.position - prev.right * UnityEngine.Random.Range(0.1f, 0.3f);
            _transforms.Insert(0,n);
        }
        public void ReName()
        {
            if (_transforms.Count == 0) return;
            for (int i = 0; i < _transforms.Count; i++)
                _transforms[i].name = "Node + " + (i + 1).ToString();
        }

        public void SpaceFromLast()
        {
            if (_transforms.Count < 2) return;
            int ind = _transforms.Count-1;
            List<Vector3> directions = new List<Vector3>(_transforms.Count-1);
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
            if(_transforms.Count == 1)
            {
                Debug.Log("1 case");
                _transforms[0].position = transform.position;
                return;
            }
            List<Vector3> directions = new List<Vector3>(_transforms.Count - 1);
            for (int i = 1; i < _transforms.Count; i++)
            {
                directions.Add(_transforms[i].position - _transforms[0].position);

            }
            _transforms[0].localPosition = Vector3.zero;
            for (int i = 1; i < _transforms.Count; i++)
            {
                _transforms[i].position = _transforms[0].position + directions[i-1];

            }


        }
        public void CenterAround_L()
        {
            if (_transforms.Count == 0) return;
            if (_transforms.Count == 1)
            {
                _transforms[0].position = transform.position;
                return;
            }
            List<Vector3> directions = new List<Vector3>(_transforms.Count - 1);
            for (int i = 0; i < _transforms.Count-1; i++)
            {
                directions.Add(_transforms[i].position - _transforms[_transforms.Count-1].position);
            }
            _transforms[_transforms.Count - 1].localPosition = Vector3.zero;
            for (int i = 0; i < _transforms.Count-1; i++)
            {
                _transforms[i].position = _transforms[_transforms.Count-1].position + directions[i];

            }
        }


        public void ScaleAllPoints()
        {
            
            foreach(Transform t in _transforms) 
            {
                if(t != null)
                    t.localScale = Vector3.one * DrawSphereSize;
            }
        }

        public void SetDepth()
        {
            foreach(Transform t in _transforms)
            {
                t.localPosition = new Vector3(t.localPosition.x,
                    t.localPosition.y,0);

            }
        }


        public void UpdatePoints()
        {
            if (_transforms == null || _transforms.Count == 0) return;
            foreach(Transform t in _transforms)
            {
                if (t == null)
                    _transforms.Remove(t);
            }
        }

        public void SetNodeNeighbours()
        {
            List<SplineNode> nodes = new List<SplineNode>(_transforms.Count);
            foreach(Transform t in _transforms)
            {
                SplineNode node = t.GetComponent<SplineNode>();
                if (node != null) nodes.Add(node);
            }
            for(int i=0; i< nodes.Count; i++)
            {
                if (i < nodes.Count - 1)
                    nodes[i].ConnectNode(nodes[i + 1]);
                if (i > 0)
                    nodes[i].ConnectNode(nodes[i - 1]);
            }
        }







    }



    [CustomEditor(typeof(PathBranch))]
    public class PathBranchEditor: Editor
    {
        PathBranch me;

        private void OnEnable()
        {
          me  = (PathBranch)target;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PathBranch me = target as PathBranch;
            me.ScaleAllPoints();

            GUILayout.BeginHorizontal();

            GUILayout.Space(10);
            if (GUILayout.Button("AddPoint"))
                me.AddPoint();
            if (GUILayout.Button("SetZeroDepth"))
                me.SetDepth();
            
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("SpaceFrom_F"))
                me.SpaceFromFirst();
            if (GUILayout.Button("SpaceFrom_L"))
                me.SpaceFromLast();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("CenterAround_F"))
                me.CenterAround_F();
            if (GUILayout.Button("CenterAround_L"))
                me.CenterAround_L();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("RenameAll"))
                me.ReName();
            GUILayout.Space(10);
            if (GUILayout.Button("SetNeighbours"))
                me.SetNodeNeighbours();

      
        }


        private void OnSceneGUI()
        {
            me.UpdatePoints();
            me.ScaleAllPoints();
          
            DrawLinks(me._transforms);
        }


        public void DrawLinks(List<Transform> points)
        {
            Handles.color = Color.red;
            for(int i =1; i<points.Count; i++)
            {
                Vector3[] ends = new Vector3[2];
                ends[0] = points[i - 1].position;
                ends[1] = points[i].position;
                Handles.DrawAAPolyLine(10, ends);
            }
        }

    }


}