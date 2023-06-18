using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if MFPS
using Inherited.Inherited_Lovatto.InGame.Player;
#endif

namespace UnityUtils.Extensions
{
    public static class ImageExtensions
    {
        public static void CalculateFillAmount(this Image image, float currentValue, float maxValue) =>
        image.fillAmount = (currentValue / (maxValue / 100)) / 100;

#if MFPS
        public static void CalculateFillAmountHealth(this Image image, PlayerHealthManager playerHealth) =>
            image.fillAmount = ((float)playerHealth.health / ((float)playerHealth.maxHealth / 100f)) / 100f;
#endif

        #region Unity Editor
#if UNITY_EDITOR
        public static Image InsImage(this GameObject gObject, string name)
        {
            // Create a custom game object
            GameObject imageObject = new GameObject(name);
            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(imageObject, gObject);
            
            return imageObject.AddComponent<Image>();
        }
        
        public static Sprite GetSpriteFromPath(this string filter, string searchInFolder) =>
            filter.GetObjectFromPath<Sprite>(searchInFolder);
#endif
        #endregion
    }
}