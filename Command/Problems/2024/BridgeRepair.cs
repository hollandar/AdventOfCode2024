using Command.Framework;
using System.Diagnostics;
using System.Net.Mail;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Command.Problems._2024;

enum Operator { Plus, Multiply, Concatenate }
record Equation(ulong testValue, ulong[] numbers)
{
    public override string ToString()
    {
        return $"{testValue}: {string.Join(' ', numbers)}";
    }
}

public partial class BridgeRepair : ProblemBase<ulong>
{
    List<Equation> equations = new();

    public BridgeRepair()
    {
    }

    protected override void Line(string line)
    {
        var parts = line.Split(':');
        var testValue = ulong.Parse(parts[0]);
        var numbers = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToArray();
        this.equations.Add(new Equation(testValue, numbers));
    }

    public override ulong CalculateOne()
    {
        ulong sum = 0;
        foreach (var eq in equations)
        {
            if (Calculate(eq.testValue, eq.numbers, [Operator.Plus]))
            {
                sum += eq.testValue;
            }

            else if (Calculate(eq.testValue, eq.numbers, [Operator.Multiply]))
            {
                sum += eq.testValue;
            }
        }

        return sum;
    }

    static bool Calculate(ulong testValue, ulong[] numbers, Operator[] op)
    {
        Debug.Assert(numbers.Length > 0);
        if (op.Length == numbers.Length - 1)
        {
            ulong lvalue = numbers[0];

            for (int i = 0; i < op.Length; i++)
            {
                switch (op[i])
                {
                    case Operator.Plus:
                        lvalue += numbers[i + 1];
                        break;
                    case Operator.Multiply:
                        lvalue *= numbers[i + 1];
                        break;
                    default:
                        throw new Exception();
                }
            }

            return lvalue == testValue;
        }

        var plusResult = Calculate(testValue, numbers, [.. op, Operator.Plus]);
        var multiplyResult = Calculate(testValue, numbers, [.. op, Operator.Multiply]);

        return plusResult || multiplyResult;
    }

    static bool CalculateConcat(ulong testValue, ulong[] numbers, Operator[] op)
    {
        Debug.Assert(numbers.Length > 0);
        if (op.Length == numbers.Length - 1)
        {
            ulong lvalue = (ulong)numbers[0];
            for (int i = 0; i < op.Length; i++)
            {

                switch (op[i])
                {
                    case Operator.Plus:
                        lvalue += (ulong)numbers[i+1];
                        break;
                    case Operator.Multiply:
                        lvalue *= (ulong)numbers[i+1];
                        break;
                    case Operator.Concatenate:
                        lvalue = ulong.Parse($"{lvalue}{numbers[i + 1]}");
                        break;
                    default:
                        throw new Exception();
                }
            }

            return lvalue == testValue;
        }

        var plusResult = CalculateConcat(testValue, numbers, [.. op, Operator.Plus]);
        var multiplyResult = CalculateConcat(testValue, numbers, [.. op, Operator.Multiply]);
        var concatResult = CalculateConcat(testValue, numbers, [.. op, Operator.Concatenate]);

        return plusResult || multiplyResult || concatResult;
    }

    public override ulong CalculateTwo()
    {
        ulong sum = 0;
        foreach (var eq in equations)
        {
            if (CalculateConcat(eq.testValue, eq.numbers, [Operator.Plus]))
            {
                sum += eq.testValue;
            }

            else if (CalculateConcat(eq.testValue, eq.numbers, [Operator.Multiply]))
            {
                sum += eq.testValue;
            }

            else if (CalculateConcat(eq.testValue, eq.numbers, [Operator.Concatenate]))
            {
                sum += eq.testValue;
            }
        }

        return sum;
    }


}
