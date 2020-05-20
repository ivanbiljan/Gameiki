﻿using System;
using System.Configuration;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGui.Extensions {
    /// <summary>
    /// Provides extension methods for the <see cref="SpriteBatch"/> class.
    /// </summary>
    public static class SpriteBatchExtensions {
        public static void DrawRectangleOutline(this SpriteBatch spriteBatch, Vector2 position, int width, int height, Color color,
            int borderWidth = 1) {
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

        public static void DrawHorizontalLine(this SpriteBatch spriteBatch, Vector2 position, int length, Color color, float rotation = 0f, int borderWidth = 1) {
            if (spriteBatch == null) {
                throw new ArgumentNullException(nameof(spriteBatch));
            }

            if (length < 0) {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (borderWidth < 0) {
                throw new ArgumentException(nameof(borderWidth));
            }
            
            // var colors = new Color[length * borderWidth];
            // for (var x = 0; x < length; ++x) {
            //     for (var y = 0; y < borderWidth; ++y) {
            //         colors[x + y * length] = color;
            //     }
            // }

            // var texture = new Texture2D(spriteBatch.GraphicsDevice, length, borderWidth);
            // texture.SetData(colors);
            // spriteBatch.Draw(texture, position, null, color, rotation, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.Draw(XnaGui.WhiteTexture, position, null, color, rotation, Vector2.Zero, new Vector2(length, borderWidth),
                SpriteEffects.None, 0);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 startPosition, Vector2 endPosition, Color color, int borderWidth = 1) {
            if (spriteBatch == null) {
                throw new ArgumentNullException(nameof(spriteBatch));
            }
            
            if (borderWidth < 0) {
                throw new ArgumentException(nameof(borderWidth));
            }

            var angle = (float) Math.Atan2(endPosition.Y - startPosition.Y, endPosition.X - startPosition.X);
            var length = (int) Vector2.Distance(startPosition, endPosition);
            DrawHorizontalLine(spriteBatch, startPosition, length, color, angle, borderWidth);
        }

        public static void DrawArbitraryPolygon(this SpriteBatch spriteBatch, Vector2[] vertices, Color color, int borderWidth = 1) {
            if (spriteBatch == null) {
                throw new ArgumentNullException(nameof(spriteBatch));
            }
            
            if (borderWidth < 0) {
                throw new ArgumentException(nameof(borderWidth));
            }
            
            for (var i = 0; i < vertices.Length - 1; ++i) {
                DrawLine(spriteBatch, vertices[i], vertices[i + 1], color, borderWidth);
            }
            
            DrawLine(spriteBatch, vertices[vertices.Length - 1], vertices[0], color, borderWidth);
        }

        public static void DrawNSidedPolygon(this SpriteBatch spriteBatch, Vector2 center, int radius, int sides) {
            if (spriteBatch == null) {
                throw new ArgumentNullException(nameof(spriteBatch));
            }

            if (radius < 0) {
                throw new ArgumentException(nameof(radius));
            }

            if (sides < 0) {
                throw new ArgumentException(nameof(sides));
            }
            
            var vertices = new Vector2[sides];
            for (var i = 0; i < vertices.Length; ++i) {
                vertices[i] = new Vector2(radius * (float) Math.Cos(Math.PI * 2 * i / sides) + center.X,
                    radius * (float) Math.Sin(Math.PI * 2 * i / sides) + center.Y);
            }
            
            DrawArbitraryPolygon(spriteBatch, vertices, Color.Aqua);
        }

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
    }
}