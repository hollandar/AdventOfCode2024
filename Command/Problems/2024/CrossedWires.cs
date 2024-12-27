using Command.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Command.Problems._2024;

enum Op { AND, OR, XOR }
record Gate(string a, string b, Op op, string o)
{
    public override string ToString()
    {
        return $"{a} {op} {b} -> {o}";
    }
}
public partial class CrossedWires : ProblemBase<long>
{
    List<Gate> gates = new();
    Dictionary<string, bool> wires = new();
    public CrossedWires()
    {
    }

    protected override void Line(string line)
    {
        var wireMatch = Regex.Match(line, @"^(.+?): ([01])$");
        if (wireMatch.Success)
        {
            wires.Add(wireMatch.Groups[1].Value, wireMatch.Groups[2].Value == "1");
        }

        var gateMatch = Regex.Match(line, @"^(.+?) (AND|OR|XOR) (.+?) -> (.+)$");
        if (gateMatch.Success)
        {
            var a = gateMatch.Groups[1].Value;
            var b = gateMatch.Groups[3].Value;
            var op = gateMatch.Groups[2].Value switch
            {
                "AND" => Op.AND,
                "OR" => Op.OR,
                "XOR" => Op.XOR,
                _ => throw new NotImplementedException()
            };
            var o = gateMatch.Groups[4].Value;
            gates.Add(new Gate(a, b, op, o));
        }
    }

    public override long CalculateOne(bool exampleData)
    {
        Execute(wires, gates);

        var outputs = wires.Where(r => r.Key.StartsWith("z")).Select(r => new { line = int.Parse(r.Key.Substring(1)), value = r.Value });
        var answer = outputs.Select(r => ((r.value ? (long)1 : (long)0) << r.line)).Sum();

        return answer;
    }
    void WriteChildren(Gate gate, int c = 5)
    {
        var children = gates.Where(r => r.a == gate.o || r.b == gate.o);
        foreach (var child in children)
        {
            Console.Write(String.Join("", Enumerable.Repeat(" ", c)));
            Console.WriteLine(child);
            WriteChildren(child, c + 5);
        }

    }

    public override long CalculateTwo(bool exampleData)
    {

        return default;
    }

    private void Execute(Dictionary<string, bool> wires, List<Gate> gates)
    {
        var gateList = gates.ToList();
        while (gateList.Any())
        {
            bool step = false;
            for (int i = 0; i < gateList.Count; i++)
            {

                var gate = gateList[i];

                if (!wires.ContainsKey(gate.a) || !wires.ContainsKey(gate.b))
                    continue;

                var a = wires[gate.a];
                var b = wires[gate.b];
                var result = gate.op switch
                {
                    Op.AND => And(a, b),
                    Op.OR => Or(a, b),
                    Op.XOR => Xor(a, b),
                    _ => throw new NotImplementedException()
                };
                wires[gate.o] = result;

                gateList.Remove(gate);
                step = true;
                break;
            }

            if (!step) throw new Exception("No step");
        }
    }

    bool And(bool a, bool b) => a && b;
    bool Or(bool a, bool b) => a || b;
    bool Xor(bool a, bool b) => a != b;

    Dictionary<string, bool> CreateWires(long x, long y, int c)
    {
        Dictionary<string, bool> wires = new();
        for (int i = 0; i < c; i++)
        {
            wires[$"x{i:00}"] = (x & (1 << i)) != 0;
        }
        for (int i = 0; i < c; i++)
        {
            wires[$"y{i:00}"] = (y & (1 << i)) != 0;
        }

        return wires;

    }

    string ToBinary(long n)
    {
        return Convert.ToString(n, 2).PadLeft(45, '0');
    }
}
