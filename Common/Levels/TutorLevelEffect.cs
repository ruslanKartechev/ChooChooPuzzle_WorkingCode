using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonGame.Events;
namespace CommonGame
{
    public class TutorLevelEffect : MonoBehaviour
    {
        [SerializeField] private LevelStartChannelSO _startLevelChannel;
        [SerializeField] private LevelFinishChannelSO _levelFinishChannel;
        private TutorHandAnimator _anim;

        private Coroutine _actionCheck;
        private void Awake()
        {
            Input.simulateMouseWithTouches = true;
            _anim = FindObjectOfType<TutorHandAnimator>();
            if(_anim == null)
            {
                Debug.Log("Tutor animator no main canvas not found");
                return;
            }
            _startLevelChannel.OnLevelStarted.AddListener( ShowTutor);
        }

        public void ShowTutor()
        {
            _startLevelChannel.OnLevelStarted.RemoveListener(ShowTutor);
            _anim.StartTutor();
            _actionCheck = StartCoroutine(ActionChecking());
        }

        public void OnClick()
        {
            _anim.HideText();
            if (_actionCheck != null)
                StopCoroutine(_actionCheck);
        }

        private IEnumerator ActionChecking()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _anim.StopTutor();
                    _actionCheck = null;
                    yield break;
                   
                }
                yield return null;
            }
        }


        
    }
}