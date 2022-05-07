using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonGame
{
    public class TutorHandAnimator : MonoBehaviour
    {
        [SerializeField] private GameObject _hand;
        [SerializeField] private Animator _anim;
        [SerializeField] private string _animName = "Level_1_Tutor";
        [Space(5)]
        [SerializeField] private GameObject _tutorText;
        [SerializeField] private string _textAnimName = "TextBouncing";
        public void StartTutor()
        {
            _anim.Play(_animName,0);
            _anim.Play(_textAnimName, 1);
            _hand.SetActive(true);
        }
        public void StopTutor()
        {
            _anim.Play("Empty", 0);
            _hand.SetActive(false);
        }
        public void HideText()
        {
            _anim.Play("Empty", 1);
            _tutorText.SetActive(false);

        }
    }
}
