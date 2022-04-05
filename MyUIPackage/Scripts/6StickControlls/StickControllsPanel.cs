using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CommonGame.Controlls;
using UnityEngine.EventSystems;
namespace CommonGame.UI
{
    public class StickControllsPanel : PanelManager
    {

        [SerializeField] private StickControllsPanelUI panelUI;
        [SerializeField] private GraphicRaycaster _graphicsRaycaster;
        [SerializeField] private EventSystem _eventSystem;
        private int maxRad = 200;
        public int MaxRad { get { return maxRad; } set { maxRad = value; } }

        // stick drag sensitivity
        private float sensitivity = 1f;
        public float Sensitivity { get { return sensitivity; } set { sensitivity = value; } }
        
        //Radius from the center
        private float currentRad;
        public float CurrentRad { get { return currentRad; } }


        private Coroutine movingStick;
        private GraphicsRaycastHandler _raycastHandler;
        public void Init()
        {
            m_ui = panelUI;
            panelUI.Init(this);
            _raycastHandler = new GraphicsRaycastHandler();
            _raycastHandler.Init(_graphicsRaycaster, _eventSystem);
        }
        
        public Vector2 GetStickPosition()
        {
            return panelUI.stickTransform.localPosition;
        }
        public float GetRad()
        {
            return panelUI.stickTransform.localPosition.magnitude;
        }
        public Vector2 GetDirVector()
        {
            return panelUI.stickTransform.localPosition.normalized;
        }
        public void OnRelease()
        {
            if (movingStick != null) StopCoroutine(movingStick);
            panelUI.SetStickLocalPos(Vector2.zero);
        }

        public InputTypes OnClick()
        {
            if (CheckPointerOverTag(UITags.ControllStick))
                return InputTypes.ControllStick;
            else if (CheckPointerOverTag(UITags.AttackButton))
                return InputTypes.AttackButton;
            else
                return InputTypes.None; 
        }



        #region CheckingPointerOver
        public bool CheckPointerOverTag(string tag)
        {
            List<string> results = _raycastHandler.GraphRaycastTags();
            if (results.Contains(tag))
                return true;
            else
                return false;
        }
        public bool CheckPointerOverName(string name)
        {
            List<string> results = _raycastHandler.GraphRaycastNames();
            if (results.Contains(tag))
                return true;
            else
                return false;
        }
        public bool CheckPointerOverGO(GameObject go)
        {
            List<GameObject> results = _raycastHandler.GraphRaycastGO();
            if (results.Contains(go))
                return true;
            else
                return false;
        }

        #endregion



        #region ControllStickMoving
        public void StartStickMovement()
        {

            OnRelease();
            movingStick = StartCoroutine(StickMovingHandler());
        }

        private void MoveStick(Vector2 move)
        {
            Vector2 localPos = (Vector2)panelUI.stickTransform.localPosition;
            if ((localPos + move).magnitude <= maxRad)
            {
                panelUI.stickTransform.localPosition = (localPos + move);

            }
            else
            {
                panelUI.stickTransform.localPosition = (localPos + move).normalized * MaxRad;
            }
        }

        private IEnumerator StickMovingHandler()
        {
            Vector2 pointerOldPos = panelUI.stickTransform.position;
            Vector2 pointerNewPos = new Vector2();
            while (true)
            {
                pointerOldPos = panelUI.stickTransform.position;
                pointerNewPos = Input.mousePosition;
                Vector2 d = (pointerNewPos - pointerOldPos);
                // depricated
                //Vector2 delta = (pointerNewPos - pointerOldPos).normalized;
                float distance = d.magnitude;
                if (distance >= 1)
                {
                    Vector2 move = d * sensitivity;
                    if (distance < move.magnitude)
                        move = d;
                    MoveStick(move);
                }

                yield return null;
            }
        }
        #endregion

        private void OnDisable()
        {
            OnRelease();
            StopAllCoroutines();
        }
    }
}