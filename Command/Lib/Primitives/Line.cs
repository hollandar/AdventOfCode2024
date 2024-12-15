using Command.Lib.Primitives;

namespace Command.Primitives;

public struct Line
{
    private double m; // gradient
    private double c; // y-intercept

    public Line(float m, long c)
    {
        this.m = m;
        this.c = c;
    }

    public Line(Point p1, Point p2)
    {
        double a = p2.Y - p1.Y;
        double b = p2.X - p1.X;
        this.m = a / b;
        this.c = p1.Y - (m * p1.X);
    }

    public static Line FromPoints(Point p1, Point p2)
    {
        return new Line(p1, p2);
    }

    public float Y_GivenX(double x)
    {
        return (long)(m * x + c);
    }

    public float X_GivenY(double y)
    {
        return (long)((y - c) / m);
    }

    public Point PointGivenX(double x)
    {
        return new Point((long)Math.Round(x), (long)Math.Round(Y_GivenX(x)));
    }

    public Point PointGivenY(double y)
    {
        return new Point((long)Math.Round(X_GivenY(y)), (long)Math.Round(y));
    }

    public (bool i, double x, double y) Intersects(Line l)
    {
        if (m == l.m) return (false, 0, 0);
        var x = (l.c - c) / (m - l.m);
        var y = Y_GivenX(x);
        return (true, x, y);
    }
    public override string ToString()
    {
        return $"y = {m}x + {c}";
    }
}
