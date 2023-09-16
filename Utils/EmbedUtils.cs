using System.IO;
using UnityEngine;
using System.Reflection;

namespace Utils
{
    static public class EmbedUtils
    {

        static public Sprite GetImage(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream resourceStream = assembly.GetManifestResourceStream(resourceName);
            if (resourceStream == null)
            {
                Debug.LogError($"Resource stream not found for {resourceName}");
                return null;
            }
            Texture2D texture = new Texture2D(1, 1);
            byte[] bytes = ReadFully(resourceStream);
            if (bytes == null)
            {
                Debug.LogError($"Failed to read resource bytes for {resourceName}");
                return null;
            }
            texture.LoadImage(bytes);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}