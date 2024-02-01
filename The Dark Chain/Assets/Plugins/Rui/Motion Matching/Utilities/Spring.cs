namespace Boopoo.MotionMatching
{
    public static class Spring
    {
        public static float CalculateDampingFromHalfLife(float halflife, float epsilon = 1e-5f)
        {
            return (4.0f * Math.LN2) / (halflife + epsilon);
        }

        public static void SolveDamper(
            ref float position,
            ref float velocity,
            float targetPosition,
            float dampingHalfLife,
            float deltaTime)
        {
            float dampingFactor = CalculateDampingFromHalfLife(dampingHalfLife) / 2.0f;
            float error = position - targetPosition;
            float correction = velocity + error * dampingFactor;
            float expTerm = Math.CalculateFastNegativeExponential(dampingFactor * deltaTime);

            position = expTerm * (error + correction * deltaTime) + targetPosition;
            velocity = expTerm * (velocity - correction * dampingFactor * deltaTime);
        }
    }
}