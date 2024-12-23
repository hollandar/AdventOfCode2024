using Command.Framework;
using Command.Lib.Primitives;

namespace Command.Problems._2024;


public partial class CeresSearch : ProblemBase<int>
{
    List<string> _list = new List<string>();
    public CeresSearch()
    {
    }

    protected override void Line(string line)
    {
        _list.Add(line);
    }

    public override int CalculateOne(bool exampleData)
    {
        var count = 0;
        Bounds bounds = new Bounds(new Point(0, 0), new Point(_list.First().Length - 1, _list.Count - 1));
        for (int x = (int)bounds.TopLeft.X; x <= bounds.BottomRight.X; x++)
        {
            for (int y = (int)bounds.TopLeft.Y; y <= bounds.BottomRight.Y; y++)
            {

                if (_list[y][x] == 'X')
                {
                    var point = new Point(x, y);

                    count += Search("XMAS", 0, bounds, point, (p) => p.East()) ? 1 : 0;
                    count += Search("XMAS", 0, bounds, point, (p) => p.West()) ? 1 : 0;
                    count += Search("XMAS", 0, bounds, point, (p) => p.North()) ? 1 : 0;
                    count += Search("XMAS", 0, bounds, point, (p) => p.South()) ? 1 : 0;
                    count += Search("XMAS", 0, bounds, point, (p) => p.East().North()) ? 1 : 0;
                    count += Search("XMAS", 0, bounds, point, (p) => p.East().South()) ? 1 : 0;
                    count += Search("XMAS", 0, bounds, point, (p) => p.West().North()) ? 1 : 0;
                    count += Search("XMAS", 0, bounds, point, (p) => p.West().South()) ? 1 : 0;
                }
            }
        }

        return count;
    }

    protected bool Search(string word, int index, Bounds bounds, Point point, Func<Point, Point> move)
    {
        if (index == word.Length || !bounds.Contains(point))
        {
            return false;
        }
        var cell = _list[(int)point.Y][(int)point.X];
        if (cell == word[index])
        {
            if (index == word.Length - 1)
            {
                return true;
            }

            var nextMove = move(point);
            return Search(word, index + 1, bounds, nextMove, move);
        }

        return false;
    }

    public override int CalculateTwo(bool exampleData)
    {
        Bounds bounds = new Bounds(new Point(0, 0), new Point(_list.First().Length - 1, _list.Count - 1));
        var atPoint = (Point p, char[] c) =>
        {
            if (!bounds.Contains(p)) return false;
            return c.Contains(_list[(int)p.Y][(int)p.X]);
        };

        var count = 0;
        for (int x = (int)bounds.TopLeft.X; x <= bounds.BottomRight.X; x++)
        {
            for (int y = (int)bounds.TopLeft.Y; y <= bounds.BottomRight.Y; y++)
            {
                var point = new Point(x, y);

                if (!atPoint(point, ['A'])) continue;
                if (!bounds.Contains(point.NorthWest())) continue;
                if (!bounds.Contains(point.SouthWest())) continue;
                if (!bounds.Contains(point.NorthEast())) continue;
                if (!bounds.Contains(point.SouthEast())) continue;

                if (!atPoint(point.NorthWest(), ['M', 'S']) || !atPoint(point.SouthWest(), ['M', 'S'])) continue;

                if (atPoint(point.NorthWest(), ['M', 'S']))
                {
                    if (atPoint(point.NorthWest(), ['M']) && !atPoint(point.SouthEast(), ['S'])) continue;
                    if (atPoint(point.NorthWest(), ['S']) && !atPoint(point.SouthEast(), ['M'])) continue;
                }

                if (atPoint(point.SouthWest(), ['M', 'S']))
                {
                    if (atPoint(point.SouthWest(), ['M']) && !atPoint(point.NorthEast(), ['S'])) continue;
                    if (atPoint(point.SouthWest(), ['S']) && !atPoint(point.NorthEast(), ['M'])) continue;
                }
                count++;
            }
        }

        return count;
    }
}
