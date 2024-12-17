using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Command.Lib.AStar;

public static class PathFinder
{
    public static DirectedPath FindPath(Node start, Node end)
    {
        HashSet<Node> openSet = new HashSet<Node> { start };
        HashSet<Node> closedSet = new HashSet<Node>();
        Dictionary<Node, Node> cameFrom = new();
        Dictionary<Node, Score> scores = new();
        scores[start] = new Score { G = 0 };

        while (openSet.Count > 0)
        {
            var current = openSet.Select(r => new { r = r, s = scores[r] }).OrderBy(r => r.s.F).Select(r => r.r).First();

            if (current == end)
            {
                List<Step> steps = new List<Step>();
                var currentStep = end;
                do
                {
                    var from = cameFrom[currentStep];
                    steps.Insert(0, new Step(from, from.GetEdgeBetween(currentStep)));
                    currentStep = from;
                }
                while (currentStep != start);

                return new DirectedPath(steps);

            }

            openSet.Remove(current);
            closedSet.Add(current);
            foreach (var edge in current.Edges)
            {
                var otherNode = edge.End;
                if (closedSet.Contains(otherNode))
                {
                    continue;
                }

                if (!scores.ContainsKey(otherNode))
                {
                    scores[otherNode] = new Score();
                }

                var d = edge.Cost;
                var tentative_g = scores[current].G + d;

                if (tentative_g < scores[otherNode].G)
                {
                    cameFrom[otherNode] = current;
                    scores[otherNode] = new Score(tentative_g, tentative_g + edge.Cost);
                    if (!openSet.Contains(otherNode))
                        openSet.Add(otherNode);
                }
            }
        }

        return new DirectedPath(null);
    }
}
