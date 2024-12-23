using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Command.Lib.Primitives;

public enum Orientation { _0, _90, _180, _270 }

public struct Bounds
{
    public Point TopLeft { get; }
    public Point BottomRight { get; }
    public Orientation Orientation { get; }

    public Bounds(Point topLeft, Point bottomRight, Orientation orientation = Orientation._0)
    {
        this.TopLeft = topLeft;
        this.BottomRight = bottomRight;
        this.Orientation = orientation;
    }

    public Bounds(long x1, long y1, long x2, long y2) : this(new Point(x1, y1), new Point(x2, y2)) { }

    public static Bounds ZeroToPoint(Point p) => new Bounds(0, 0, p.X, p.Y);
    public static Bounds FromPoint(Point p) => new Bounds(p, p);
    public static Bounds FromPoints(params IEnumerable<Point> points)
    {
        if (!points.Any()) return Zero;
        Bounds bounds = Bounds.FromPoint(points.First());
        foreach (var point in points) bounds = bounds.Expand(point);
        return bounds;
    }
    public long Top => TopLeft.Y;
    public long Bottom => BottomRight.Y;
    public long Left => TopLeft.X;
    public long Right => BottomRight.X;
    public long Height => BottomRight.Y - TopLeft.Y + 1;
    public long Width => BottomRight.X - TopLeft.X + 1;
    public long Size => Width * Height;
    public Bounds LocalBounds => new Bounds((0, 0), (Right - Left, Bottom - Top));

    public static Bounds Zero = Bounds.FromPoint((0, 0));

    public Bounds Clone(Point? topLeft = null, Point? bottomRight = null)
    {
        return new Bounds(topLeft ?? TopLeft, bottomRight ?? BottomRight);
    }

    public bool Contains(Point p)
    {
        return p.WithinBounds(TopLeft, BottomRight);
    }

    public Point ToLocal(Point p)
    {
        Debug.Assert(Contains(p), $"The point {p} is not within the bounds {this}");
        var unadjustedLocal = new Point(p.X - TopLeft.X, p.Y - TopLeft.Y);
        return Orientation switch
        {
            Orientation._0 => unadjustedLocal,
            Orientation._270 => RotateLocal90(unadjustedLocal),
            Orientation._180 => RotateLocal90(RotateLocal90(unadjustedLocal)),
            Orientation._90 => RotateLocal90(RotateLocal90(RotateLocal90(unadjustedLocal))),
            _ => throw new NotImplementedException(),
        };
    }

    public Point FromLocal(Point p)
    {
        var unadjustedLocal = Orientation switch
        {
            Orientation._0 => p,
            Orientation._90 => RotateLocal90(p),
            Orientation._180 => RotateLocal90(RotateLocal90(p)),
            Orientation._270 => RotateLocal90(RotateLocal90(RotateLocal90(p))),
            _ => throw new NotImplementedException(),
        };
        var result = new Point(unadjustedLocal.X + TopLeft.X, unadjustedLocal.Y + TopLeft.Y);
        Debug.Assert(Contains(result), $"The point {result} is not within the bounds {this}");
        return result;
    }

    public static bool operator ==(Bounds a, Bounds b) => a.TopLeft == b.TopLeft && a.BottomRight == b.BottomRight;
    public static bool operator !=(Bounds a, Bounds b) => !(a == b);
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Bounds bounds && this == bounds;
    }

    override public int GetHashCode()
    {
        return TopLeft.GetHashCode() ^ BottomRight.GetHashCode() ^ Orientation.GetHashCode();
    }

    private Point RotateLocal90(Point p)
    {
        return new Point(this.LocalBounds.Bottom - p.Y, p.X);

    }

    public override string ToString()
    {
        return $"{TopLeft} => {BottomRight}";
    }

    public Bounds Expand(Point p)
    {
        var bounds = this;
        if (p.X < bounds.TopLeft.X)
        {
            bounds = new Bounds(new Point(p.X, bounds.TopLeft.Y), bounds.BottomRight);
        }
        if (p.Y < bounds.TopLeft.Y)
        {
            bounds = new Bounds(new Point(bounds.TopLeft.X, p.Y), bounds.BottomRight);
        }
        if (p.X > bounds.BottomRight.X)
        {
            bounds = new Bounds(bounds.TopLeft, new Point(p.X, bounds.BottomRight.Y));
        }
        if (p.Y > bounds.BottomRight.Y)
        {
            bounds = new Bounds(bounds.TopLeft, new Point(bounds.BottomRight.X, p.Y));
        }

        return bounds;
    }

    public Point WrapTo(Point p)
    {
        var x = p.X;
        var y = p.Y;
        while (x < Left) x += Width;
        while (x > Right) x -= Width;
        while (y < Top) y += Height;
        while (y > Bottom) y -= Height;

        return new Point(x, y);
    }

    public void Print(Func<Point, char> point)
    {
        for (long y = Top; y <= Bottom; y++)
        {
            for (long x = Left; x <= Right; x++)
            {
                Console.Write(point(new Point(x, y)));
            }
            Console.WriteLine();
        }
    }

    public string ToString(Func<Point, char> point)
    {
        var build = new StringBuilder();
        for (long y = Top; y <= Bottom; y++)
        {
            for (long x = Left; x <= Right; x++)
            {
                build.Append(point(new Point(x, y)));
            }
            build.AppendLine();
        }

        return build.ToString();
    }

    public IEnumerable<Point> Inside(params IEnumerable<Point> points)
    {
        foreach (var p in points)
        {
            if (Contains(p))
            {
                yield return p;
            }
        }
    }

    public IEnumerable<Point> Points()
    {
        for (var y = Top; y <= Bottom; y++)
        {
            for (var x = Left; x <= Right; x++)
            {
                yield return new Point(x, y);
            }
        }
    }


}
