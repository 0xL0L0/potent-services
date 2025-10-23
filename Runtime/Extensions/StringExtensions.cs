using System;
using System.IO;
using UnityEngine;

namespace Potency.Services.Runtime.Extensions
{
    public static class StringExtensions
    {
        public static string UnityToFullAssetPath(this string unityAssetPath)
        {
            string assetsPath = Application.dataPath.Substring(0, Application.dataPath.IndexOf("Assets", StringComparison.Ordinal));
            return Path.Combine(assetsPath, unityAssetPath);
        }
        
        public static float TryParseFloatOrZero(this string thisString) => float.TryParse(thisString, out var result) ? result : 0;
    }
}