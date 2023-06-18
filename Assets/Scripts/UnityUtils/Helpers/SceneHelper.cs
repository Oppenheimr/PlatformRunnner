using System.Collections;
using UnityEngine.SceneManagement;

namespace UnityUtils.Helpers
{
    public static class SceneHelper
    {
        public static IEnumerator WaitScene(string sceneName)
        {
            yield return CoroutineHelper.WaitCondition(() => SceneManager.GetActiveScene().name == sceneName);
        }
    }
}