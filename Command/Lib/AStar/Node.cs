using Command.Lib.Primitives;

namespace Command.Lib.AStar;

public sealed class Node
{
    private Point point;
    private List<Edge> edges = new();

    public Node(Point point)
    {
        this.point = point;
    }

    public Point Point { get { return point; } }
    public List<Edge> Edges { get { return edges; } }

    public void ConnectEdge(Node otherNode, int cost)
    {
        var connection = new Edge(this, otherNode, cost);
        this.AddEdge(connection);
        otherNode.AddEdge(connection);
    }

    void AddEdge(Edge edge)
    {
        this.edges.Add(edge);
    }

    public Edge GetEdgeBetween(Node nextNode)
    {
        foreach (var edge in this.edges)
        {
            if (edge.GetOpposingNode(this) == nextNode)
                return edge;
        }

        throw new Exception();
    }
}
