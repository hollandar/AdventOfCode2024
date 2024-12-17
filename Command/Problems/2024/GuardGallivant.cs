using Command.Framework;
using Command.Lib.Primitives;
using Command.Primitives;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
namespace Command.Problems._2024;

public partial class GuardGallivant : ProblemBase<int>
{
    TextMap map = new();
    Point guardStart = new Point(0, 0);
    public GuardGallivant()
    {
    }

    protected override void Line(string line)
    {
        map.Add(line);
        if (line.Contains('^'))
        {
            guardStart = new Point(line.IndexOf('^'), map.Height - 1);
        }
    }

    public override int CalculateOne()
    {
        HashSet<Point> visited = new();
        Contract.Requires(map.Bounds.Contains(guardStart));
        Facing facing = Facing.North;

        var point = guardStart;
        visited.Add(guardStart);
        while (true)
        {
            var nextPoint = Move(point, facing);

            if (!map.Bounds.Contains(nextPoint))
            {
                break;
            }

            if (map[nextPoint] == '.' || map[nextPoint] == '^')
            {
                visited.Add(nextPoint);
                point = nextPoint;
                continue;
            }

            if (map[nextPoint] == '#')
            {
                facing = TurnRight(facing);
                continue;
            }

            throw new Exception();
        }

        return visited.Count;
    }

    static bool Looping(TextMap map, Point point, Facing facing)
    {
        HashSet<Point> visited = new([point]);
        int visitedPlaces = 0;
        int breakCount = 0;
        while (true)
        {
            // If we have visited more successive sports that we have visited in total, we are looping.
            if (breakCount > visitedPlaces)
            {
                return true;
            }

            var nextPoint = Move(point, facing);

            if (!map.Bounds.Contains(nextPoint))
            {
                return false;
            }

            if (map[nextPoint] == '.' || map[nextPoint] == '^')
            {
                // If its a place we have not visited, reset the count
                if (!visited.Contains(nextPoint))
                {
                    visited.Add(nextPoint);
                    breakCount = 0;
                    visitedPlaces++;
                }

                // If we have visited it, start count successive visits to visisted places
                else
                {
                    breakCount++;
                }
                point = nextPoint;

                continue;
            }

            if (map[nextPoint] == '#')
            {
                facing = TurnRight(facing);
                continue;
            }
        }
    }

    static Point Move(Point point, Facing facing)
    {
        return facing switch
        {
            Facing.North => point.North(),
            Facing.East => point.East(),
            Facing.South => point.South(),
            Facing.West => point.West(),
            _ => throw new Exception()
        };
    }
    static Facing TurnRight(Facing facing)
    {
        return facing switch
        {
            Facing.North => Facing.East,
            Facing.East => Facing.South,
            Facing.South => Facing.West,
            Facing.West => Facing.North,
            _ => throw new Exception()
        };
    }

    public override int CalculateTwo()
    {
        var count = 0;
        HashSet<Point> visited = new();
        Debug.Assert(map.Bounds.Contains(guardStart));

        var point = guardStart;

        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                var position = new Point(x, y);
                if (position != guardStart && map[position] != '#')
                {
                    var cloneMap = map.Clone();
                    cloneMap.Set(position, '#');
                    if (Looping(cloneMap, guardStart, Facing.North))
                    {
                        count++;
                    }
                }
            }
        }

        return count;


    }

    static void PrintMap(TextMap map, HashSet<Point> visited)
    {
        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                if (visited.Contains(new Point(x, y)))
                {
                    Console.Write("X");
                }
                else
                {
                    Console.Write(map[new Point(x, y)]);
                }

            }

            Console.WriteLine();
        }
    }

}
