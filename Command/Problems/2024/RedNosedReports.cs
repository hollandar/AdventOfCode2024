using Command.Framework;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Command.Problems._2024;

public record Report(int[] levels);

public partial class RedNosedReports : ProblemBase<int>
{
    List<Report> reports = new();

    public RedNosedReports()
    {
    }

    protected override void Line(string line)
    {
        var levels = line.Split(' ').Select(int.Parse).ToArray();
        reports.Add(new Report(levels));
    }

    public bool IsSafe(int[] levels)
    {
        int[] directions = new int[levels.Length - 1];
        int[] climbs = new int[levels.Length - 1];

        for (int i = 0; i < levels.Length - 1; i++)
        {
            var climbValue = levels[i + 1] - levels[i];
            directions[i] = climbValue switch
            {
                > 0 => 1,
                < 0 => -1,
                _ => 0
            };
            climbs[i] = Math.Abs(climbValue);
        }

        return climbs.All(c => c > 0 && c <= 3) && (directions.All(d => d == 1) || directions.All(d => d == -1));
    }

    public override int CalculateOne(bool exampleData)
    {
        int count = 0;
        foreach (var report in reports)
        {
            if (IsSafe(report.levels))
            {
                count++;
            }
        }

        return count;
    }

    
    public override int CalculateTwo(bool exampleData)
    {
        int count = 0;
        foreach (var report in reports)
        {
            if (IsSafe(report.levels))
            {
                count++;
                continue;
            }

            for (int i = 0; i < report.levels.Length; i++)
            {
                int j = i + 1;
                int[] levels = (report.levels[..i]).Concat(report.levels[j..]).ToArray();
                if (IsSafe(levels))
                {
                    count++;
                    goto next;
                }
            }

        next:
            continue;
        }

        return count;
    }


}
