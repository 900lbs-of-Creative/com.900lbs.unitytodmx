using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using NineHundredLbs.UnitytoDMX.LED;

namespace NineHundredLbs.UnitytoDMX.LED.Examples.SingleLED
{
    public class DemoManager : MonoBehaviour
    {
        [Header("Object References")]
        [SerializeField] private LEDDMXController ledDMXController = default;
        [SerializeField] private Button clearButton = default;
        [SerializeField] private List<LEDEffectButton> ledEffectButtons = new List<LEDEffectButton>();
        [SerializeField] private LEDDMXEmulator ledDMXEmulator = default;

        private void OnEnable()
        {
            ledDMXController.CommandSent = (bytes) =>
            {
                ledDMXEmulator.UpdateEmulation(new ArraySegment<byte>(bytes, 18, 512));
            };

            ledEffectButtons.ForEach(ledEffectButton => ledEffectButton.Clicked = (effect) =>
            {
                ledDMXController.SetProperties(effect);
            });

            clearButton.onClick.AddListener(() =>
            {
                ledDMXController.Clear();
            });
        }

        private void OnDisable()
        {
            ledDMXController.CommandSent = null;
            ledEffectButtons.ForEach(ledEffectButton => ledEffectButton.Clicked = null);
            clearButton.onClick.RemoveListener(() =>
            {
                ledDMXController.Clear();
            });
        }
    }
}