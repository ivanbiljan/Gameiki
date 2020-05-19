using System;
using System.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGui.Extensions {
    /// <summary>
    /// Provides extension methods for the <see cref="SpriteBatch"/> class.
    /// </summary>
    public static class SpriteBatchExtensions {
        public static void DrawRightTriangleOutline(this SpriteBatch spriteBatch, int width, int height, Color color) {
            if (spriteBatch == null) {
                throw new ArgumentNullException(nameof(spriteBatch));
            }

            if (width <= 0) {
                throw new ArgumentOutOfRangeException(nameof(width));
            }
            
            if (height <= 0) {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            var colors = new Color[width * height];
            for (var x = 0; x < width; ++x) {
                for (var y = 0; y < height; ++y) {
                    if (x == 0 || x == width - 1) {
                        colors[x * y] = color;
                    }
                    else {
                        colors[x] = color;
                        colors[x * height] = color;
                    }
                }
            }
        }
        
        public static void DrawRectangleOutline(this SpriteBatch spriteBatch, Vector2 position, int width, int height, Color color, int borderWidth = 1) {
            if (spriteBatch == null) {
                throw new ArgumentNullException(nameof(spriteBatch));
            }

            if (width <= 0) {
                throw new ArgumentOutOfRangeException(nameof(width));
            }
            
            if (height <= 0) {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            var colors = new Color[width * height];
            for (var x = 0; x < width; ++x) {
                for (var y = 0; y < height; ++y) {
                    if (x <= borderWidth || x >= width - 1 - borderWidth || y <= borderWidth || y >= height - 1 - borderWidth) {
                        colors[x + y * width] = color;
                    }
                }
            }

            var texture = new Texture2D(spriteBatch.GraphicsDevice, width, height);
            texture.SetData(colors);
            spriteBatch.Draw(texture, position, color);
        }
    }
}