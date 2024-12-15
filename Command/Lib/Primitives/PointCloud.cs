using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Command.Lib.Primitives
{
    public class PointCloud
    {
        HashSet<Point> points;

        public PointCloud(params IEnumerable<Point> points)
        {
            this.points = new HashSet<Point>(points);
        }

        public PointCloud()
        {
            points = new HashSet<Point>();
        }
        
        public Bounds Bounds => Bounds.FromPoints(points);

        public bool Contains(Point p)
        {
            return points.Contains(p);
        }

        public void Add(Point p)
        {
            points.Add(p);
        }

        // These calculations are based on a perimeter which is a point that is inside the cloud bounded by a point that is outside the cloud, not considering diagonals
        // +--+
        // |CC|
        // +--+
        // This has a perimeter of 6, an area of 2 and sides of 4

        // The shape can be odd, internal sides are still sides, consider the following
        // +------+
        // |.OOOO.|
        // |OO..OO|
        // |OO..OO|
        // |.OOOO.|
        // +------+
        // This has a perimeter of 20, an area of 16 and sides of 12

        public int Area()
        {
            return points.Count;
        }

        public int Perimeter()
        {
            return points.SelectMany(p => p.AdjacentPointsWithoutDiagonals()).Count(p => !points.Contains(p));
        }

        public int Sides()
        {
            // Find the effective bounds of the point cloud
            var bounds = Bounds;
            var sides = 0;

            // Sides to the West
            for (long x = bounds.Left; x <= bounds.Right; x++)
            {
                bool in_side = false;
                for (long y = bounds.Top; y <= bounds.Bottom; y++)
                {
                    var point = new Point(x, y);
                    if (points.Contains(point) && !points.Contains(point.West()))
                    {
                        if (!in_side)
                        {
                            in_side = true;
                        }
                    }
                    else
                    {
                        if (in_side) sides++;
                        in_side = false;

                    }
                }
                if (in_side) sides++;
            }
            // Sides to the East
            for (long x = bounds.Left; x <= bounds.Right; x++)
            {
                bool in_side = false;
                for (long y = bounds.Top; y <= bounds.Bottom; y++)
                {
                    var point = new Point(x, y);
                    if (points.Contains(point) && !points.Contains(point.East()))
                    {
                        if (!in_side)
                        {
                            in_side = true;
                        }
                    }
                    else
                    {
                        if (in_side) sides++;
                        in_side = false;

                    }
                }
                if (in_side) sides++;
            }
            // Sides to the North
            for (long y = bounds.Top; y <= bounds.Bottom; y++)
            {
                bool in_side = false;
                for (long x = bounds.Left; x <= bounds.Right; x++)
                {
                    var point = new Point(x, y);
                    if (points.Contains(point) && !points.Contains(point.North()))
                    {
                        if (!in_side)
                        {
                            in_side = true;
                        }
                    }
                    else
                    {
                        if (in_side) sides++;
                        in_side = false;
                    }
                }
                if (in_side) sides++;
            }
            // Sides to the South
            for (long y = bounds.Top; y <= bounds.Bottom; y++)
            {
                bool in_side = false;
                for (long x = bounds.Left; x <= bounds.Right; x++)
                {
                    var point = new Point(x, y);
                    if (points.Contains(point) && !points.Contains(point.South()))
                    {
                        if (!in_side)
                        {
                            in_side = true;
                        }
                    }
                    else
                    {
                        if (in_side) sides++;
                        in_side = false;
                    }
                }
                if (in_side) sides++;
            }

            return sides;
        }
    }
}
