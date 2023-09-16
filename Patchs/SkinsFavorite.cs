using Game.Interface;
using HarmonyLib;
using System.Reflection;
using Services;
using Buttons;
using UnityEngine;
using UnityEngine.UI;
using Utils;
namespace Patchs
{
    [HarmonyPatch(typeof(SkinSelectionItem), "Init")]
    class SkinFavoriteButton
    {
        [HarmonyPostfix]
        public static void AddFavoriteButton(SkinSelectionItem __instance, int skinId)
        {
            RectTransform sourceRectTransform = __instance.transform.GetChild(0).GetComponent<RectTransform>();
            Vector3 topRightCorner = sourceRectTransform.localPosition + new Vector3(sourceRectTransform.rect.width * 0.35f, sourceRectTransform.rect.height * 0.35f, 0f);
            GameObject star = Object.Instantiate(__instance.transform.GetChild(0).gameObject, __instance.transform);
            star.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
            star.transform.localPosition = topRightCorner;
            star.GetComponent<Image>().sprite = OtherUtils.star;
            star.SetActive(false);
            FavoriteObjectBehaviour obj = __instance.gameObject.AddComponent<FavoriteObjectBehaviour>();
            obj.star = star;
            obj.decorationType = "Character";
            obj.decorationId = Service.Game.Cast.GetCharacterIdBySkinId(skinId);
            obj.skinId = skinId;

        }
    }
}