using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PuzzleGame
{
    public interface ISplineFollower
    {
        Transform GetTransform();
        bool PushFromNode(Vector2 dir);
    }



    public class SplineNode : MonoBehaviour
    {
        public List<SplineNode> linkedNodes;
        public Vector3 _position { get { return transform.position; } }
        public bool IsOccupied { get { 
                if (currentFollower == null) 
                    return false; 
                else 
                    return true; } }
        public ISplineFollower currentFollower { get; set; }


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
            Debug.Log("already occupied");
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

#endif
}
