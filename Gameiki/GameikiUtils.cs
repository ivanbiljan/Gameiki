using System;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Terraria;

namespace Gameiki {
    internal static class GameikiUtils {
        public static string GetColoredString(string message, Color color) => $"[c/{color.Hex3()}:{message}]";

        public static int GetRandom(int max) {
            var bytes = new byte[4];
            using (var cryptoProvider = new RNGCryptoServiceProvider()) {
                cryptoProvider.GetBytes(bytes);
                return (int) (BitConverter.ToUInt32(bytes, 0) % max);
            }
        }

        public static void SetTime(int hours, int minutes) {
            var time = hours % 24 + minutes / 60.0m - 4.50m;
            if (time < 0.00m) {
                time += 24.00m;
            }

            if (time >= 15.00m) {
                Main.dayTime = false;
                Main.time = (double) ((time - 15.00m) * 3600.0m);
            }
            else {
                Main.dayTime = true;
                Main.time = (double) (time * 3600.0m);
            }
        }
    }
}