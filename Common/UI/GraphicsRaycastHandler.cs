using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace CommonGame.UI
{
    public class GraphicsRaycastHandler
    {

        public GraphicRaycaster m_Raycaster;
        public PointerEventData m_PointerEventData;
        public EventSystem m_EventSystem;

        public void Init(GraphicRaycaster raycaster, EventSystem eventSystem)
        {
            m_Raycaster = raycaster;
            m_EventSystem = eventSystem;
        }

        public List<RaycastResult> GraphRaycast()
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();

            m_Raycaster.Raycast(m_PointerEventData, results);
            return results;
        }
        public List<GameObject> GraphRaycastGO()
        {
            List<RaycastResult> results = GraphRaycast();
            if (results == null) return null;
            List<GameObject> go = new List<GameObject>(results.Count);
            foreach (RaycastResult res in results)
            {
                go.Add(res.gameObject);
            }
            return go;
        }

        public List<string> GraphRaycastTags()
        {
            List<RaycastResult> results = GraphRaycast();
            if (results == null) return null;
            List<string> Tags = new List<string>(results.Count);
            foreach(RaycastResult res in  results)
            {
                Tags.Add(res.gameObject.tag);
            }
            return Tags;
        }

        public List<string> GraphRaycastNames()
        {
            List<RaycastResult> results = GraphRaycast();
            if (results == null) return null;
            List<string> Names = new List<string>(results.Count);
            foreach (RaycastResult res in results)
            {
                Names.Add(res.gameObject.name);
            }
            return Names;
        }



    }
}