using System;
using Microsoft.Xna.Framework;

namespace XnaGui.Helpers {
    public static class MathHelper {
        /// <summary>
        /// Calculates the slope of a line given the 2 points.
        /// </summary>
        /// <param name="point1">The first point.</param>
        /// <param name="point2">The second point.</param>
        /// <returns>The slope.</returns>
        public static float GetSlope(Vector2 point1, Vector2 point2) {
            return (point2.Y - point1.Y) / (point2.X - point1.X);
        }

        /// <summary>
        /// Converts <paramref name="degrees"/> to radians.
        /// </summary>
        /// <param name="degrees">The degrees.</param>
        /// <returns>The radian equivalent.</returns>
        public static float ToRadians(float degrees) {
            return Microsoft.Xna.Framework.MathHelper.ToRadians(degrees);
        }

        /// <summary>
        /// Converts <paramref name="radians"/> to degrees.
        /// </summary>
        /// <param name="radians">The radians.</param>
        /// <returns>The degree equivalent.</returns>
        public static float ToDegrees(float radians) {
            return Microsoft.Xna.Framework.MathHelper.ToDegrees(radians);
        }

        /// <summary>
        /// Calculates the incline angle of the specified slope.
        /// </summary>
        /// <param name="slope">The slope.</param>
        /// <returns>The incline angle.</returns>
        public static float GetInclineAngle(float slope) {
            return (float) Math.Atan2(Math.Abs(slope), 1);
        }
    }
}