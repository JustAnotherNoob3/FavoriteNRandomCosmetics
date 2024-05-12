using System.Security.Cryptography;
using Utils;
using HarmonyLib;
using SML;
using System;
using Services;
using Server.Shared.State;
using Home.HomeScene;
using Game.Decorations;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Home.Customizations;
using Home.Shared;

namespace Patchs
{
    [HarmonyPatch(typeof(HomeSceneController), "Start")]
    class HomeScene
    {
        static HomeSceneController instance;
        static public void Prefix(HomeSceneController __instance)
        {
            instance = __instance;
            OtherUtils.check = false;
            MapDecoration mapDecoration = Service.Home.Decorations.GetMapDecoration(Service.Home.Customizations.myCustomizationSelections.Data.mapId);
            OtherUtils.RandomizeSelections();
            Console.WriteLine("Randomizing selections");
            MapDecoration mapDecoration2 = Service.Home.Decorations.GetMapDecoration(OtherUtils.pcs.mapId);
            if(!ModSettings.GetBool("Randomize Map In Home", "JAN.favoritenrandom"))
            if(!(PlayerPrefsUtils.GetValue<bool>("RandomizeMap") && ModSettings.GetBool("Just randomize everything.", "JAN.favoritenrandom"))) return;
            __instance.StartCoroutine(SwapMapAsync(mapDecoration.mapSceneName, mapDecoration2.mapSceneName));
        }
        
        static IEnumerator SwapMapAsync(string previousMap, string mapName)
		{
			bool flag = !string.IsNullOrEmpty(previousMap);
			Debug.Log("SwapMapAsync from " + previousMap + " to " + mapName);
			
            var trans = ApplicationController.ApplicationContext.TransitionOverlay;
			if (flag)
			{
                trans.NormalTransitionContainer.SetActive(true);
                trans.TransitionLabel.gameObject.SetActive(true);
                trans.TipText.gameObject.SetActive(true);
                trans.TipText.SetText("Fun Fact! "+OtherUtils.funfacts[UnityEngine.Random.Range(0,OtherUtils.funfacts.Count)]);
				yield return new WaitForSeconds(0.5f);
			}
			IEnumerator enumerator;
			if (!string.IsNullOrEmpty(previousMap))
			{
				Debug.Log("SwapMapAsync cleanly unload " + previousMap);
				enumerator = ApplicationController.ApplicationContext.navigationController.CleanlyUnloadMap(previousMap);
				yield return enumerator;
			}
			Debug.Log("SwapMapAsync load " + mapName);
			enumerator = ApplicationController.ApplicationContext.navigationController.ShowMapSceneAdditive(mapName);
			yield return enumerator;
			yield return new WaitForEndOfFrame();
			                trans.NormalTransitionContainer.SetActive(false);

			yield return new WaitForEndOfFrame();
			yield break;
		}
    }
    
    [HarmonyPatch(typeof(HomeScenePersonalizations), "ValidateCustomizations")]
    class ShowRandomizations
    {
        static public bool Prefix(HomeScenePersonalizations __instance)
        {
            PlayerCustomizationSelections data = OtherUtils.pcs;
            if (data == null) return true;
            __instance.characterWrapper.SwapWithCharacter(data.skinId, true);
            __instance.petWrapper.SwapWithPet(data.petId, true);
            __instance.houseSpot.SetHouseIsSpawned(data.houseId);
            __instance.houseSpot.groundSpot.SetIsSpawned(data.pathwaySelection, data);
            __instance.houseSpot.lawnA.SetIsSpawned(data.lawnDecorationA, Service.Home.Decorations.GetPropDecorationByType(DecorationType.Lawn_DecorationB, data.lawnDecorationA));
            __instance.houseSpot.lawnB.SetIsSpawned(data.lawnDecorationB, Service.Home.Decorations.GetPropDecorationByType(DecorationType.Lawn_DecorationB, data.lawnDecorationB));
            __instance.ForceDeathAnimationUpdate();

            return false;
        }
    }
    /*[HarmonyPatch(typeof(Leo), "GetMapScene")]
    static class RandomizeMap
    {
        static public bool Prefix(ref string __result)
        {
            if (OtherUtils.firstly) return true;
            if (!(PlayerPrefsUtils.GetValue<bool>("RandomizeMap") && ModSettings.GetBool("Just randomize everything.", "JAN.favoritenrandom"))) return true;
            int mapId = OtherUtils.pcs.mapId;
            Debug.Log($"Getting id: {mapId}");
            MapDecoration mapDecoration = Service.Home.Decorations.GetMapDecoration(mapId);
            if (mapDecoration == null)
            {
                __result = string.Empty;
                return false;
            }
            __result = mapDecoration.mapSceneName;
            Debug.Log(mapDecoration.mapSceneName);
            return false;
        }
    }*/
    static class ListExtensions
    {
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