
using Command.Framework;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text.RegularExpressions;

namespace Command.Problems._2024;


public partial class LinenLayout : ProblemBase<long>
{
    HashSet<string> patterns = new();
    HashSet<string> towels = new();

    public LinenLayout()
    {
    }

    protected override void Line(string line)
    {
        if (line.Contains(','))
        {
            line.Split(',').ToList().ForEach(x => patterns.Add(x.Trim()));
        }
        else if (!String.IsNullOrWhiteSpace(line))
        {
            towels.Add(line);
        }
    }


    public override long CalculateOne(bool exampleData)
    {
        var patternLengths = patterns.Select(r => r.Length).Distinct().OrderByDescending(r => r).ToArray();
        var designCount = 0;

        foreach (var design in towels)
        {
            var queue = new Queue<int>();
            queue.Enqueue(0);

            while (queue.Count > 0)
            {
                var position = queue.Dequeue();

                if (position== design.Length)
                {
                    designCount++;
                    break;
                }

                foreach (var length in patternLengths)
                {
                    if (length > design.Length - position)
                    {
                        continue;
                    }

                    var remainingDesign = design.Substring(position, length);
                    if (patterns.Contains(remainingDesign))
                    {
                        var startNewDesign = position + length;
                        if (!queue.Any(existingPosition => existingPosition == startNewDesign))
                            queue.Enqueue(startNewDesign);
                    }
                }
            }
        }

        return designCount;
    }

    public override long CalculateTwo(bool exampleData)
    {
        // Recursion is too slow.
        // Approach instead is to find out how many tokens are valid at each location and sum up the paths to the end of the string based on those valid tokens.
        long count = 0;
        foreach (var design in towels)
        {
            var tokensAtPosition = new List<string>[design.Length];
            for (int i = 0; i < design.Length; i++)
            {
                tokensAtPosition[i] = new();
                var token = design.Substring(i);
                foreach (var pattern in patterns)
                {
                    if (token.StartsWith(pattern))
                    {
                        tokensAtPosition[i].Add(pattern);
                    }
                }
            }

            long[] pathsTo = Enumerable.Repeat<long>(0, design.Length + 1).ToArray();
            pathsTo[0] = 1;

            for (int i = 0; i < design.Length; i++)
            {
                foreach (var token in tokensAtPosition[i])
                {
                    if (i + token.Length < design.Length + 1)
                        pathsTo[i + token.Length] += pathsTo[i];
                }
            }

            count += pathsTo[design.Length];
        }

        return count;
    }

}
