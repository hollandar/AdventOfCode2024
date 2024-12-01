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
}
