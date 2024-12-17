namespace Command.Lib.AStar;

public sealed class Step
{
    Node node;
    Edge? edge;

    public Step(Node node, Edge? edge)
    {
        this.node = node;
        this.edge = edge;
    }

    public Node Node { get { return node; } }
    public Edge? Edge { get { return edge; } }

    public bool IsTerminal { get { return this.edge == null; } }

    public override string ToString()
    {
        if (Edge == null)
        {
            return $"Node: {Node}";
        }
        else
        {
            return $"{Edge.Start} -> {Edge.End} = {Edge.Cost}";
        }
    }
}
