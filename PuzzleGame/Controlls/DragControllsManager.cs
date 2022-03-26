using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleGame;
using CommonGame.Controlls;
using CommonGame.UI;

namespace PuzzleGame.Controlls
{
    public class DragControllsManager : ControllsManager
    {
        public LayerMask CastingMask;
        private IMovable currentTarget;
        private Coroutine movingHandler;

        public override void Init(object ui)
        {
            
        }

        public override void EnableControlls()
        {
            base.EnableControlls();
        }
        public override void DisableControlls()
        {
            base.DisableControlls();
        }


        public override void OnClick()
        {
            if (movingHandler != null) StopCoroutine(movingHandler);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 200f,  CastingMask))
            {
                IMovable movable = hit.transform.gameObject.GetComponent<IMovable>();
                currentTarget = movable;
                movable?.OnMoveStart();
                if (currentTarget != null)
                    movingHandler = StartCoroutine(Moving());
              
            }
        }

        public override void OnRelease()
        {
            if (movingHandler != null) StopCoroutine(movingHandler);
            currentTarget?.OnMoveEnd();
        }

        private IEnumerator Moving()
        {
            float sensitivity = 1;
            Vector2 pointerOldPos = Input.mousePosition;
            Vector2 pointerNewPos = new Vector2();
            while (Input.GetMouseButton(0))
            {
                pointerNewPos = Input.mousePosition;
                Vector2 d = (pointerNewPos - pointerOldPos);
                float distance = d.magnitude;
                if (distance >= 1)
                {
                    Vector2 move = d * sensitivity;
                    if (distance < move.magnitude)
                        move = d;
                    currentTarget?.TakeInput(move);
                }
                pointerOldPos = pointerNewPos;
                yield return null;
            }

        }


        protected override IEnumerator InputChecking()
        {
            while (true)
            {

                if (Input.GetMouseButtonDown(0))
                    OnClick();
                else if (Input.GetMouseButtonUp(0))
                    OnRelease();


                yield return null;
            }

        }
    }
}
