using UnityEngine;
using UnityEngine.UI;

namespace UnityUtils.Extensions
{
    public static class TextureExtensions
    {
        public static Texture2D DecodeByteArrayToTexture2D(this byte[] textureData)
        {
            var asd = new Texture2D(1920, 1080, TextureFormat.RGBA32 , false);
            asd.Apply();
            asd.LoadImage(textureData);
            asd.Apply();
            return asd;
        }
    }
}