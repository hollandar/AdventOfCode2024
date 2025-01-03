﻿
using Command.Framework;
using Command.Lib.Alg;
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

    public override int CalculateOne(bool exampleData)
    {
        if (!exampleData)
        {
            bounds = new Bounds(Point.Zero, (70, 70));
            cycles = 1024;
        }

        TextMap map = new TextMap(bounds, '.');
        foreach (var byteInstruction in byteInstructions.Take(cycles))
        {
            map.Set(byteInstruction, '#');
        }

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

    public override int CalculateTwo(bool exampleData)
    {
        var search = new BinarySearch(0, byteInstructions.Count - 2);
        var ix = search.Find((cycles) =>
        {
            TextMap map = new TextMap(bounds, '.');
            foreach (var byteInstruction in byteInstructions.Take(cycles))
            {
                map.Set(byteInstruction, '#');
            }
            int pathLength = int.MaxValue;
            MinPath(map, Point.Zero, map.Bounds.BottomRight, ref pathLength);

            map.Set(byteInstructions.Skip(cycles).First(), '#');
            int nextPathLength = int.MaxValue;
            MinPath(map, Point.Zero, map.Bounds.BottomRight, ref nextPathLength);


            return
            pathLength != int.MaxValue && nextPathLength == int.MaxValue ? 0 :
            pathLength == int.MaxValue && nextPathLength == int.MaxValue ? -1 : 1;
        });

        if (ix == -1) throw new Exception("No cross over found.");

        var lastCell = byteInstructions.Skip(ix).First();
        Console.WriteLine("Last cell added at " + lastCell);
        return ix;
    }


}
