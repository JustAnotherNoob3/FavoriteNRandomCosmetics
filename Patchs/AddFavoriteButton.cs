using HarmonyLib;
using Buttons;
using UnityEngine;
using Game.Interface;
using Game.Decorations;
using Utils;
using UnityEngine.UI;
using FavoriteNRandomCosmetics2;

namespace Patchs
{
    [HarmonyPatch(typeof(DecorationSelectionItem), "Init")]
    class DecorationFavoriteButton
    {
        [HarmonyPrefix]
        public static void AddFavoriteButton(DecorationSelectionItem __instance, DecorationType decorationType, int decorId)
        {
            if (OtherUtils.Skip(decorationType)) return;
            if (decorId < 0) return;
            if (OtherUtils.HasSkins(decorationType, decorId)) return;
            GameObject scrollReference = __instance.transform.GetChild(3).gameObject;
            scrollReference.transform.GetChild(0).gameObject.SetActive(false);
            Image image = scrollReference.GetComponent<Image>();
            image.sprite = OtherUtils.star;
            image.type = Image.Type.Simple;
            image.material = null;
            image.color = Color.white;
            scrollReference.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
            FavoriteObjectBehaviour favoriteObjectBehaviour = __instance.gameObject.AddComponent<FavoriteObjectBehaviour>();
            favoriteObjectBehaviour.star = scrollReference;
            favoriteObjectBehaviour.decorationType = decorationType.ToString();
            favoriteObjectBehaviour.decorationId = decorId;
            favoriteObjectBehaviour.skinId = decorId;
        }
    }
}