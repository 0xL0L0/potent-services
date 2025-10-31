using System;

namespace Potency.Services.Runtime.Utils.Rng
{
    public static class Rng
    {
        private static Random _rng = new Random();

        public static void Seed(int seed)
        {
            _rng = new Random(seed);
        }

        public static int Range(int min, int max)
        {
            return _rng.Next(min, max);
        }
        
        public static double Range(double min, double max)
        {
            var randomDouble = _rng.NextDouble();
            var rangeConvertedDouble = randomDouble * (max - min) + min;
            return rangeConvertedDouble;
        }

        public static int Next()
        {
            return _rng.Next();
        }
    }
}