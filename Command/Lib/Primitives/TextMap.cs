
namespace Command.Lib.Primitives;

class TextMap
{

    List<string> map = new();
    Bounds bounds = Bounds.Zero;

    public TextMap()
    {
    }

    public TextMap(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            Add(line);
        }
    }

    public void Add(string line)
    {
        map.Add(line);
        bounds = new Bounds(new Point(0, 0), new Point(Width - 1, Height - 1));
    }

    public int Width => map.First().Length;
    public int Height => map.Count;

    public List<string> Rows => map;

    public Bounds Bounds => bounds;
    public char this[Point p] => map[(int)p.Y][(int)p.X];
    public TextMap Clone()
    {
        var clonedMap = new TextMap();
        foreach (var line in map)
        {
            clonedMap.Add(line);
        }

        return clonedMap;
    }
    public void Set(Point p, char c)
    {
        if (Bounds.Contains(p))
        {
            var line = map[(int)p.Y].ToCharArray();
            line[(int)p.X] = c;
            map[(int)p.Y] = new String(line);
        }
    }

    public char Get(Point p) => this[p];

    public void ForEach(Action<Point> action)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                action(new Point(x, y));
            }
        }
    }

    public IEnumerable<Point> Where(Func<char, bool> f)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (f(this[new Point(x, y)]))
                {
                    yield return new Point(x, y);
                }
            }
        }
    }

    public IEnumerable<Point> Where(char c)
    {
        return Where(x => x == c);
    }

    public Point? FindFirst(char c)
    {
        for (long y = Bounds.Top; y <= bounds.Bottom; y++)
        {
            for (long x = Bounds.Left; x <= bounds.Right; x++)
            {
                if (this[(x,y)] == c)
                {
                    return (x, y);
                }
            }
        }

        return null;
    }
    
    public IEnumerable<Point> Points()
    {
        for (int y = 0; y < bounds.Height; y++)
            for (int x = 0; x < bounds.Width; x++)
                yield return new Point(x, y);
    }

    public void PrintMap()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Console.Write(this[new Point(x, y)]);
            }
            Console.WriteLine();
        }
    }

    // Flood fill within the map over elements with the same character, starting at some starting position
    // Covers all adjacent points, including diagonals
    // Pass (p) => p.AdjacentPointsWithoutDiagonals() to only cover cardinal directions
    public IEnumerable<Point> Flood(Point startingPoint, Func<Point, IEnumerable<Point>>? points = null)
    {
        if ( points == null)
        {
            points = (point) => point.AdjacentPoints(Bounds);   
        }

        char target = this[startingPoint];
        var visited = new HashSet<Point>();
        var toVisit = new Queue<Point>();
        toVisit.Enqueue(startingPoint);
        while (toVisit.Count > 0)
        {
            var current = toVisit.Dequeue();
            if (visited.Contains(current))
            {
                continue;
            }
            visited.Add(current);
            if (this[current] == target)
            {
                yield return current;
                var adjacent = points(current);
                foreach (var adj in adjacent)
                {
                    toVisit.Enqueue(adj);
                }
            }
        }
    }

    public IEnumerable<char> DistinctChars()
    {
        return map.SelectMany(r => r).Select(r => r).ToHashSet();
    }
}
