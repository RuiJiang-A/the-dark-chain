namespace Boopoo.MotionMatching
{
    public static class Math
    {
        public const float LN2 = 0.69314718056f;

        public static float CalculateFastNegativeExponential(float x)
        {
            return 1.0f / (1.0f + x + 0.48f * x * x + 0.235f * x * x * x);
        }
    }
}