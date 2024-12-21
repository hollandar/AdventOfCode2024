
using Command.Framework;
using Command.Lib.Primitives;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Command.Problems._2024;


public partial class RaceCondition : ProblemBase<int>
{
    TextMap map = new();
    public RaceCondition()
    {
    }

    protected override void Line(string line)
    {
        map.Add(line);
    }

    public record Cheat(int distance, Point cheatPoint, Point jumpTo);
    public override int CalculateOne()
    {
        var start = map.FindFirst('S').Value;
        var end = map.FindFirst('E').Value;

        Dictionary<Point, int> distanceToEnd = CalculateDistanceToEnd(start, end);

        List<Cheat> cheats = FindCheatsByForce(start, distanceToEnd);

        foreach (var saving in cheats.Select(r => r.distance).OrderBy(r => r).Distinct())
        {
            Console.WriteLine($"There are {cheats.Count(r => r.distance == saving)} cheats that save {saving} picoseconds.");
        }
        return cheats.Where(r => r.distance >= 100).Count();
    }

    private static List<Cheat> FindCheatsByForce(Point start, Dictionary<Point, int> distanceToEnd)
    {
        List<Cheat> cheats = new();

        Point[] displacements = [(-2, 0), (2, 0), (0, -2), (0, 2)];
        var endDistance = distanceToEnd[start];
        foreach (var mapPoint in distanceToEnd)
        {
            foreach (var displacement in displacements)
            {
                var point = mapPoint.Key + displacement;
                if (distanceToEnd.ContainsKey(point))
                {
                    var associatedDistanceToEnd = distanceToEnd[point];
                    var toStart = endDistance - mapPoint.Value;
                    var toEnd = associatedDistanceToEnd;

                    if (toStart + toEnd + 2 < endDistance)
                    {
                        cheats.Add(new Cheat(endDistance - (toStart + toEnd + 2), mapPoint.Key, point));
                    }
                }
            }
        }

        return cheats;
    }

    private Dictionary<Point, int> CalculateDistanceToEnd(Point start, Point end)
    {
        Dictionary<Point, int> distanceToEnd = new();
        var at = end;
        var cameFrom = at;
        var distance = 0;
        while (at != start)
        {
            distanceToEnd[at] = distance;
            var adjacentPoints = at.AdjacentPointsWithoutDiagonals();
            var nextPoint = adjacentPoints.Where(p => (map[p] == '.' || p == start) && p != cameFrom).Single();
            cameFrom = at;
            at = nextPoint;
            distance++;
        }
        distanceToEnd[start] = distance;

        return distanceToEnd;
    }

    public override int CalculateTwo()
    {
        var start = map.FindFirst('S').Value;
        var end = map.FindFirst('E').Value;

        Dictionary<Point, int> distanceToEnd = CalculateDistanceToEnd(start, end);

        List<Cheat> cheats = FindCheatsByDistance(start, distanceToEnd);

        foreach (var saving in cheats.Select(r => r.distance).OrderBy(r => r).Distinct())
        {
            Console.WriteLine($"There are {cheats.Count(r => r.distance == saving)} cheats that save {saving} picoseconds.");
        }
        return cheats.Where(r => r.distance >= 100).Count();
    }


    // Brute force is not good enough any more, instead use the step distance between pairs of known to be valid points.
    private static List<Cheat> FindCheatsByDistance(Point start, Dictionary<Point, int> distanceToEnd)
    {
        List<Cheat> cheats = new();
        int endDistance = distanceToEnd[start];

        foreach (var point in distanceToEnd)
        {
            foreach (var otherPoint in distanceToEnd)
            {
                if (point.Key == otherPoint.Key)
                {
                    continue;
                }

                var distance = point.Key.AbsoluteDistance(otherPoint.Key);
                if (distance <= 20)
                {
                    var toStart = endDistance - distanceToEnd[point.Key];
                    var toEnd = distanceToEnd[otherPoint.Key];
                    if (toStart + toEnd + distance < endDistance)
                    {
                        cheats.Add(new Cheat(endDistance - (toStart + toEnd + distance), point.Key, otherPoint.Key));
                    }
                }
            }
        }

        return cheats;
    }

}
