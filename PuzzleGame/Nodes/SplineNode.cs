using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
namespace PuzzleGame
{

    public class SplineNode : MonoBehaviour
    {
        [SerializeField] private bool HasAngleConstr;
        [SerializeField] private AngleConstraint AngleConst;
        public List<SplineNode> linkedNodes;
        private List<IConstrained> mConstraints = new List<IConstrained>();



        public Vector3 _position { get { return transform.position; } }
        public bool IsOccupied
        {
            get
            {
                if (currentFollower == null)
                    return false;
                else
                    return true;
            }
        }
        public ISplineFollower currentFollower { get; set; }
        public List<IConstrained> _Constraints { get { return mConstraints; } }


        private void Start()
        {
            AddBaseConstr();
            if (HasAngleConstr)
                AddAngleConstr();
        }

        #region Constraints
        private void AddBaseConstr()
        {
            BaseConstraint bc = new BaseConstraint();
            mConstraints.Add(bc);
        }
        private void AddAngleConstr()
        {
            mConstraints.Add(AngleConst);
        }

        #endregion




        public void ConnectNode(SplineNode node)
        {
            if(linkedNodes.Contains(node) == false && node != this)
                linkedNodes.Add(node);
        }

        public bool PushFromNode(Vector2 dir)
        {
            bool allow = true;
            if(currentFollower != null)
            {
                allow = currentFollower.PushFromNode(dir);
            }
            return allow;
         
        }
        public bool SetCurrentFollower(ISplineFollower follower) 
        {
            if(currentFollower == null)
            {
                currentFollower = follower;
                return true;
            }
            return false;
        }
        public void SetCurrentFollowerForce(ISplineFollower follower) => currentFollower = follower;

        public void ReleaseNode()
        {
            currentFollower = null;
        }






    }


#if UNITY_EDITOR
    [CustomEditor(typeof(SplineNode))]
    public class CustomSplineNodeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SplineNode me = target as SplineNode;
            
            DrawLinks(me);
            GUILayout.BeginHorizontal();
    
            GUILayout.EndHorizontal();

            serializedObject.Update();
            ConstraintEditorList.Show(serializedObject.FindProperty("_Constraints"));
            serializedObject.ApplyModifiedProperties();

        }

        private void OnSceneGUI()
        {
            SplineNode me = target as SplineNode;
            DrawLinks(me);
        }

        public void DrawLinks(SplineNode me )
        {
            Handles.color = Color.red;
            if (me.linkedNodes.Count < 1) return;
            for (int i = 0; i < me.linkedNodes.Count; i++)
            {
                Vector3[] ends = new Vector3[2];
                ends[0] = me.transform.position;
                ends[1] = me.linkedNodes[i].transform.position;
                Handles.DrawAAPolyLine(20, ends);
            }
        }
    }




    public static class ConstraintEditorList
    {
        public static void Show(SerializedProperty list)
        {
            if (list == null) return;
            EditorGUILayout.PropertyField(list);
            for(int i =0; i<list.arraySize; i++)
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            }
            EditorGUI.indentLevel -= 1;
        }
    }


#endif
}
