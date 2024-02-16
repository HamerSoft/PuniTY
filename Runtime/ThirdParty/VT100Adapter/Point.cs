using System;

namespace HamerSoft.PuniTY.ThirdParty.VT100Adapter
{
    public struct Point : IEquatable<Point>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsEmpty => X == 0 && Y == 0;
        public static Point Empty => new Point(0, 0);

        public Point(int position)
        {
            X = position;
            Y = position;
        }

        public Point(int positionX, int positionY)
        {
            X = positionX;
            Y = positionY;
        }

        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Point other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator !=(Point a, Point b)
            => !a.Equals(b);

        public static bool operator ==(Point a, Point b) => a.Equals(b);
    }
}