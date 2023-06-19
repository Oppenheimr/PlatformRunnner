using UnityEngine;

namespace GamePlay
{
    /// <summary>
    /// Holds joystick controls and values.
    /// </summary>
    public class JoystickController : FloatingJoystick
    {
        private float _horizontal;
        private float _vertical;

        public Vector2 Input
        {
            get
            {
                // Check if the game is running in Unity Editor
#if UNITY_EDITOR
                if (Horizontal != 0 && Vertical != 0)
                    return new Vector2(Horizontal, Vertical);

                return new Vector2(_horizontal, _vertical);
#endif

                // If not running in Unity Editor, return the current input values
                return new Vector2(Horizontal, Vertical);
            }
        }

// Update method called in Unity Editor
#if UNITY_EDITOR
        public void Update()
        {
            // Retrieve input values from the horizontal and vertical axes in Unity Input system
            _horizontal = UnityEngine.Input.GetAxis("Horizontal");
            _vertical = UnityEngine.Input.GetAxis("Vertical");
        }
#endif

        //This is examlpe for singleton
        private static JoystickController _instance;
        public static JoystickController Instance =>  _instance ? _instance :
            (_instance = FindObjectOfType<JoystickController>());

    }
}