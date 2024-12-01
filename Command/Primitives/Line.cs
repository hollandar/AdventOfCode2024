namespace Command.Primitives;

public struct Line
{
    private float m; // gradient
    private long c; // y-intercept

    public Line(float m, long c)
    {
        this.m = m;
        this.c = c;
    }

    public Line(Point p1, Point p2)
    {
        long a = p2.Y - p1.Y;
        long b = p2.X - p1.X;
        this.m = a / b;
        this.c =(long)( p1.Y - m * p1.X);
    }

    public static Line FromPoints(Point p1, Point p2)
    {
        return new Line(p1, p2);
    }

    public long Y_GivenX(long x)
    {
        return (long)(m * x + c);
    }

    public long X_GivenY(long y)
    {
        return (long)((y - c) / m);
    }
}
