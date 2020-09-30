using System;

namespace BlazorAI.Shared.Types
{
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int ManhattanDistanceTo(Point p) => Math.Abs(X - p.X) + Math.Abs(Y - p.Y);

        public double DistanceTo(Point p)
        {
            var dx = Math.Abs(X - p.X);
            var dy = Math.Abs(Y - p.Y);

            return Math.Sqrt(dx * dx + dy * dy);
        }

        public override string ToString() => $"{X},{Y}";
    }
}
