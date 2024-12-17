namespace Command.Lib.AStar;

public sealed class Edge
{
    public Edge(Node from, Node to, int cost)
    {
        this.Cost = cost;
        this.Start = from;
        this.End = to;
    }

    public int Cost { get; }
    public Node Start { get; }
    public Node End { get; }

    public Node GetOpposingNode(Node thisNode)
    {
        if (Start == thisNode) return End;
        if (End == thisNode) return Start;
        throw new Exception("Not connected");
    }

    public override string ToString()
    {
        return $"{Start.Point} -> {End.Point} = {Cost}";
    }


}
