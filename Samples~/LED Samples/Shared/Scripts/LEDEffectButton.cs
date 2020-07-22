using System;
using UnityEngine;
using UnityEngine.UI;

namespace NineHundredLbs.UnitytoDMX.LED.Examples
{
    public class LEDEffectButton : MonoBehaviour
    {
        public Action<LEDEffect> Clicked { get; set; }

        [SerializeField] private LEDEffect effect = default;
        [SerializeField] private Button button = default;

        private void OnEnable()
        {
            button.onClick.AddListener(Button_OnClick);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(Button_OnClick);
        }

        private void Button_OnClick()
        {
            Clicked?.Invoke(effect);
        }
    }
}
