using System;

namespace Ethereum.Encoding
{
    public static class ArraySegmentExtensions
    {
        public static ArraySegment<T> Slice<T>(this ArraySegment<T> source, int start, int? end = null)
        {
            // TODO: safety checks (e.g. end not greater than length etc)
            start += source.Offset;
            var count = end.HasValue ? end.Value : Math.Abs(start - source.Array.Length);

            return new ArraySegment<T>(source.Array, start, count);
        }

        public static T[] ToArray<T>(this ArraySegment<T> source)
        {
            T[] targetArray = new T[source.Count];
            Array.Copy(source.Array, source.Offset, targetArray, 0, source.Count);

            return targetArray;
        }
    }
}
