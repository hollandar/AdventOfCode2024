using Command.Framework;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Command.Problems._2023;


public partial class Trebuchet : ProblemBase<int>
{
    List<string> lines = new();
    public Trebuchet()
    {
    }

    protected override void Line(string line)
    {
        lines.Add(line);
    }

    public override int CalculateOne()
    {
        var sum = 0;
        foreach (var line in lines)
        {
            var leftDigit = line.AsEnumerable().First(r => char.IsDigit(r));
            var rightDigit = line.AsEnumerable().Last(r => char.IsDigit(r));
            var val = int.Parse($"{leftDigit}{rightDigit}");

            sum += val;
        }

        return sum;
    }

    static Dictionary<string, int> values = new Dictionary<string, int>
    {
        { "1", 1 },
        { "2", 2 },
        { "3", 3 },
        { "4", 4 },
        { "5", 5 },
        { "6", 6 },
        { "7", 7 },
        { "8", 8 },
        { "9", 9 },
        {"one", 1 },
        {"two", 2 },
        {"three", 3 },
        {"four", 4 },
        {"five", 5 },
        {"six", 6 },
        {"seven", 7 },
        {"eight", 8 },
        {"nine", 9 },
    };
    public override int CalculateTwo()
    {
        var sum = 0;
        foreach (var line in lines)
        {
            var first = 0;
            for (int i = 0; i < line.Length; i++)
            {
                foreach (var value in values)
                {
                    if (line.Length - i >= value.Key.Length)
                    {
                        var sub = line.Substring(i, value.Key.Length);
                        if (sub == value.Key)
                        {
                            first = value.Value;
                            goto doneFirst;
                        }
                    }
                }
            }
            doneFirst:

            var second = 0;
            for (int i = line.Length - 1; i >= 0; i--)
            {
                foreach (var value in values)
                {
                    if (line.Length - i >= value.Key.Length)
                    {
                        var sub = line.Substring(i, value.Key.Length);
                        if (sub == value.Key)
                        {
                            second = value.Value;
                            goto doneSecond;
                        }
                    }
                }
            }
            doneSecond:

            var val = (first * 10) + second;
            sum += val;
        }

        return sum;
    }


}
