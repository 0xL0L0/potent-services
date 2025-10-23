using System;
using UnityEngine;

namespace Potency.Services.Runtime.Extensions
{
    /// <summary>
    /// A class for reusing the animation functions used to initialize a `UnityEngine.AnimationCurve` by using our
    /// custom extension `UnityExtensions.CreateFromFunction`.
    ///
    /// The range in the x-axis (the parameter) always from 0 to 2 * PI
    /// 
    /// The tangent values in the range of the y-axis (the return value) are represented by the postfix used in the names
    /// using 'N' for negative values (p.e. `Sin_0_1_N1_0` goes from 0 to 1 then to -1 and finally to 0 at the end 
    /// </summary>
    public static class PredefinedAnimations
    {
        private enum FunctionType { BackEaseInOut_0_1, CubicEaseOut_0_1, Cos_0_1, Cos_0_2_0, Sin_0_1_N1_0, Cos_1_0_1 }

        public static Func<float, float> CubicEaseOut_0_1 => GetFunction(FunctionType.CubicEaseOut_0_1);
        public static Func<float, float> BackEaseInOut_0_1 => GetFunction(FunctionType.BackEaseInOut_0_1);
        public static Func<float, float> Cos_0_1 => GetFunction(FunctionType.Cos_0_1);
        public static Func<float, float> Cos_0_2_0 => GetFunction(FunctionType.Cos_0_2_0);
        public static Func<float, float> Sin_0_1_N1_0 => GetFunction(FunctionType.Sin_0_1_N1_0);
        public static Func<float, float> Cos_1_0_1 => GetFunction(FunctionType.Cos_1_0_1);

        private static Func<float, float> GetFunction(FunctionType functionType) => functionType switch
        {
            // https://bit.ly/3CtotqW
            FunctionType.BackEaseInOut_0_1 => (x =>
            {
                x = x * 1.9f / (2 * Mathf.PI);
                return (x * x * x - 3f * x * Mathf.Sin(x * Mathf.PI)) / 8.6251f;
            }),
            
            // https://bit.ly/3rrt5Ym
            FunctionType.CubicEaseOut_0_1 => (x =>
            {
                x = x / (2f * Mathf.PI) - 1f;
                return 1 + x * x * x;
            }),
            
            // https://bit.ly/3SyNWF8
            FunctionType.Cos_0_1 => (x => (1f - Mathf.Cos(x * 0.5f)) * 0.5f),
            
            // https://bit.ly/3C0Pm41
            FunctionType.Cos_0_2_0 => x => 1f - Mathf.Cos(x),
            
            // https://bit.ly/3rqEdon
            FunctionType.Sin_0_1_N1_0 => x => Mathf.Sin(x),
            
            // https://bit.ly/3frLRMo
            FunctionType.Cos_1_0_1 => x => (Mathf.Cos(x) + 1f) * 0.5f,
            
            _ => throw new ArgumentOutOfRangeException(nameof(functionType), functionType, null)
        };
    }
}