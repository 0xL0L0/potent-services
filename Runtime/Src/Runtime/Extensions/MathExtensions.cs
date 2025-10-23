using System;
using UnityEngine;

namespace Potency.Services.Runtime.Extensions
{
    public static class MathExtensions
    {
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static int ToMs(this float value)
        {
            return Mathf.RoundToInt(value * 1000);
        }

        public static bool CloseTo(this float thisFloat, float other)
        {
            return Mathf.Approximately(thisFloat, other);
        }

        public static bool CloseTo(this float thisFloat, float other, float maxDistance)
        {
            return Mathf.Abs(thisFloat - other) < maxDistance;
        }

        public static int EuclidDistanceTo(this Vector2Int fromVec, Vector2Int toVec)
        {
            return Mathf.RoundToInt(Vector2Int.Distance(fromVec, toVec));
        }
        
        public static int ManhattanDistanceTo(this Vector2Int fromVec, Vector2Int toVec)
        {
            var xDelta = Math.Abs(fromVec.x - toVec.x);
            var yDelta = Math.Abs(fromVec.y - toVec.y);
            return xDelta + yDelta;
        }
        
        public static int ChebyshevDistanceTo(this Vector2Int fromVec, Vector2Int toVec)
        {
            var xDelta = Math.Abs(fromVec.x - toVec.x);
            var yDelta = Math.Abs(fromVec.y - toVec.y);
            return Math.Max(xDelta, yDelta);
        }
    }
}