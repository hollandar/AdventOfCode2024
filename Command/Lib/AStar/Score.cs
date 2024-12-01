namespace Command.Lib.AStar;

struct Score
{
    public Score()
    {
        F = long.MaxValue;
        G = long.MaxValue;
    }

    public Score(long f, long g)
    {
        F = f;
        G = g;
    }

    public long F { get; set; }
    public long G { get; set; }
}
