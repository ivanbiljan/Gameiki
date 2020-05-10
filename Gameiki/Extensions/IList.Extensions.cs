using System.Collections.Generic;
using System.Linq;

namespace Gameiki.Extensions {
    public static class IListExtensions {
        public static void Shuffle<T>(this IList<T> list) {
            var remaining = list.Count();
            while (remaining > 0) {
                var randomIndex = GameikiUtils.GetRandom(list.Count);
                var temp = list[remaining];
                list[remaining--] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }
    }
}