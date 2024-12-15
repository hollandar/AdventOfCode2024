using Command.Framework;
using Command.Lib.Primitives;
using Command.Primitives;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace Command.Problems._2024;


public partial class ResonantCollinearity : ProblemBase<int>
{
    TextMap nodes = new();

    public ResonantCollinearity()
    {
    }

    protected override void Line(string line)
    {
        nodes.Add(line);
    }

    public override int CalculateOne()
    {
        var antinodes = new TextMap(Enumerable.Repeat(new String('.', nodes.Width), nodes.Height));
        nodes.ForEach((Point p) =>
        {
            if (nodes[p] != '.')
            {
                List<Point> others = nodes.Where(nodes[p]).Where(r => p != r).ToList();

                foreach (var q in others)
                {
                    var sps = Point.SortX(p, q).ToArray();
                    var pl = sps[0];
                    var pr = sps[1];

                    var a1 = Vector.FromPoints(pl, pr).NextPoint(2);
                    var a2 = Vector.FromPoints(pr, pl).NextPoint(2);

                    antinodes.Set(a1, nodes[p]);
                    antinodes.Set(a2, nodes[p]);
                }
            }
        });

        return antinodes.Where(c => c != '.').Count();
    }

    public override int CalculateTwo()
    {
        var antinodes = new TextMap(Enumerable.Repeat(new String('.', nodes.Width), nodes.Height));
        nodes.ForEach((Point p) =>
        {
            if (nodes[p] != '.')
            {
                List<Point> others = nodes.Where(nodes[p]).Where(r => p != r).ToList();

                foreach (var q in others)
                {
                    var sps = Point.SortX([p, q]).ToArray();
                    var pl = sps[0];
                    var pr = sps[1];

                    var scalar = 2;
                    Point a1;
                    Point a2;
                    do
                    {
                        a1 = Vector.FromPoints(pl, pr).NextPoint(scalar);
                        a2 = Vector.FromPoints(pr, pl).NextPoint(scalar);

                        antinodes.Set(a1, nodes[p]);
                        antinodes.Set(a2, nodes[p]);

                        scalar++;

                    } while (nodes.Bounds.Contains(a1) || nodes.Bounds.Contains(a2));

                    antinodes.Set(pl, nodes[p]);
                    antinodes.Set(pr, nodes[p]);
                }
            }
        });

        return antinodes.Where(c => c != '.').Count();
    }

    private static void PrintNodes(TextMap nodes, TextMap antinodes)
    {
        Console.WriteLine();
        for (int y = 0; y < nodes.Height; y++)
        {
            for (int x = 0; x < nodes.Width; x++)
            {
                var point = (x, y);
                if (antinodes[point] != '.')
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                Console.Write(nodes[point]);
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();

        }
    }
}
