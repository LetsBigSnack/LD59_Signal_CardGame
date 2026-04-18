using System.Collections.Generic;

namespace Helpers
{
    public static class CollectionHelper
    {
        public static void Shuffle<T>(this IList<T> ts) {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i) {
                var r = UnityEngine.Random.Range(i, count);
                (ts[i], ts[r]) = (ts[r], ts[i]);
            }
        }
        
        public static T Pop<T>(this IList<T> ts) {
            
            if(ts.Count == 0) return default;
            
            T firstElement = ts[0];
            ts.RemoveAt(0);
            
            return firstElement;
        }
    }
}