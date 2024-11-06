﻿namespace ApiAggregator.Helpers
{
    public static class Constraints
    {
        public static void NotNull<T>(this T value)
        {
            if (value == null)
                throw new ArgumentNullException(typeof(T).Name);
        }
    }
}