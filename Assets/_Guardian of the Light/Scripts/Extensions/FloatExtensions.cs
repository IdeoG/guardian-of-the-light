using UnityEngine;

namespace _Guardian_of_the_Light.Scripts.Extensions
{
    public static class FloatExtensions
    {
        public static bool EqualWith(this float origin, float destination)
        {
            return Mathf.Abs(origin - destination) < 1e-2;
        }

        public static bool IsBetweenBounds(this float origin, float bound1, float bound2)
        {
            return Mathf.Abs(bound1) <= Mathf.Abs(origin)
                   && Mathf.Abs(origin) < Mathf.Abs(bound2)
                   && bound1 * origin > 0
                   && bound2 * origin > 0;
        }
    }
}