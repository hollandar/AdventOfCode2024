
using Command.Framework;
using Command.Lib.AStar;
using Command.Lib.Primitives;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace Command.Problems._2024;

public record Segment(Point start, Point end);

public partial class ReindeerMaze : ProblemBase<int>
{
    TextMap map = new();
    public ReindeerMaze()
    {
    }

    protected override void Line(string line)
    {
        map.Add(line);
    }

    public override int CalculateOne()
    {
        var start = map.FindFirst('S')!.Value;
        var end = map.FindFirst('E')!.Value;

        List<Step> steps = new List<Step>();
        steps.Add(new Step(start, Facing.East, 0, []));
        int minimumCost = int.MaxValue;
        Dictionary<Point, int> visited = new Dictionary<Point, int>();

        while (steps.Count > 0)
        {
            var nextStep = steps.OrderBy(s => s.cost).First();
            steps.Remove(nextStep);

            if (!openPositions.Contains(map.Get(nextStep.point, '#')))
            {
                throw new Exception("Unexpected");
            }
            if (nextStep.cost > minimumCost)
            {
                continue;
            }
            if (nextStep.point == end)
            {
                minimumCost = Math.Min(minimumCost, nextStep.cost);
                continue;
            }

            if (visited.TryGetValue(nextStep.point, out var visitedCost) && visitedCost < nextStep.cost)
            {
                continue;
            }
            visited[nextStep.point] = nextStep.cost;


            if (openPositions.Contains(map.Get(nextStep.point.North(), '#')) && nextStep.facing != Facing.South)
            {
                steps.Add(new Step(nextStep.point.North(), Facing.North, nextStep.cost + (nextStep.facing == Facing.North ? 1 : 1001), [..nextStep.path, nextStep.point]));
            }
            if (openPositions.Contains(map.Get(nextStep.point.East(), '#')) && nextStep.facing != Facing.West)
            {
                steps.Add(new Step(nextStep.point.East(), Facing.East, nextStep.cost + (nextStep.facing == Facing.East ? 1 : 1001), [.. nextStep.path, nextStep.point]));
            }
            if (openPositions.Contains(map.Get(nextStep.point.West(), '#')) && nextStep.facing != Facing.East)
            {
                steps.Add(new Step(nextStep.point.West(), Facing.West, nextStep.cost + (nextStep.facing == Facing.West ? 1 : 1001), [.. nextStep.path, nextStep.point]));
            }
            if (openPositions.Contains(map.Get(nextStep.point.South(), '#')) && nextStep.facing != Facing.North)
            {
                steps.Add(new Step(nextStep.point.South(), Facing.South, nextStep.cost + (nextStep.facing == Facing.South ? 1 : 1001), [.. nextStep.path, nextStep.point]));
            }
        }
        return minimumCost;

    }

    record Step(Point point, Facing facing, int cost, Point[] path);
    record Path(Point[] path, int cost);

    HashSet<char> openPositions = new(['S', 'E', '.']);
    public override int CalculateTwo()
    {
        var start = map.FindFirst('S')!.Value;
        var end = map.FindFirst('E')!.Value;

        List<Step> steps = new List<Step>();
        List<Path> paths = new List<Path>();
        steps.Add(new Step(start, Facing.East, 0, []));
        int minimumCost = int.MaxValue;
        Dictionary<Point, int> visited = new Dictionary<Point, int>();

        while (steps.Count > 0)
        {
            var nextStep = steps.OrderBy(s => s.cost).First();
            steps.Remove(nextStep);

            if (!openPositions.Contains(map.Get(nextStep.point, '#')))
            {
                throw new Exception("Unexpected");
            }
            if (nextStep.cost > minimumCost)
            {
                continue;
            }
            if (nextStep.point == end)
            {
                minimumCost = Math.Min(minimumCost, nextStep.cost);
                paths.Add(new Path([..nextStep.path, end], nextStep.cost));
                if (nextStep.cost > minimumCost)
                {
                    break;
                }

                continue;
            }

            if (visited.TryGetValue(nextStep.point, out var visitedCost))
            {
                if (visitedCost + 1000 < nextStep.cost)  // Short circuits visits to a single spot enough to get the right answer in a timely manner
                    continue;
            }
            else
            {
                visited[nextStep.point] = nextStep.cost;
            }


            if (openPositions.Contains(map.Get(nextStep.point.North(), '#')) && nextStep.facing != Facing.South)
            {
                steps.Add(new Step(nextStep.point.North(), Facing.North, nextStep.cost + (nextStep.facing == Facing.North? 1: 1001), [.. nextStep.path, nextStep.point]));
            }
            if (openPositions.Contains(map.Get(nextStep.point.East(), '#')) && nextStep.facing != Facing.West)
            {
                steps.Add(new Step(nextStep.point.East(), Facing.East, nextStep.cost + (nextStep.facing == Facing.East ? 1 : 1001), [.. nextStep.path, nextStep.point]));
            }
            if (openPositions.Contains(map.Get(nextStep.point.West(), '#')) && nextStep.facing != Facing.East)
            {
                steps.Add(new Step(nextStep.point.West(), Facing.West, nextStep.cost + (nextStep.facing == Facing.West ? 1 : 1001), [.. nextStep.path, nextStep.point]));
            }
            if (openPositions.Contains(map.Get(nextStep.point.South(), '#')) && nextStep.facing != Facing.North)
            {
                steps.Add(new Step(nextStep.point.South(), Facing.South, nextStep.cost + (nextStep.facing == Facing.South ? 1 : 1001), [.. nextStep.path, nextStep.point]));
            }
        }

        
        return paths.Where(p => p.cost == minimumCost).SelectMany(r => r.path).Distinct().Count();
    }


}
