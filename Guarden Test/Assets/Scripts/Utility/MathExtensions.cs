using UnityEngine;

namespace TheGuarden.Utility
{
    public static class MathExtensions
    {
        public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
        {
            Vector3 AB = b - a;
            Vector3 AV = value - a;
            return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
        }

        public static float CalculateDistanceBasedOnFrustum(float width, float aspectRatio, float fov)
        {
            float height = width / aspectRatio;
            return CalculateDistanceBasedOnFrustum(height, fov);
        }

        public static float CalculateDistanceBasedOnFrustum(float height, float fov)
        {
            return height * 0.5f / Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
        }
    }
}
