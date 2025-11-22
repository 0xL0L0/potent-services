using UnityEngine.Networking;

namespace Potency.Services.Runtime.Utils
{
    public static class Extensions
    {
        public static void TryAppendAuthHeader(this UnityWebRequest request, string authToken)
        {
            if (!string.IsNullOrEmpty(authToken))
            {
                request.SetRequestHeader("Authorization", "Bearer " + authToken);
            }
        }
        
        public static bool IsNumber(this object value)
        {
            return value is sbyte
                   || value is byte
                   || value is short
                   || value is ushort
                   || value is int
                   || value is uint
                   || value is long
                   || value is ulong
                   || value is float
                   || value is double
                   || value is decimal;
        }
        
        public static bool IsString(this object value)
        {
            return value is string;
        }
    }
}
