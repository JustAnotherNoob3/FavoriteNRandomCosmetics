using Utils;
using HarmonyLib;
using SML;
using System;
using Services;
using Server.Shared.State;
using Home.HomeScene;
using Game.Decorations;
using System.Collections.Generic;

namespace Patchs
{
    [HarmonyPatch(typeof(HomeSceneController), "Start")]
    class HomeScene
    {
        static public void Prefix()
        {
            OtherUtils.RandomizeSelections(); Console.WriteLine("Randomizing selections");
        }
    }
    [HarmonyPatch(typeof(HomeScenePersonalizations), "ValidateCustomizations")]
    class ShowRandomizations{
        static public bool Prefix(HomeScenePersonalizations __instance){
            PlayerCustomizationSelections data = OtherUtils.pcs;
            if(data == null) return true;
            __instance.characterWrapper.SwapWithCharacter(data.skinId, true);
			__instance.petWrapper.SwapWithPet(data.petId, true);
			__instance.houseSpot.SetHouseIsSpawned(data.houseId);
			__instance.houseSpot.groundSpot.SetIsSpawned(data.pathwaySelection, data);
            __instance.houseSpot.lawnA.SetIsSpawned(data.lawnDecorationA, Service.Game.Decorations.GetLawnADecorations().GetElement(data.lawnDecorationA));
			__instance.houseSpot.lawnB.SetIsSpawned(data.lawnDecorationB, Service.Game.Decorations.GetLawnBDecorations().GetElement(data.lawnDecorationB));
            return false;
        }
        
    }
    static class ListExtensions{
        public static T GetElement<T>(this List<T> list, int index)
		{
			if (index < 0 || index >= list.Count)
			{
				return default(T);
			}
			return list[index];
		}
    }
}