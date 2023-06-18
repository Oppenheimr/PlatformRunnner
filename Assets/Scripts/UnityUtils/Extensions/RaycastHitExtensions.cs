using UnityEngine;

namespace UnityUtils.Extensions
{
    public static class RaycastHitExtensions
    {
        public static bool IsPlayer(this RaycastHit raycastHit) => raycastHit.transform.CompareTag("Player");
        public static bool IsBot(this RaycastHit raycastHit) => raycastHit.transform.CompareTag("AI");
    }
}