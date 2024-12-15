
namespace Command.Lib.Primitives;

class TextMap
{

    List<string> map = new();
    Bounds bounds = new Bounds(0, 0);

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

    public IEnumerable<char> DistinctChars()
    {
        return map.SelectMany(r => r).Select(r => r).ToHashSet();
    }
}
