namespace UnityUtils.StaticClasses
{
    public static class Platform
    {
        
        public static bool IsEditor => GetPlatform() == PlatformType.Editor;
        public static bool IsWeb => GetPlatform() == PlatformType.Web;
        public static bool IsDesktop => GetPlatform() == PlatformType.Desktop;
        public static bool IsMobile => GetPlatform() == PlatformType.Mobile;
        
        public static PlatformType GetPlatform()
        {
#if UNITY_EDITOR
            return PlatformType.Editor;
#elif UNITY_ANDROID || UNITY_IOS
            return PlatformType.Mobile;
#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            return PlatformType.Desktop;
#elif UNITY_WEBGL
            return PlatformType.Web;
#endif
        }
    }

    public enum PlatformType
    {
        Editor,
        Web,
        Desktop,
        Mobile,
    }
}