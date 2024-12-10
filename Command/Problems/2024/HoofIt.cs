
using Command.Framework;
using Command.Lib.Primitives;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Command.Problems._2024;


public partial class HoofIt : ProblemBase<int>
{
    TextMap map = new();
    public HoofIt()
    {
    }

    protected override void Line(string line)
    {
        map.Add(line);
    }

    int ToHeight(char c) => (int)(c - '0');

    public override int CalculateOne()
    {
        int count = 0;
        var trailHeads = map.Where('0').ToList();
        foreach (var trailHead in trailHeads)
        {
            HashSet<Point> reachable = new();
            TraverseDistinct(trailHead, 0, reachable);
            count += reachable.Count;
        }

        return count;
    }

    void TraverseDistinct(Point point, int height, HashSet<Point> reachable)
    {
        if (!map.Bounds.Contains(point))
        {
            return;
        }

        var thisHeight = ToHeight(map[point]);
        if (thisHeight != height)
        {
            return;
        }

        if (thisHeight == 9)
        {
            reachable.Add(point);
            return;
        }

        TraverseDistinct(point.North(), height + 1, reachable);
        TraverseDistinct(point.West(), height + 1, reachable);
        TraverseDistinct(point.East(), height + 1, reachable);
        TraverseDistinct(point.South(), height + 1, reachable);
    }

    public override int CalculateTwo()
    {
        int count = 0;
        var trailHeads = map.Where('0').ToList();
        foreach (var trailHead in trailHeads)
        {
            int rating = 0;
            TraverseRating(trailHead, 0, ref rating);
            count += rating;
        }

        return count;
    }

    void TraverseRating(Point point, int height, ref int count)
    {
        if (!map.Bounds.Contains(point))
        {
            return;
        }

        var thisHeight = ToHeight(map[point]);
        if (thisHeight != height)
        {
            return;
        }

        if (thisHeight == 9)
        {
            count++;
            return;
        }

        TraverseRating(point.North(), height + 1, ref count);
        TraverseRating(point.West(), height + 1, ref count);
        TraverseRating(point.East(), height + 1, ref count);
        TraverseRating(point.South(), height + 1, ref count);
    }


}
