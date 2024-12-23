using Command.Framework;
using System.Diagnostics;
using System.Runtime.Intrinsics.Wasm;
using System.Text.RegularExpressions;

namespace Command.Problems._2022;

record Pack(int calories);
record Elf(List<Pack> packs);

public partial class CalorieCounting: ProblemBase<int>
{
    List<Elf> elves = new();
    public CalorieCounting()
    {
    }

    protected override void Line(string line)
    {
        if (String.IsNullOrEmpty(line))
        {
            elves.Add(new Elf([]));
            return;
        }

        if (elves.Count == 0)
        {
            elves.Add(new Elf([new Pack(int.Parse(line))]));
            return;
        }

        elves[^1].packs.Add(new Pack(int.Parse(line)));
    }

    public override int CalculateOne(bool exampleData)
    {
        return elves.Select(r => r.packs.Sum(r => r.calories)).Max();
    }

    public override int CalculateTwo(bool exampleData)
    {
        return elves.Select(r => r.packs.Sum(s => s.calories)).OrderByDescending(r => r).Take(3).Sum();
    }


}
