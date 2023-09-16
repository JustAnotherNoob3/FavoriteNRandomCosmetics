
using UnityEngine;
using TMPro;
using Game.Decorations;
using UnityEngine.UI;
using Home.Common;
using Utils;
using System;
using Home.Common.Tooltips;
namespace Buttons
{
    public class SmartRandomButtonBehaviour : MonoBehaviour
    {
        public DecorationType cat = DecorationType.Character;
        public ToggleCustomClick button;
        public void Start()
        {
            gameObject.GetComponentInChildren<TooltipTrigger>().NonLocalizedString = "Right click any item to favorite them! Randomization will occur only taking into account favorite items, or every item if none are favorites.";
            button = gameObject.GetComponentInChildren<ToggleCustomClick>();
            button.onValueChanged = new Toggle.ToggleEvent();
            GameObject anonReference = transform.parent.GetChild(0).gameObject;
            RectTransform anonTransform = anonReference.GetComponent<RectTransform>();
            transform.localPosition = anonReference.transform.localPosition - new Vector3(0, anonTransform.rect.height - 3.5f, 0f);
            button.gameObject.name = "Randomize Checkbox";
            button = GetComponentInChildren<ToggleCustomClick>();
            UpdateButton(cat);
        }
        public void UpdateButton(DecorationType category)
        {
            if (OtherUtils.Skip(category)) { gameObject.SetActive(false); return; }
            cat = category;
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Randomize " + category.ToString().Replace("_", " ");
            ResetButton();
        }
        public void ResetButton()
        {
            button.OnToggleClicked = null;
            button.isOn = IsRandom();
            button.OnToggleClicked += SetRandom;
        }
        public bool IsRandom()
        {
            return PlayerPrefsUtils.GetValue<bool>("Randomize" + cat.ToString());
        }

        public void SetRandom(ToggleCustomClick toggle)
        {
            Console.WriteLine($"Setting Randomize{cat.ToString()} to {toggle.isOn}");
            PlayerPrefsUtils.SetValue("Randomize" + cat.ToString(), toggle.isOn);
        }
    }
}