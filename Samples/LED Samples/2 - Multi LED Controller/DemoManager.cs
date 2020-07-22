using System;
using System.Collections.Generic;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED.Examples.MultiLED
{
    public class DemoManager : MonoBehaviour
    {
        [Header("Object References")]
        [SerializeField] private MultiLEDDMXController multiLEDDMXController = default;
        [SerializeField] private List<LEDDMXController> ledDMXControllers = default;
        [SerializeField] private List<UILEDDMXEmulator> uiLEDDMXEmulators = new List<UILEDDMXEmulator>();
        [SerializeField] private List<LEDEffectButton> ledEffectButtons = new List<LEDEffectButton>();

        private void OnEnable()
        {
            ledEffectButtons.ForEach(ledEffectButton => ledEffectButton.Clicked = (effect) =>
            {
                if (effect != null)
                    multiLEDDMXController.SetProperties(effect);
                else
                    multiLEDDMXController.Clear();
            });

            ledDMXControllers.ForEach(ledDMXController => ledDMXController.CommandSent = (command) =>
            {
                uiLEDDMXEmulators.Find(ledDMXEmulator => ledDMXEmulator.Universe == command[14]).UpdateEmulation(new ArraySegment<byte>(command, 18, 512));
            });
        }

        private void OnDisable()
        {
            ledEffectButtons.ForEach(ledEffectButton => ledEffectButton.Clicked = null);

            foreach (var ledDMXController in ledDMXControllers)
                ledDMXController.CommandSent = null;
        }
    }
}
