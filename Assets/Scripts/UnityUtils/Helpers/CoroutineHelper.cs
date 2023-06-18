using System;
using System.Collections;
using UnityEngine;

namespace UnityUtils.Helpers
{
    public static class CoroutineHelper
    {
        public static IEnumerator WaitCondition(Func<bool> condition)
        {
            while (!condition())
                yield return new WaitForEndOfFrame();
        }
    }
}