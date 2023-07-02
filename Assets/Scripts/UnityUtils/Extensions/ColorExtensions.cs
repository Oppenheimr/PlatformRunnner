using System;
using UnityEngine;

namespace UnityUtils.Extensions
{
    public static class ColorExtensions
    {
        public static Color DarkRed = new Color(0.6f,0.2f,0.2f);
        public static Color DarkGreen = new Color(0,0.4f,0);
        public static Color Orange = new Color(0.8f,0.5f,0);

        public static Color SetAlpha(this Color color, float alpha)
        {
            color = new Color(color.r, color.g, color.b, alpha);
            return color;
        }

        public static bool EqualsWithTolerance(this Color color, Color target, float tolerance)
        {
            if (!(Math.Abs(color.r - target.r) < tolerance)) return false;
            if (!(Math.Abs(color.g - target.g) < tolerance)) return false;
            return Math.Abs(color.b - target.b) < tolerance;
        }
        
        public static bool EqualsWithTolerance(this Color32 color, Color32 target, float tolerance)
        {
            if (!(Math.Abs(color.r - target.r) < tolerance)) return false;
            if (!(Math.Abs(color.g - target.g) < tolerance)) return false;
            return Math.Abs(color.b - target.b) < tolerance;
        }
    }
}