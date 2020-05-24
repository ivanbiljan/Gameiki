using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaGui.Extensions {
    // TODO: Look into BasicEffect, refactor this to reduce memory usage and improve performance (write an actual primitives library?)

    /// <summary>
    ///     Provides extension methods for the <see cref="SpriteBatch" /> class.
    /// </summary>
    public static class SpriteBatchExtensions {
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

        public static void DrawArc(this SpriteBatch spriteBatch, Vector2 center, int radius, float startAngle, float sweepAngle,
            Color color, int borderWidth = 1) {
            if (spriteBatch == null) {
                throw new ArgumentNullException(nameof(spriteBatch));
            }

            var precision = 1 / 10F;
            var firstPoint = new Vector2(center.X + (float) Math.Cos(MathHelper.ToRadians(startAngle)) * radius,
                center.Y + (float) Math.Sin(MathHelper.ToRadians(startAngle)) * radius);
            var secondPoint = new Vector2(center.X + (float) Math.Cos(MathHelper.ToRadians(startAngle + sweepAngle)) * radius,
                center.Y + (float) Math.Sin(MathHelper.ToRadians(startAngle + sweepAngle)) * radius);
            var slope = Helpers.MathHelper.GetSlope(firstPoint, secondPoint);
            var controlPoint = new Vector2(
                center.X + (float) Math.Cos(MathHelper.ToRadians(startAngle + 90 - Helpers.MathHelper.GetInclineAngle(slope))) * radius,
                center.Y + (float) Math.Sin(MathHelper.ToRadians(startAngle + 90 - Helpers.MathHelper.GetInclineAngle(slope))) * radius);

            DrawBezierCurve(spriteBatch, firstPoint, secondPoint, controlPoint, color, borderWidth);
        }

        public static void DrawBezierCurve(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Vector2 controlPoint, Color color,
            int borderWidth = 1) {
            // ReSharper disable once InlineOutVariableDeclaration
            Vector2 currentVertex;
            var precision = 1 / 8F;
            var vertices = new List<Vector2>();
            for (float i = 0; i < 1.1; i += precision) {
                ComputeBezierCurvePoint(point1, point2, controlPoint, i, out currentVertex);
                vertices.Add(currentVertex);
            }

            for (var i = 1; i < vertices.Count; ++i) {
                DrawLine(spriteBatch, vertices[i - 1], vertices[i], color, borderWidth);
            }

            void ComputeBezierCurvePoint(Vector2 p1, Vector2 p2, Vector2 c, float t, out Vector2 final) {
                final.X = p1.X * (float) Math.Pow(1 - t, 2) + 2 * c.X * (1 - t) * t + p2.X * (float) Math.Pow(t, 2);
                final.Y = p1.Y * (float) Math.Pow(1 - t, 2) + 2 * c.Y * (1 - t) * t + p2.Y * (float) Math.Pow(t, 2);
            }
        }

        public static void DrawEllipse(this SpriteBatch spriteBatch, Vector2 center, int a, int b, Color color, int borderWidth = 1) {
            // (x - center.X)^2 / a^2 + (y - center.Y)^2 / b^2 = 1 --> solve for y
            var vertices = new List<Vector2>();
            for (var x = center.X - a; x <= center.X + a; x += 2) {
                var y1 = center.Y + b * Math.Sqrt(Math.Pow(a, 2) - Math.Pow(x - center.X, 2)) / a;
                var y2 = center.Y - b * Math.Sqrt(Math.Pow(a, 2) - Math.Pow(x - center.X, 2)) / a;

                vertices.Insert(0, new Vector2(x, (float) y1));
                vertices.Add(new Vector2(x, (float) y2));
            }

            for (var i = 1; i < vertices.Count; ++i) {
                DrawLine(spriteBatch, vertices[i - 1], vertices[i], Color.Red);
            }
        }

        public static void DrawHorizontalLine(this SpriteBatch spriteBatch, Vector2 position, int length, Color color, float rotation = 0f,
            int borderWidth = 1) {
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

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 startPosition, Vector2 endPosition, Color color,
            int borderWidth = 1) {
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

        public static void DrawRectangle(this SpriteBatch spriteBatch, Vector2 position, int width, int height, Color color,
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

        public static void DrawRightTriangle(this SpriteBatch spriteBatch, int width, int height, Color color) {
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

        public static void FillCircle(this SpriteBatch spriteBatch, Vector2 center, int radius, Color color) {
            if (spriteBatch == null) {
                throw new ArgumentNullException(nameof(spriteBatch));
            }

            if (radius < 0) {
                throw new ArgumentOutOfRangeException(nameof(radius));
            }

            for (var x = center.X - radius; x <= center.X + radius; ++x) {
                // (x - p)^2 + (y - q)^2 = r^2 --> solve for y
                var y1 = (float) Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(x - center.X, 2)) + center.Y;
                var y2 = (float) -Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(x - center.X, 2)) + center.Y;
                DrawLine(spriteBatch, new Vector2(x, y1), new Vector2(x, y2), color);
            }
        }

        public static void FillRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color) {
            if (spriteBatch == null) {
                throw new ArgumentNullException(nameof(spriteBatch));
            }

            spriteBatch.Draw(XnaGui.WhiteTexture, rectangle, color);
        }
    }
}