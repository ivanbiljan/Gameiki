using System.Collections.Concurrent;
using Terraria;

namespace Gameiki.Extensions {
    public static class PlayerExtensions {
        private static readonly ConcurrentDictionary<string, object> Properties = new ConcurrentDictionary<string, object>();

        public static T GetData<T>(this Player player, string name) => Properties.TryGetValue(name, out var value) ? (T) value : default;

        public static void SetData<T>(this Player player, string name, T obj) {
            Properties[name] = obj;
        }
    }
}