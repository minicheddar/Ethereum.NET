using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// Extensions for <see cref="System.Collections.Generic"/>
    /// </summary>
    public static class IEnumerableExtensions
    {
        public static bool IsNotNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return source != null && source.Any();
        }

        public static IEnumerable<T> ToEnumerable<T>(this T source)
        {
            return new[] { source };
        }
    }
}