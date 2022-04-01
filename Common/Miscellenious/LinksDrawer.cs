using UnityEngine;
using UnityEditor;
using System.Linq;
namespace PuzzleGame
{
    public static class LinksDrawer
    {
        public static void DrawLinks(SplineNode me)
        {
            Handles.color = Color.red;
            if (me.linkedNodes == null) return;
            if (me.linkedNodes.Count < 1) return;
            for (int i = 0; i < me.linkedNodes.Count; i++)
            {
                if(me.linkedNodes[i] == null)
                {
                    me.linkedNodes.RemoveAt(i);
                    continue;
                }
                Vector3[] ends = new Vector3[2];
                ends[0] = me.transform.position;
                ends[1] = me.linkedNodes[i].transform.position;
                Handles.DrawAAPolyLine(20, ends);
            }
        }
    }
}
