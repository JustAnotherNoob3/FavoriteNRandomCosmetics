using System;
using Game.Decorations;
using UnityEngine;

namespace Utils
{
    class PlayerPrefsUtils
    {
        public static void Startup()
        {
            foreach (DecorationType key in Enum.GetValues(typeof(DecorationType)))
            {
                if (!PlayerPrefs.HasKey($"Favorite{key.ToString()}"))
                {
                    SetValue($"Favorite{key.ToString()}", "");
                }
                if (!PlayerPrefs.HasKey($"Randomize{key.ToString()}"))
                {
                    SetValue($"Randomize{key.ToString()}", false);
                }
            }

        }

        public static T GetValue<T>(string key)
        {
            object result = null;
            if (typeof(T) == typeof(string))
            {
                result = PlayerPrefs.GetString(key);
            }
            else if (typeof(T) == typeof(bool))
            {
                result = PlayerPrefs.GetInt(key) == 1;
            }
            else if (typeof(T) == typeof(int))
            {
                result = PlayerPrefs.GetInt(key);
            }
            return (T)result;

        }
        public static void SetValue(string key, bool value)
        {

            if (value)
            {
                PlayerPrefs.SetInt(key, 1);
                PlayerPrefs.Save();
                return;
            }
            PlayerPrefs.SetInt(key, 0);
            PlayerPrefs.Save();
            return;

        }
        public static void SetValue(string key, int value)
        {
            PlayerPrefs.SetInt(key, (int)value);
            PlayerPrefs.Save();
        }
        public static void SetValue(string key, string value)
        {
            PlayerPrefs.SetString(key, (string)value);
            Debug.Log("Set " + key + " to " + value);
            PlayerPrefs.Save();
        }

    }
}
