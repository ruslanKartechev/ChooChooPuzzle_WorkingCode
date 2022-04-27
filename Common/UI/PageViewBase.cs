using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace CommonGame.UI
{
    public class PageViewBase : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TextMeshProUGUI _header;
        [SerializeField] private Animator _animator;
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _buttonText;
        public Action OnButtonClick;

        private void Start()
        {
            _button.onClick.AddListener(OnButton);
        }
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
        private void OnButton()
        {
            OnButtonClick?.Invoke();
        }
    }

}