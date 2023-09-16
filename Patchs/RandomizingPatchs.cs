using Utils;
using HarmonyLib;
using SML;
using System;
using Game.Interface;
using Server.Shared.State;

namespace Patchs
{
    [HarmonyPatch(typeof(Home.HomeScene.HomeSceneController), "Start")]
    class HomeScene
    {
        static public void Postfix()
        {
            OtherUtils.RandomizeSelections(); Console.WriteLine("Randomizing selections");
        }
    }
    
}