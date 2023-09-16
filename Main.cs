using System.Security.AccessControl;
using SML;
using Buttons;
using HarmonyLib;
using Game.Interface;
using Services;
using Game.Decorations;
using UnityEngine;
using Utils;


namespace FavoriteNRandomCosmetics2
{
    [Mod.SalemMod]
    public class Main
    {
        static public DecorationType curDecor = DecorationType.Character;
        public static void Start()
        {
            Debug.Log("Working?");
            PlayerPrefsUtils.Startup();
            OtherUtils.star = EmbedUtils.GetImage("FavoriteNRandomCosmetics2.resources.images.star.png");
        }
    }
}
