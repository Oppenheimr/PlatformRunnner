using UnityEngine;

namespace UnityUtils.Extensions
{
    public static class BehaviourExtensions
    {
        public static void SetActivate(this Behaviour behaviour, bool active)
        {
            if (behaviour == null) return;
            if (behaviour.gameObject.activeSelf != active)
                behaviour.gameObject.SetActive(active);
        }
    }
}