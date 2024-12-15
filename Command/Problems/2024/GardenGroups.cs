
using Command.Framework;
using Command.Lib.Primitives;

namespace Command.Problems._2024;

public class GardenGroup(char type, params IEnumerable<Point> points)
{
    public char Type => type;
    public PointCloud Points { get; } = new(points);

    public int Area() => Points.Area();
    public int Perimeter() => Points.Perimeter();
    public int Sides() => Points.Sides();
    public bool Contains(Point p) => Points.Contains(p);
}

public partial class GardenGroups : ProblemBase<int>
{
    TextMap map = new TextMap();

    public GardenGroups()
    {
    }

    protected override void Line(string line)
    {
        map.Add(line);
    }

    public override int CalculateOne()
    {
        List<GardenGroup> groups = GetGroups();

        int count = 0;
        foreach (var group in groups)
        {
            count += group.Area() * group.Perimeter();
        }

        return count;
    }

    private List<GardenGroup> GetGroups()
    {
        List<GardenGroup> groups = new();
        foreach (var point in map.Points())
        {
            var c = map[point];

            var covered = false;
            foreach (var group in groups)
            {
                if (group.Type == c && group.Points.Contains(point))
                {
                    covered = true;
                    break;
                }
            }
            if (covered)
            {
                continue;
            }
            else
            {
                var points = map.Flood(point, (point) => point.AdjacentPointsWithoutDiagonals(map.Bounds));
                var group = new GardenGroup(c, points);
                groups.Add(group);
            }
        }

        return groups;
    }

    public override int CalculateTwo()
    {
        List<GardenGroup> groups = GetGroups();

        int count = 0;
        foreach (var group in groups)
        {
            count += group.Area() * group.Sides();
        }

        return count;
    }

}

