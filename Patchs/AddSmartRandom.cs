using Game.Interface;
using HarmonyLib;
using UnityEngine;
using Buttons;
using Utils;
using Home.Shared.Enums;
using Game.Decorations;
namespace Patchs
{
    [HarmonyPatch(typeof(DecorationsPanel), "Start")]
    class AddSmartRandom
    {
        [HarmonyPrefix]
        public static void AddButton(DecorationsPanel __instance)
        {
            OtherUtils.first = false;
            GameObject anonReference = __instance.transform.GetChild(0).gameObject;
            GameObject SmartRandomSelection = Object.Instantiate(anonReference, __instance.transform);
            OtherUtils.Random = SmartRandomSelection;
            SmartRandomSelection.AddComponent<SmartRandomButtonBehaviour>();
        }
    }
    [HarmonyPatch(typeof(DecorationsPanel), "PopulateDecorationList")]
    class DecorationsPanelPatch
    {

        [HarmonyPrefix]
        public static void PopulateDecorationList(DecorationType decorType)
        {
            if (!OtherUtils.first) { OtherUtils.first = true; return; }
            OtherUtils.Random.SetActiveIfNot();
            OtherUtils.Random.GetComponent<SmartRandomButtonBehaviour>().UpdateButton(decorType);
        }
    }
}