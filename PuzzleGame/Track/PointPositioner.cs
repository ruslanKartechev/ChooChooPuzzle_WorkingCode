using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PuzzleGame
{
    //public enum PositioningType { angle, direction}
    public enum Direction { up,right,forward}
    public class PointPositioner : MonoBehaviour
    {
        public Transform target;
        [Space(10)]
        public float angle = 0;
        public Direction around = Direction.up;
        public float distance = 1;


        public void SetPosition()
        {
            if (target == null) return;
            Vector3 origin = transform.position;
            Vector3 dir = GetDir();
            Vector3 point = GetPointVector();
            point = Quaternion.AngleAxis(angle, dir) * point;
            target.transform.position = origin + point*distance;

        }
        private Vector3 GetDir()
        {
            Vector3 dir = transform.right;
            switch (around)
            {
                case Direction.right:
                    dir = transform.right;
                    break;
                case Direction.up:
                    dir = transform.up;
                    break;
                case Direction.forward:
                    dir = transform.forward;
                    break;
            }
            return dir;
        }
        private Vector3 GetPointVector()
        {
            Vector3 dir = transform.right;
            switch (around)
            {
                case Direction.right:
                    dir = -transform.forward;
                    break;
                case Direction.up:
                    dir = transform.right;
                    break;
                case Direction.forward:
                    dir = transform.right;
                    break;
            }
            return dir;
        }

        


    }

#if UNITY_EDITOR

    [CustomEditor(typeof(PointPositioner))]
    public class PointPositionerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            PointPositioner me = target as PointPositioner;
            if (GUILayout.Button("SetPosition"))
                me.SetPosition();
        }
    }
#endif

}