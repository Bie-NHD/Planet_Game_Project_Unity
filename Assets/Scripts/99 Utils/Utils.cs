using Math = System.Math;

namespace Utils
{
    public static class MathUtils
    {
        public static bool Clamp01ToBool(float value)
        {
            float clampedValue = Math.Clamp(value, 0, 1);
            return clampedValue > 0.5f;
        }

        public static bool Clamp01ToBool(int value)
        {
            int clampedValue = Math.Clamp(value, 0, 1);
            return clampedValue == 1;
        }

        public static int BoolToInt(bool value)
        {
            return value ? 1 : 0;
        }
    }
}
