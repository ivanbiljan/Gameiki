using System.Collections.Concurrent;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Terraria;

namespace Gameiki.Extensions {
    public static class PlayerExtensions {
        private static readonly ConcurrentDictionary<string, object> Properties = new ConcurrentDictionary<string, object>();

        public static T GetData<T>(this Player player, string name) => Properties.TryGetValue(name, out var value) ? (T) value : default;

        public static void SetData<T>(this Player player, string name, T obj) {
            Properties[name] = obj;
        }

        public static void SendGameikiMessage(this Player player, string message, Color color = default) {
            if (color == default) {
                Main.NewTextMultiline($"[Gameiki] {message}");
            }
            else {
                Main.NewTextMultiline($"[Gameiki] {message}", c: color);
            }
        }
    }
}