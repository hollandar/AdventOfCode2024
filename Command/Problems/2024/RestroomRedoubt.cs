
using Command.Framework;
using Command.Lib.Primitives;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Command.Problems._2024;

public class Robot(Point position, Point velocity)
{
    public Point Position { get; set; } = position;
    public Point Velocity { get; set; } = velocity;

    public void Move() => Position += Velocity;
    public void SetPosition(Point position) => Position = position;
    public override string ToString()
    {
        return $"{Position} {Velocity}";
    }
}

public partial class RestroomRedoubt : ProblemBase<int>
{
    List<Robot> robots = new();
    Bounds bounds = Bounds.Zero;
    public RestroomRedoubt()
    {
    }

    protected override void Line(string line)
    {
        var match = Regex.Match(line, @"p=([+-]?\d+),([+-]?\d+) v=([+-]?\d+),([+-]?\d+)");
        if (match.Success)
        {
            var p = new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
            var v = new Point(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
            robots.Add(new Robot(p, v));
        }
    }

    public override void MakeExample() => bounds = new Bounds(new Point(0, 0), new Point(10, 6));
    public override void MakeFinal() => bounds = new Bounds(new Point(0, 0), new Point(100, 102));
    public override int CalculateOne()
    {
        for (int i = 0; i < 100; i++)
        {
            foreach (var robot in robots)
            {
                robot.Move();
                robot.SetPosition(bounds.WrapTo(robot.Position));

            }
        }


        var xx = (int)(bounds.Width / 2);
        var yy = (int)(bounds.Height / 2);

        var f1 = new Bounds(bounds.TopLeft, new Point(xx - 1, yy - 1)).Inside(robots.Select(r => r.Position)).Count();
        var f2 = new Bounds(new Point(xx + 1, bounds.TopLeft.Y), new Point(bounds.BottomRight.X, yy - 1)).Inside(robots.Select(r => r.Position)).Count();
        var f3 = new Bounds(new Point(bounds.TopLeft.X, yy + 1), new Point(xx - 1, bounds.BottomRight.Y)).Inside(robots.Select(r => r.Position)).Count();
        var f4 = new Bounds(new Point(xx + 1, yy + 1), bounds.BottomRight).Inside(robots.Select(r => r.Position)).Count();

        return f1 * f2 * f3 * f4;
    }

    public override int CalculateTwo()
    {
        if (bounds.Width < 50) return 0;


        for (int i = 1; ; i += 1)
        {
            foreach (var r in robots)
            {
                r.Move();
                r.SetPosition(bounds.WrapTo(r.Position));
            }
            var points = robots.Select(r => r.Position).OrderBy(r => r.Y).ThenBy(r => r.X).ToList();
            var longestSubset = 0;
            var thisSubset = 0;
            Point? lastPoint = null;
            foreach (var p in points)
            {
                if (lastPoint.HasValue && lastPoint.Value.Y == p.Y && lastPoint.Value.X == p.X - 1)
                {
                    thisSubset++;
                    longestSubset = Math.Max(thisSubset, longestSubset);
                }
                else
                {
                    thisSubset = 0;
                }
                lastPoint = p;
            }
            if (
                longestSubset > 5
            )
            {
                Console.WriteLine(i);
                bounds.Print(p => robots.Where(r => r.Position == p).Count() switch
                {
                    0 => '.',
                    int count => (char)('0' + count)
                });
                Console.ReadLine();
            }

        }

        return -1;

    }
}

