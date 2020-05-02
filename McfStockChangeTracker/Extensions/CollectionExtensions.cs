using System.Collections.Generic;
using System.Linq;

namespace McfStockChangeTracker.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmptyCollection<T>(this IEnumerable<T> collection)
        {
            return collection is null || !collection.Any();
        }
    }
}
