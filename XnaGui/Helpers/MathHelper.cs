using System;
using Microsoft.Xna.Framework;

namespace XnaGui.Helpers {
    public static class MathHelper {
        /// <summary>
        ///     Calculates the incline angle of the specified slope.
        /// </summary>
        /// <param name="slope">The slope.</param>
        /// <returns>The incline angle.</returns>
        public static float GetInclineAngle(float slope) => (float) Math.Atan2(Math.Abs(slope), 1);

        /// <summary>
        ///     Calculates the slope of a line given the 2 points.
        /// </summary>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The slope.</returns>
        public static float GetSlope(Vector2 point1, Vector2 point2) => (point2.Y - point1.Y) / (point2.X - point1.X);

        /// <summary>
        ///     Converts <paramref name="radians" /> to degrees.
        /// </summary>
        /// <param name="radians">The radians.</param>
        /// <returns>The degree equivalent.</returns>
        public static float ToDegrees(float radians) => Microsoft.Xna.Framework.MathHelper.ToDegrees(radians);

        /// <summary>
        ///     Converts <paramref name="degrees" /> to radians.
        /// </summary>
        /// <param name="degrees">The degrees.</param>
        /// <returns>The radian equivalent.</returns>
        public static float ToRadians(float degrees) => Microsoft.Xna.Framework.MathHelper.ToRadians(degrees);

        /// <summary>
        /// Computes the results of a quadratic equation.
        /// </summary>
        /// <param name="a">The quadratic coefficient.</param>
        /// <param name="b">The linear coefficient.</param>
        /// <param name="c">The free term.</param>
        /// <returns>A tuple denoting the results.</returns>
        public static Tuple<float, float> ComputeQuadraticEquation(float a, float b, float c) {
            var x1 = (float) (-b + Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (2 * a);
            var x2 = (float) (-b - Math.Sqrt(Math.Pow(b, 2) - 4 * a * c)) / (2 * a);
            return new Tuple<float, float>(x1, x2);
        }
    }
}