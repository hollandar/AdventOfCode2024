using Command.Framework;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Command.Problems._2024;


public partial class MullItOver: ProblemBase<int>
{
    List<string> instructions = new();
    public MullItOver()
    {
    }

    [GeneratedRegex(@"(mul\(\d+,\d+\)|do\(\)|don\'t\(\))")]
    public partial Regex InstructionsRegex();

    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    public partial Regex MulRegex();

    [GeneratedRegex(@"do\(\)")]
    public partial Regex DoRegex();

    [GeneratedRegex(@"don\'t\(\)")]
    public partial Regex DontRegex();

    protected override void Line(string line)
    {
        var matches = InstructionsRegex().Matches(line);
        var newInstructions = matches.Select(m => m.Groups[1].Value).ToList();
        instructions.AddRange(newInstructions);
    }

    public override int CalculateOne(bool exampleData)
    {
        return instructions
            .Where(r => MulRegex().IsMatch(r))
            .Select(r => MulRegex().Match(r))
            .Select(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value))
            .Sum();
    }

    public override int CalculateTwo(bool exampleData)
    {
        bool enabled = true;
        int sum = 0;

        foreach (var instruction in instructions)
        {
            if (DoRegex().IsMatch(instruction))
            {
                enabled = true;
            }
            else if (DontRegex().IsMatch(instruction))
            {
                enabled = false;
            }
            else if (enabled && MulRegex().IsMatch(instruction))
            {
                var match = MulRegex().Match(instruction);
                var a = int.Parse(match.Groups[1].Value);
                var b = int.Parse(match.Groups[2].Value);
                sum += a * b;
            }
        }

        return sum;
    }


}
