using HarmonyLib;
using Buttons;
using UnityEngine;
using Game.Interface;
using Game.Decorations;
using Utils;
using SML;
using Services;
using System.Collections.Generic;
using Server.Shared.Info;
using System.Linq;
using Server.Shared.Extensions;
using System;
using Game.Characters;
using Home.Shared.Enums;
using Server.Shared.Messages;

namespace Patchs
{
    [HarmonyPatch(typeof(RoleDeckPanelController), "Start")]
    class AlwaysSelectFavorite
    {
        [HarmonyPostfix]
        public static void ChangeIfNotFavorite()
        {
            if (ModSettings.GetBool("Randomize in-between games")) { OtherUtils.RandomizeSkinsNPet(); Console.WriteLine("Randomizing selections"); return;}
            if (!ModSettings.GetBool("Only the skins i want!")) return;
            string[] skinsString = PlayerPrefsUtils.GetValue<string>("FavoriteCharacter").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (skinsString.Length == 0) return;
            List<KeyValuePair<int, int>> skins = new();
            skinsString.ForEach(
                delegate (string favorites)
                {
                    string[] pair = favorites.Split('/');
                    int charId = Convert.ToInt16(pair[0]);
                    int skinId = Convert.ToInt16(pair[1]);
                    skins.Add(new KeyValuePair<int, int>(charId, skinId));
                }
            );
            int curId = Pepper.GetSkinIdByPosition(Pepper.GetMyPosition());
            if (curId == Service.Home.UserService.UserInfo.CharacterSkinSelection) return;
            if (skins.Contains(new KeyValuePair<int, int>(Service.Game.Cast.GetCharacterIdBySkinId(curId), curId))) return;
            List<PlayerCustomizationSelectionsObservation> playerCustomizations = Service.Game.Sim.info.playerCustomizationSelectionsObservations;
            HashSet<int> usedChars = new();
            Service.Game.Sim.info.discussionPlayers.ForEach(delegate (DiscussionPlayerObservation playerObservation)
            {
                if (playerObservation.Data.valid)
                {
                    int skinId = playerCustomizations[playerObservation.Data.position].Data.skinId;
                    int charId = Service.Game.Cast.GetCharacterIdBySkinId(skinId);
                    usedChars.Add(charId);
                }
            });
            foreach (KeyValuePair<int, int> favorite in skins)
            {
                if (!usedChars.Contains(favorite.Key))
                {
                    int petId = Service.Home.UserService.UserInfo.PetSelection;
                    int myPosition = Pepper.GetMyPosition();
                    Service.Home.Customizations.myCustomizationSelections.Data.playerPosition = myPosition;
                    Service.Home.Customizations.myCustomizationSelections.Data.skinId = favorite.Value;
                    Service.Home.Customizations.myCustomizationSelections.Data.characterId = Service.Game.Cast.GetCharacterIdBySkinId(favorite.Value);
                    Service.Game.Network.Send(new PlayerCustomizationsMessage(favorite.Value, petId));
                    return;
                }
            }
            
        }


    }
}