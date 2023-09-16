using Utils;
using HarmonyLib;
using SML;
using System;
using Game.Interface;
using Server.Shared.State;

namespace Patchs.Randomizing
{
    [HarmonyPatch(typeof(PickNamesPanel), "Start")]
    class InitGame
    {
        static public void Prefix()
        {
            if (ModSettings.GetBool("Randomize in-between games")) { OtherUtils.RandomizeSkinsNPet(); Console.WriteLine("Randomizing selections"); }
        }
    }
    [HarmonyPatch(typeof(Home.HomeScene.HomeSceneController), "Start")]
    class HomeScene
    {
        static public void Postfix()
        {
            OtherUtils.RandomizeSelections(); Console.WriteLine("Randomizing selections");
        }
    }
    
}