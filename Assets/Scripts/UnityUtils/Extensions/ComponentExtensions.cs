using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
#endif

namespace UnityUtils.Extensions
{
    public static class ComponentExtensions
    {
        public static bool CompareTags(this Component component, params string[] tags) =>
            tags.Any(component.CompareTag);

        public static bool IsPlayer(this Component comp) => comp.CompareTag("Player");
        public static bool IsBot(this Component comp) => comp.CompareTag("AI");

        public static void DestroyImmediateInChildren(this Component target)
        {
            for (int i = 0; i < target.transform.childCount; i++)
                Object.DestroyImmediate(target.transform.GetChild(i).gameObject);
        }

#if PHOTON_UNITY_NETWORKING
        public static bool IsLocalPlayer(this Component comp)
        {
            if (!comp.CompareTag("Player"))
                return false;

            if (!comp.TryGetComponent(out PhotonView view))
                return false;
            
            return view.IsMine;
        }

#endif
    }
}