using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Command.Primitives;

public struct Point
{
    long x, y;
    public Point(long x, long y)
    {
        this.x = x;
        this.y = y;
    }

    public long X => x;
    public long Y => y;

    public Point()
    {

    }

    public Point(Point p)
    {
        this.x = p.x;
        this.y = p.y;
    }

    public double AngularDistanceTo(Point p)
    {
        var distance = Math.Abs(Math.Sqrt(Math.Pow((p.x - x), 2) + Math.Pow((p.y - y), 2)));
        return distance;
    }

    public long ManhattanDisanceTo(Point point)
    {
        return Math.Abs(x - point.x) + Math.Abs(y - point.y);
    }

    public long HorizontalLength(Point p)
    {
        Debug.Assert(Y == p.Y, $"The points must be on the same row {Y}");
        return Math.Abs(x - p.x) + 1;
    }

    public long VerticalLength(Point p)
    {
        Debug.Assert(X == p.X, $"The points must be in the same column {X}");
        return Math.Abs(y - p.y) + 1;
    }

    public static bool operator==(Point a, Point b) => a.Equals(b);
    public static bool operator!=(Point a, Point b) => !(a.Equals(b));

    public override bool Equals(object? point)
    {
        if (point is Point p){
            return p.X == X && p.Y == Y;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ y.GetHashCode();
    }

    public override string ToString()
    {
        return $"({x}, {y})";
    }

    public bool AdjacentTo(Point point)
    {
        long xadj = Math.Abs(point.x - x);
        long yadj = Math.Abs(point.y - y);

        return xadj <= 1 && yadj <= 1;
    }

    public void MoveUp(int distance)
    {
        y -= distance;
    }

    public void MoveDown(int distance)
    {
        y += distance;
    }

    public void MoveLeft(int distance)
    {
        x -= distance;
    }

    public void MoveRight(int distance)
    {
        x += distance;
    }

    public bool WithinBounds(Point topLeft, Point bottomRight)
    {
        var inX = X >= topLeft.X && X <= bottomRight.X;
        var inY = Y >= topLeft.Y && Y <= bottomRight.Y;
        return inX && inY;
    }

    public bool WithinBounds(Point bottomRight) => WithinBounds(Zero, bottomRight);
    public bool WithinBounds(long x2, long y2) => WithinBounds(Zero, new Point(x2, y2));
    public bool WithinBounds(long x1, long y1, long x2, long y2) => WithinBounds(new Point(x1, y1), new Point(x2, y2));
    public bool WithinBounds(Bounds bounds) => WithinBounds(bounds.TopLeft, bounds.BottomRight);
    public IEnumerable<Point> AdjacentPoints(Bounds bounds)
    {
        if (!WithinBounds(bounds)) throw new Exception("Point is not within bounds");
        if (X > bounds.TopLeft.X && Y > bounds.TopLeft.Y) yield return new Point(x - 1, y - 1);
        if (Y > bounds.TopLeft.Y) yield return new Point(x, y - 1);
        if (X < bounds.BottomRight.X && Y > bounds.TopLeft.Y) yield return new Point(x + 1, y - 1);
        if (X > bounds.TopLeft.X) yield return new Point(x - 1, y);
        if (X < bounds.BottomRight.X) yield return new Point(x + 1, y);
        if (X > bounds.TopLeft.X && Y < bounds.BottomRight.Y) yield return new Point(x - 1, y + 1);
        if (Y < bounds.BottomRight.Y) yield return new Point(x, y + 1);
        if (X < bounds.BottomRight.X && Y < bounds.BottomRight.Y) yield return new Point(x + 1, y + 1);
    }

    public Point LeftAbove() => new Point(x - 1, y - 1);
    public Point North() => new Point(x, y - 1);
    public Point RightAbove() => new Point(x + 1, y - 1);
    public Point West() => new Point(x - 1, y);
    public Point East() => new Point(x + 1, y);
    public Point LeftBelow() => new Point(x - 1, y + 1);
    public Point South() => new Point(x, y + 1);
    public Point RightBelow() => new Point(x + 1, y + 1);

    public static Point Zero = new Point(0, 0);
    public static bool operator ==(Point? a, Point? b) => a?.Equals(b) ?? false;
    public static bool operator !=(Point? a, Point? b) => !(a == b);
}
