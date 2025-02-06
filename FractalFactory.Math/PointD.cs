using System;
using System.Diagnostics.CodeAnalysis;

namespace FractalFactory.Math
{
    /// <summary>
    /// Represents an ordered pair of x and y coordinates that define a point in a two-dimensional plane.
    /// </summary>
    /// <remarks>Based on the code underlying System.Drawing.PointF</remarks>
    public struct PointD : IEquatable<PointD>
    {
        private double x; // Do not rename (binary serialization)
        private double y; // Do not rename (binary serialization)

        public static readonly PointD Origin = new PointD(0, 0);

        /// <summary>
        /// Initializes a new instance of a point with the specified coordinates.
        /// </summary>
        public PointD(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>Gets the x-coordinate of this point.</summary>
        public double X
        {
            readonly get => x;
            set => x = value;
        }

        /// <summary>Gets the y-coordinate of this point.</summary>
        public double Y
        {
            readonly get => y;
            set => y = value;
        }

        /// <summary>Translates a point by a given delta.</summary>
        public static PointD operator +(PointD pt, PointD delta) => Add(pt, delta);

        /// <summary>Translates a point by the negative of a given delta.</summary>
        public static PointD operator -(PointD pt, PointD delta) => Subtract(pt, delta);

        /// <summary>
        /// Compares two point objects. Returns true when the X and Y properties of the two objects are equal.
        /// </summary>
        public static bool operator ==(PointD left, PointD right) => left.X == right.X && left.Y == right.Y;

        /// <summary>
        /// Compares two point objects. Returns true when the X and Y properties of the two objects are not equal.
        /// </summary>
        public static bool operator !=(PointD left, PointD right) => !(left == right);

        /// <summary>Translates a point by a given delta.</summary>
        public static PointD Add(PointD pt, PointD delta) => new PointD(pt.X + delta.X, pt.Y + delta.Y);

        /// <summary>Translates a point by the negative of a given delta.</summary>
        public static PointD Subtract(PointD pt, PointD delta) => new PointD(pt.X - delta.X, pt.Y - delta.Y);

        public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is PointD && Equals((PointD)obj);

        public readonly bool Equals(PointD other) => this == other;

        public override readonly int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode());

        public override readonly string ToString() => $"{{X={x}, Y={y}}}";
    }
}
