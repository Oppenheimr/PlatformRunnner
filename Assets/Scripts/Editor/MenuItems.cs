using UnityEditor;
using UnityEngine;
using UnityUtils.Extensions;
namespace Editor
{
    public static class MenuItems
    {
        [MenuItem("Platform Runnner/Documentation")]
        public static void OpenGameDesignDoc()
        {
            //=> Application.OpenURL(ProjectUrls.GameDesignDoc);
        }
    }
}