using Command.Primitives;

namespace Command.Lib.AStar;

public sealed class DirectedPath
{
    List<Step> steps;

    public DirectedPath(List<Step>? steps = null)
    {
        this.steps = steps ?? new List<Step>();
    }

    public IEnumerable<Edge> GetEdges()
    {
        foreach (var step in steps)
            if (step.Edge != null)
                yield return step.Edge;
    }

    public IEnumerable<Node> GetNodes()
    {
        if (steps.Any())
        {
            foreach (var step in steps)
            {
                yield return step.Edge!.Start;
            }

            yield return steps.Last().Edge!.End;
        }
    }

    public Point[] Points => GetNodes().Select(n => n.Point).ToArray();

    public string ToPointsString()
    {
        string pointsString = "";
        foreach (var node in GetNodes())
            pointsString += node.Point;
        return pointsString;
    }

    public bool Incomplete { get { return steps.Count == 0; } }
    public int NodeCount { get { return steps.Count + 2; } }
    public int EdgeCount { get { return steps.Count; } }
}
