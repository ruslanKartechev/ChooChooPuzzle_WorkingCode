using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace CommonGame.UI
{
    public class PageView : MonoBehaviour
    {
        [SerializeField] protected Canvas _canvas;
        [SerializeField] protected TextMeshProUGUI _header;
        [SerializeField] protected Animator _animator;

        public virtual void ShowPage()
        {
            _canvas.enabled = true;
            _animator.enabled = true;
        }
        public virtual void HidePage()
        {
            _canvas.enabled = false;
            _animator.enabled = false;
        }

        public void SetHeader(string text)
        {
            _header.text = text;
        } 

    }

}