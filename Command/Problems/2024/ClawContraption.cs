using Command.Framework;
using Command.Lib.Primitives;
using Command.Primitives;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Command.Problems._2024;

public record Rule(Point ButtonA, Point ButtonB, Point Prize)
{
    public int CostA => 3;
    public int CostB => 1;
}

public enum PressType { ButtonA, ButtonB }
public record Subset(int Cost, Point Distance);

public partial class ClawContraption : ProblemBase<ulong>
{
    List<Rule> rules = new();
    Point? buttonA;
    Point? buttonB;
    Point? prize;

    public ClawContraption()
    {
    }


    protected override void Line(string line)
    {
        var buttonAMatch = Regex.Match(line, @"Button A: X([+-]\d+), Y([+-]\d+)");
        if (buttonAMatch.Success)
        {
            buttonA = new Point(int.Parse(buttonAMatch.Groups[1].Value), int.Parse(buttonAMatch.Groups[2].Value));
        }
        var buttonBMatch = Regex.Match(line, @"Button B: X([+-]\d+), Y([+-]\d+)");
        if (buttonBMatch.Success)
        {
            buttonB = new Point(int.Parse(buttonBMatch.Groups[1].Value), int.Parse(buttonBMatch.Groups[2].Value));
        }
        var prizeMatch = Regex.Match(line, @"Prize: X=(\d+), Y=(\d+)");
        if (prizeMatch.Success)
        {
            prize = new Point(int.Parse(prizeMatch.Groups[1].Value), int.Parse(prizeMatch.Groups[2].Value));
            Debug.Assert(buttonA is not null && buttonB is not null && prize is not null);
            rules.Add(new Rule(buttonA.Value, buttonB.Value, prize.Value));
        }
        if (String.IsNullOrWhiteSpace(line))
        {
            buttonA = null;
            buttonB = null;
            prize = null;
        }
    }

    public override ulong CalculateOne()
    {
        ulong sumCost = 0;
        foreach (var rule in rules)
        {
            var minCost = ulong.MaxValue;
            Solve(rule, ref minCost);
            if (minCost != ulong.MaxValue)
            {
                sumCost += minCost;
            }
        }
        return sumCost;
    }

    void Solve(Rule rule, ref ulong minCost)
    {
        var la1 = new Line(Point.Zero, rule.ButtonA);
        var lb1 = new Line(rule.Prize - rule.ButtonB, rule.Prize);

        // Test if the lines intersect at all, and if so at what point
        var i1 = la1.Intersects(lb1);
        if (i1.i)
        {
            // Determine the length of the first and second segments in x and y
            var x1 = Math.Round(i1.x);
            var y1 = Math.Round(la1.Y_GivenX(i1.x));
            var x2 = rule.Prize.X - x1;
            var y2 = rule.Prize.Y - y1;

            // Determine the number of times the button was pressed (based on x)
            var na = (long)(x1 / rule.ButtonA.X);
            var nb = (long)(x2 / rule.ButtonB.X);

            // Do those counts get us to the prize?
            if (na * rule.ButtonA.X + nb * rule.ButtonB.X != rule.Prize.X)
            {
                return;
            }
            if (na * rule.ButtonA.Y + nb * rule.ButtonB.Y != rule.Prize.Y)
            {
                return;
            }
            
            //Console.WriteLine($"B: {rule} na={na} nb={nb}");
            minCost = Math.Min(minCost, (ulong)(na * rule.CostA + nb * rule.CostB));
        }
    }

    public override ulong CalculateTwo()
    {
        ulong sumCost = 0;
        foreach (var rule in rules)
        {
            var minCost = ulong.MaxValue;
            var newRule = new Rule(rule.ButtonA, rule.ButtonB, new Point(rule.Prize.X + 10000000000000, rule.Prize.Y + 10000000000000));
            Solve(newRule, ref minCost);
            if (minCost != ulong.MaxValue)
            {
                sumCost += minCost;
            }
        }
        return sumCost;
    }
}
