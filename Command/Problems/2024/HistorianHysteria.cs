using Command.Framework;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Command.Problems._2024;


public partial class HistorianHysteria : ProblemBase<int>
{
    List<int> l1 = new List<int>();
    List<int> l2 = new List<int>();
    public HistorianHysteria()
    {
    }

    [GeneratedRegex(@"^(\d+)\s*(\d+)$", RegexOptions.Singleline)]
    private static partial Regex LineRegex();

    protected override void Line(string line)
    {
        var match = LineRegex().Match(line);
        Debug.Assert(match.Success, $"Failed to match {line}");
        l1.Add(int.Parse(match.Groups[1].Value));
        l2.Add(int.Parse(match.Groups[2].Value));
    }

    public override int CalculateOne(bool exampleData)
    {
        int cdist = 0;
        Debug.Assert(l1.Count == l2.Count);
        l1.Sort();
        l2.Sort();

        for (int i = 0; i < l1.Count; i++)
        {
            var v1 = l1[i];
            var v2 = l2[i];
            cdist += Math.Max(v1, v2) - Math.Min(v1, v2);
        }

        return cdist;
    }

    public override int CalculateTwo(bool exampleData)
    {
        int cdist = 0;
        Debug.Assert(l1.Count == l2.Count);

        for (int i = 0; i < l1.Count; i++)
        {
            var v1 = l1[i];
            var v2 = l2.Where(r => r == v1).Count();
            cdist += v1 * v2;
        }

        return cdist;
    }


}
