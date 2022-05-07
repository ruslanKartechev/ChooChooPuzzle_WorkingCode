using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonGame.UI
{
    public class StartPageView : PageView
    {
        [Space(10)]
        [SerializeField] private GameObject _sign;
        [SerializeField] private GameObject _hand;
        [SerializeField] private string _handAnimName;

        public override void ShowPage()
        {
            base.ShowPage();
            _sign.SetActive(true);
            _hand.SetActive(true);
        }
        public override void HidePage()
        {
            _sign.SetActive(false);
            _hand.SetActive(false);
            base.HidePage();
        }
    }
}