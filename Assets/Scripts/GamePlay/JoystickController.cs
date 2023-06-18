using UnityEngine;

namespace GamePlay
{
    public class JoystickController : FloatingJoystick
    {
        private float _horizontal;
        private float _vertical;
        public Vector2 Input
        {
            get
            {
                #if UNITY_EDITOR
                if (Horizontal != 0 && Vertical != 0)
                    return new Vector2(Horizontal, Vertical);
                return new Vector2(_horizontal, _vertical);
                #endif
                return new Vector2(Horizontal, Vertical);
            }
        }
#if UNITY_EDITOR
        public void Update()
        {
            _horizontal = UnityEngine.Input.GetAxis("Horizontal");
            _vertical = UnityEngine.Input.GetAxis("Vertical");
        }
#endif

        private static JoystickController _instance;
        public static JoystickController Instance => _instance = FindObjectOfType<JoystickController>();
    }
}