using Command.Lib.AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Command.Lib.Primitives;

public class Vector
{
    Point p1;
    Point p2;
    public Vector(Point p1, Point p2)
    {
        this.p1 = p1;
        this.p2 = p2;
    }

    public static Vector FromPoints(Point p1, Point p2)
    {
        return new Vector(p1, p2);
    }

    // Find the next point along a vector direction, multiplying distance by a scalar
    public Point NextPoint(int scalar = 2)
    {
        var distanceX = p1.DistanceX(p2);
        var distanceY = p1.DistanceY(p2);

        return new Point(p2.X + (distanceX * (scalar - 1)), p2.Y + (distanceY * (scalar - 1)));
    }
}
