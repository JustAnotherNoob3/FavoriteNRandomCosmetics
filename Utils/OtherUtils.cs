using System.ComponentModel.Design;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using Game.Characters;
using Game.Decorations;
using Home.Shared.Enums;
using Server.Shared.State;
using UnityEngine;
using Services;
using System.Linq;
using Server.Shared.Messages;
using SML;
using Server.Shared.Info;
using System.Reflection;

namespace Utils
{
    class OtherUtils
    {
        static public PlayerCustomizationSelections pcs = null;
        static public Sprite star = null;
        static public GameObject Random;
        static public bool first = false;
        static public bool check = false;
        static public bool firstly = true;
        public static void RandomizeSelections()
        {
            bool randomize = ModSettings.GetBool("Just randomize everything.", "JAN.favoritenrandom");
            bool rc = PlayerPrefsUtils.GetValue<bool>("RandomizeCharacter");
            int skinId = 0;
            int charId = 0;
            if (rc || randomize)
            {
                System.Random r = new();
                string[] favorites = PlayerPrefsUtils.GetValue<string>("FavoriteCharacter").Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                if (favorites.Length > 0 && !ModSettings.GetBool("Do not use favorites"))
                {
                    string[] pair = favorites[r.Next(0, favorites.Length)].Split('/');
                    charId = Convert.ToInt16(pair[0]);
                    skinId = Convert.ToInt16(pair[1]);
                }
                else
                {
                    List<int> i = Service.Home.UserService.Inventory.GetItems(ItemTypeCategory.Character).Keys.ToList();
                    charId = i[r.Next(0, i.Count)];

                    List<CharacterAsset> skins = Service.Game.Cast.GetSkinsForCharacter(charId);
                    if (skins.Count > 1)
                    {
                        using List<CharacterAsset>.Enumerator enumerator = skins.GetEnumerator();
                        List<int> ints = new();
                        while (enumerator.MoveNext())
                        {
                            CharacterAsset characterModel = enumerator.Current;

                            if (Service.Home.UserService.Inventory.OwnsItem(ItemTypeCategory.CharacterSkin, characterModel.skinId))
                            {
                                ints.Add(characterModel.skinId);
                            }
                        }
                        skinId = ints[r.Next(0, ints.Count)];
                    }
                    else
                    {
                        skinId = charId;
                    }
                }
            }

            PlayerCustomizationSelections selections = new()
            {
                playerPosition       = 2,
                characterId          = rc ? charId : Service.Home.UserService.UserInfo.CharacterSelection,
                skinId               = rc ? skinId : Service.Home.UserService.UserInfo.CharacterSkinSelection,
                petId                = PlayerPrefsUtils.GetValue<bool>("RandomizePet") || randomize ? GetRandomId(DecorationType.Pet) : Service.Home.UserService.UserInfo.PetSelection,
                houseId              = PlayerPrefsUtils.GetValue<bool>("RandomizeHouse") || randomize ? GetRandomId(DecorationType.House) : Service.Home.UserService.UserInfo.HouseSelection,
                pathwaySelection     = PlayerPrefsUtils.GetValue<bool>("RandomizePathway") || randomize ? GetRandomId(DecorationType.Pathway) : Service.Home.UserService.UserInfo.PathwaySelection,
                wallSelection        = PlayerPrefsUtils.GetValue<bool>("RandomizeWallpaper") || randomize ? GetRandomId(DecorationType.Wallpaper) : Service.Home.UserService.UserInfo.WallpaperSelection,
                trimSelection        = PlayerPrefsUtils.GetValue<bool>("RandomizeTrim") || randomize ? GetRandomId(DecorationType.Trim) : Service.Home.UserService.UserInfo.TrimSelection,
                floorSelection       = PlayerPrefsUtils.GetValue<bool>("RandomizeFloor") || randomize ? GetRandomId(DecorationType.Floor) : Service.Home.UserService.UserInfo.FlooringSelection,
                lawnDecorationA      = PlayerPrefsUtils.GetValue<bool>("RandomizeLawn_DecorationA") || randomize ? GetRandomId(DecorationType.Lawn_DecorationA) : Service.Home.UserService.UserInfo.LawnDecorationASelection,
                lawnDecorationB      = PlayerPrefsUtils.GetValue<bool>("RandomizeLawn_DecorationB") || randomize ? GetRandomId(DecorationType.Lawn_DecorationB) : Service.Home.UserService.UserInfo.LawnDecorationBSelection,
                wallDecorationA      = PlayerPrefsUtils.GetValue<bool>("RandomizeWall_DecorationA") || randomize ? GetRandomId(DecorationType.Wall_DecorationA) : Service.Home.UserService.UserInfo.WallDecorationASelection,
                wallDecorationB      = PlayerPrefsUtils.GetValue<bool>("RandomizeWall_DecorationB") || randomize ? GetRandomId(DecorationType.Wall_DecorationB) : Service.Home.UserService.UserInfo.WallDecorationBSelection,
                wallDecorationC      = PlayerPrefsUtils.GetValue<bool>("RandomizeWall_DecorationC") || randomize ? GetRandomId(DecorationType.Wall_DecorationC) : Service.Home.UserService.UserInfo.WallDecorationCSelection,
                floorDecoration      = PlayerPrefsUtils.GetValue<bool>("RandomizeFloor_Decoration") || randomize ? GetRandomId(DecorationType.Floor_Decoration) : Service.Home.UserService.UserInfo.FloorDecorationSelection,
                cornerA              = PlayerPrefsUtils.GetValue<bool>("RandomizeCornerA") || randomize ? GetRandomId(DecorationType.CornerA) : Service.Home.UserService.UserInfo.CornerDecorationASelection,
                cornerB              = PlayerPrefsUtils.GetValue<bool>("RandomizeCornerB") || randomize ? GetRandomId(DecorationType.CornerB) : Service.Home.UserService.UserInfo.CornerDecorationBSelection,
                cornerC              = PlayerPrefsUtils.GetValue<bool>("RandomizeCornerC") || randomize ? GetRandomId(DecorationType.CornerC) : Service.Home.UserService.UserInfo.CornerDecorationCSelection,
                cornerD              = PlayerPrefsUtils.GetValue<bool>("RandomizeCornerD") || randomize ? GetRandomId(DecorationType.CornerD) : Service.Home.UserService.UserInfo.CornerDecorationDSelection,
                deathAnimation       = PlayerPrefsUtils.GetValue<bool>("RandomizeDeathAnimation") ? GetRandomId(DecorationType.DeathAnimation) : Service.Home.UserService.UserInfo.DeathAnimationSelection,
                mapId                = PlayerPrefsUtils.GetValue<bool>("RandomizeMap") || randomize ? GetRandomId(DecorationType.Map) : Service.Home.UserService.UserInfo.MapSelection,
                trinketRoleSelection = Service.Home.UserService.UserInfo.TrinketRoleSelection,
                streamerMode         = Service.Home.UserService.UserInfo.StreamerMode,
                increaseScrollEquip1 = Service.Home.UserService.UserInfo.IncreaseScrollEquip1,
                increaseScrollEquip2 = Service.Home.UserService.UserInfo.IncreaseScrollEquip2,
                increaseScrollEquip3 = Service.Home.UserService.UserInfo.IncreaseScrollEquip3,
                increaseScrollEquip4 = Service.Home.UserService.UserInfo.IncreaseScrollEquip4,
                increaseScrollEquip5 = Service.Home.UserService.UserInfo.IncreaseScrollEquip5,
                decreaseScrollEquip1 = Service.Home.UserService.UserInfo.DecreaseScrollEquip1,
                decreaseScrollEquip2 = Service.Home.UserService.UserInfo.DecreaseScrollEquip2,
                decreaseScrollEquip3 = Service.Home.UserService.UserInfo.DecreaseScrollEquip3,
                decreaseScrollEquip4 = Service.Home.UserService.UserInfo.DecreaseScrollEquip4,
                decreaseScrollEquip5 = Service.Home.UserService.UserInfo.DecreaseScrollEquip5
            };
            pcs = selections;
            Service.Home.UserService.SendCustomizationSettings(selections);
            if(firstly) firstly = false; 
        }
        public static void RandomizeSkinsNPet()
        {
            bool randomize = ModSettings.GetBool("Just randomize everything.");
            bool rc = PlayerPrefsUtils.GetValue<bool>("RandomizeCharacter");
            int skinId = Service.Home.UserService.UserInfo.CharacterSkinSelection;
            int charId = Service.Home.UserService.UserInfo.CharacterSelection;
            if (rc || randomize)
            {
                System.Random r = new();
                string[] favorites = PlayerPrefsUtils.GetValue<string>("FavoriteCharacter").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (favorites.Length > 0)
                {
                    if (ModSettings.GetBool("Only the skins i want!"))
                    {
                        HashSet<int> chars = GetUsedSkins();
                        List<int> availableSkins = new();
                        foreach (string favorite in favorites)
                        {
                            string[] pair = favorite.Split('/');
                            charId = Convert.ToInt16(pair[0]);
                            int skinIdTemp = Convert.ToInt16(pair[1]);
                            if (chars.Contains(charId)) continue;
                            availableSkins.Add(skinIdTemp);
                        }
                        if (availableSkins.Count > 0)
                        {
                            skinId = availableSkins[r.Next(0, availableSkins.Count)];
                            int petIds = PlayerPrefsUtils.GetValue<bool>("RandomizePet") || randomize ? GetRandomId(DecorationType.Pet) : Service.Home.UserService.UserInfo.PetSelection;
                            Service.Home.Customizations.myCustomizationSelections.Data.playerPosition = Pepper.GetMyPosition();
                            Service.Home.Customizations.myCustomizationSelections.Data.skinId = skinId;
                            Service.Home.Customizations.myCustomizationSelections.Data.characterId = charId;
                            Service.Game.Network.Send(new PlayerCustomizationsMessage(skinId, petIds));
                            return;
                        }
                    }
                    else if (!ModSettings.GetBool("Do not use favorites"))
                    {
                        string[] pair = favorites[r.Next(0, favorites.Length)].Split('/');
                        skinId = Convert.ToInt16(pair[1]);
                        int petIds = PlayerPrefsUtils.GetValue<bool>("RandomizePet") || randomize ? GetRandomId(DecorationType.Pet) : Service.Home.UserService.UserInfo.PetSelection;
                        Service.Home.Customizations.myCustomizationSelections.Data.playerPosition = Pepper.GetMyPosition();
                        Service.Home.Customizations.myCustomizationSelections.Data.skinId = skinId;
                        Service.Home.Customizations.myCustomizationSelections.Data.characterId = charId;
                        Service.Game.Network.Send(new PlayerCustomizationsMessage(skinId, petIds));
                        return;
                    }

                }
                List<int> i = Service.Home.UserService.Inventory.GetItems(ItemTypeCategory.Character).Keys.ToList();
                charId = i[r.Next(0, i.Count)];

                List<CharacterAsset> skins = Service.Game.Cast.GetSkinsForCharacter(charId);
                if (skins.Count > 1)
                {
                    using List<CharacterAsset>.Enumerator enumerator = skins.GetEnumerator();
                    List<int> ints = new();
                    while (enumerator.MoveNext())
                    {
                        CharacterAsset characterModel = enumerator.Current;

                        if (Service.Home.UserService.Inventory.OwnsItem(ItemTypeCategory.CharacterSkin, characterModel.skinId))
                        {
                            ints.Add(characterModel.skinId);
                        }
                    }
                    skinId = ints[r.Next(0, ints.Count)];
                }
                else
                {
                    skinId = charId;
                }
            }
            int petId = PlayerPrefsUtils.GetValue<bool>("RandomizePet") || randomize ? GetRandomId(DecorationType.Pet) : Service.Home.UserService.UserInfo.PetSelection;
            Service.Home.Customizations.myCustomizationSelections.Data.playerPosition = Pepper.GetMyPosition();
            Service.Home.Customizations.myCustomizationSelections.Data.skinId = skinId;
            Service.Home.Customizations.myCustomizationSelections.Data.characterId = charId;
            Service.Game.Network.Send(new PlayerCustomizationsMessage(skinId, petId));
        }
        public static HashSet<int> GetUsedSkins()
        {
            HashSet<int> usedChars = new();
            List<PlayerCustomizationSelectionsObservation> playerCustomizations = Service.Game.Sim.info.playerCustomizationSelectionsObservations;
            Service.Game.Sim.info.discussionPlayers.ForEach(delegate (DiscussionPlayerObservation playerObservation)
            {
                if (playerObservation.Data.valid)
                {
                    if (playerObservation.Data.position != Pepper.GetMyPosition())
                    {
                        int skinId = playerCustomizations[playerObservation.Data.position].Data.skinId;
                        int characterIdBySkinId = Service.Game.Cast.GetCharacterIdBySkinId(skinId);
                        usedChars.Add(characterIdBySkinId);
                    }
                }
            });
            return usedChars;
        }
        public static int GetRandomId(DecorationType type)
        {
            System.Random r = new();
            string[] favorites = PlayerPrefsUtils.GetValue<string>("Favorite" + type.ToString()).Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (favorites.Length > 0 && !ModSettings.GetBool("Do not use favorites"))
            {
                return Convert.ToInt16(favorites[r.Next(0, favorites.Length)]);
            }
            else
            {
                Dictionary<int, int> objs = GetItems(type);
                if (objs == null) return 0;
                List<int> i = objs.Keys.ToList();
                if (i.Count > 0)
                {
                    return i[r.Next(0, i.Count)];
                }
            }
            return 0;
        }

        private static Dictionary<int, int> GetItems(DecorationType type)
        {
            return type switch
            {
                DecorationType.Floor => Service.Home.UserService.Inventory.GetItems(ItemTypeCategory.Flooring),
                DecorationType.CornerA or DecorationType.CornerB or DecorationType.CornerC or DecorationType.CornerD => Service.Home.UserService.Inventory.GetItems(ItemTypeCategory.CornerDecoration),
                DecorationType.Wall_DecorationA or DecorationType.Wall_DecorationB or DecorationType.Wall_DecorationC => Service.Home.UserService.Inventory.GetItems(ItemTypeCategory.WallDecoration),
                DecorationType.Lawn_DecorationA or DecorationType.Lawn_DecorationB => Service.Home.UserService.Inventory.GetItems(ItemTypeCategory.LawnDecoration),
                DecorationType.Floor_Decoration => Service.Home.UserService.Inventory.GetItems(ItemTypeCategory.FloorDecoration),
                _ => Service.Home.UserService.Inventory.GetItems((ItemTypeCategory)Enum.Parse(typeof(ItemTypeCategory), type.ToString())),
            };
        }
        public static bool HasSkins(DecorationType decorationType, int decorId)
        {
            if (decorationType == DecorationType.Character)
            {
                List<CharacterAsset> skins = Service.Game.Cast.GetSkinsForCharacter(decorId);
                if (skins.Count > 1)
                {
                    using List<CharacterAsset>.Enumerator enumerator = skins.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        CharacterAsset characterModel = enumerator.Current;
                        if (Service.Home.UserService.Inventory.OwnsItem(ItemTypeCategory.CharacterSkin, characterModel.skinId) && !characterModel.isBaseSkin)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private static List<DecorationType> SkippedItems = [
            DecorationType.DecreaseScroll1,
                DecorationType.DecreaseScroll2,
                DecorationType.DecreaseScroll3,
                DecorationType.IncreaseScroll4,
                DecorationType.IncreaseScroll5,
                DecorationType.DecreaseScroll4,
                DecorationType.DecreaseScroll5,
                DecorationType.IncreaseScroll1,
                DecorationType.IncreaseScroll2,
                DecorationType.IncreaseScroll3,
                DecorationType.Trinket,
                DecorationType.None,
        ];
        public static bool Skip(DecorationType type)
        {
            return !SkippedItems.Contains(type);
        }
        public static List<string> funfacts = [
            "I am watching you.",
            @"
|   ||
||   | _",
            "Survivor is the best role ever.",
            "Google En Passant",
            "HOLY MOLY",
            "*Insert potion master copypasta here*",
            @"Sure thing! Here we go:

I told my wife she was drawing her eyebrows too high. She looked surprised.
Why don't skeletons fight each other? They don't have the guts.
Parallel lines have so much in common. It's a shame they'll never meet.
Why did the scarecrow win an award? Because he was outstanding in his field.
I used to be a baker, but I couldn't make enough dough.
I told my computer I needed a break, but it didn't seem to understand. So, I took a Ctrl break.
Why don't scientists trust atoms? Because they make up everything.
I'm reading a book on anti-gravity. It's impossible to put down.
I told my wife she should embrace her mistakes. She gave me a hug.
My dog used to chase people on a bike a lot. It got so bad, I had to take his bike away.
Why don't some couples go to the gym? Because some relationships don't work out.
Why was the math book sad? Because it had too many problems.

Hope those brought a smile to your face!",
        "March 20th 2027.",
        "GET BACK TO WORK",
        "JustAnotherNoob is denying requests, they can't do that! In the mod user's handbook it says they must add any good suggestions. JustAnotherNoob knows this and they aren't listening!",
        "Mario will take your livers in 3 days.",
        "EXPLOSION!"
        ];
    }
    
}