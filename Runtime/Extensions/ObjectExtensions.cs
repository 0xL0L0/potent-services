namespace Potency.Services.Runtime.Extensions
{
    public static class ObjectExtensions
    {
        public static bool SafeEquals(this object a, object other)
        {
            if (a == null && other != null || a != null && other == null)
            {
                return false;
            }

            if (a == null && other == null)
            {
                return true;
            }

            return a.Equals(other);
        }
    }
}