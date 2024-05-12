
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;
namespace Buttons
{
    public class FavoriteObjectBehaviour : MonoBehaviour, IPointerClickHandler
    {
        public bool isFavorite;
        public int decorationId;
        public int skinId;
        public GameObject star;
        public string decorationType;
        public void Start()
        {
            isFavorite = IsFavorite();
            star.SetActive(isFavorite);
        }
        public void OnPointerClick(PointerEventData eventData)
        {

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                isFavorite = !isFavorite;
                star.SetActive(isFavorite);
                if (isFavorite) AddToFavoriteList();
                else RemoveFromFavoriteList();
            }

        }
        void AddToFavoriteList()
        {
            if (decorationType == "Character")
                Utils.PlayerPrefsUtils.SetValue("Favorite" + decorationType, Utils.PlayerPrefsUtils.GetValue<string>("Favorite" + decorationType) + decorationId.ToString() + "/" + skinId.ToString() + ",");
            else Utils.PlayerPrefsUtils.SetValue("Favorite" + decorationType, Utils.PlayerPrefsUtils.GetValue<string>("Favorite" + decorationType) + decorationId.ToString() + ",");
        }
        void RemoveFromFavoriteList()
        {
            if (decorationType == "Character")
                Utils.PlayerPrefsUtils.SetValue("Favorite" + decorationType, Utils.PlayerPrefsUtils.GetValue<string>("Favorite" + decorationType).Replace(decorationId.ToString() + "/" + skinId.ToString() + ",", ""));
            else Utils.PlayerPrefsUtils.SetValue("Favorite" + decorationType, Utils.PlayerPrefsUtils.GetValue<string>("Favorite" + decorationType).Replace(decorationId.ToString() + ",", ""));
        }
        public bool IsFavorite()
        {
            if (decorationType == "Character")
            {
                string favoriteObj = Utils.PlayerPrefsUtils.GetValue<string>("Favorite" + decorationType);
                string[] favoriteObjs = favoriteObj.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                return favoriteObjs.Contains($"{decorationId}/{skinId}");
            }
            else
            {
                string favoriteObj = Utils.PlayerPrefsUtils.GetValue<string>("Favorite" + decorationType);
                string[] favoriteObjs = favoriteObj.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                return favoriteObjs.Contains(decorationId.ToString());
            }
        }
    }
}