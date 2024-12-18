
using Command.Framework;
using Command.Lib.Primitives;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Command.Problems._2024;




public partial class RAMRun : ProblemBase<int>
{
    List<Point> byteInstructions = new();
    Bounds bounds = new Bounds(Point.Zero, (6, 6));
    int cycles = 12;

    public RAMRun()
    {
    }

    protected override void Line(string line)
    {
        byteInstructions.Add(Point.Parse(line));
    }

    public override void MakeFinal()
    {
        bounds = new Bounds(Point.Zero, (70, 70));
        cycles = 1024;
    }

    public override int CalculateOne()
    {
        TextMap map = new TextMap(bounds, '.');
        foreach (var byteInstruction in byteInstructions.Take(cycles))
        {
            map.Set(byteInstruction, '#');
        }

        map.PrintMap();
        int minimumPathLength = int.MaxValue;
        MinPath(map, Point.Zero, map.Bounds.BottomRight, ref minimumPathLength);

        return minimumPathLength;
    }

    record Step(Point Point, int Distance);
    public void MinPath(TextMap map, Point startPoint, Point end, ref int minimumPathLength)
    {
        List<Step> steps = new List<Step>();
        steps.Add(new Step(startPoint, 0));
        HashSet<Point> visitedPoints = new();
        while (steps.Any())
        {
            var step = steps.OrderBy(r => r.Distance).First();
            steps.Remove(step);

            if (step.Point == end)
            {
                minimumPathLength = Math.Min(minimumPathLength, step.Distance);
                continue;
            }

            if (!map.Bounds.Contains(step.Point) || map[step.Point] == '#')
            {
                continue;
            }

            if (visitedPoints.Contains(step.Point))
            {
                continue;
            }

            visitedPoints.Add(step.Point);

            steps.Add(new Step(step.Point.South(), step.Distance + 1));
            steps.Add(new Step(step.Point.East(), step.Distance + 1));
            steps.Add(new Step(step.Point.North(), step.Distance + 1));
            steps.Add(new Step(step.Point.West(), step.Distance + 1));
        }
    }

    public override int CalculateTwo()
    {
        for (int thisCycles = cycles; thisCycles < byteInstructions.Count ; thisCycles += 1)
        {
            TextMap map = new TextMap(bounds, '.');
            foreach (var byteInstruction in byteInstructions.Take(thisCycles))
            {
                map.Set(byteInstruction, '#');
            }

            int isPathBlocked = int.MaxValue;
            MinPath(map, Point.Zero, map.Bounds.BottomRight, ref isPathBlocked);
            if (isPathBlocked == int.MaxValue)
            {
                Console.WriteLine("Last cell added at " + byteInstructions.Take(thisCycles).Last());
                return thisCycles;
            }
        }

        throw new Exception("Path is never blocked.");
    }


}
