using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGui {
    public static class Utils {
        public static Texture2D TextureFromImage(Image image) {
            if (image == null) {
                throw new ArgumentNullException(nameof(image));
            }

            using (var stream = new MemoryStream()) {
                image.Save(stream, ImageFormat.Png);
                stream.Position = 0;
                return Texture2D.FromStream(XnaGui.GraphicsDevice, stream);
            }
        }

        public static Texture2D TextureFromPng(string path) {
            if (path == null) {
                throw new ArgumentNullException(nameof(path));
            }

            if (!File.Exists(path)) {
                throw new FileNotFoundException();
            }

            if (Path.GetExtension(path) != ".png") {
                throw new ArgumentException("The specified file is not a .png file.");
            }

            return Texture2D.FromStream(XnaGui.GraphicsDevice, File.OpenRead(path));
        }
    }
}