using System.Collections.Generic;

namespace BlazorAI.Shared.Utility
{
    public static class Extensions
    {
        public static IEnumerable<int> To(this int from, int to)
        {
            while (from <= to)
            {
                yield return from++;
            }
        }
    }
}
