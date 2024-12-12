
using Command.Framework;
using Command.Lib.Extensions;

namespace Command.Problems._2024;


// Three important things to note:
// If we store each outcome for future processing (say in a queue) we will run out of memory or time
// Only the second rule adds to the length of the output
// If you recalculate every stone, it will take too long, so stones at specific cycle depths are memoized

// Also tried a queued search, and a linked list approach, both too slow to calculate part 2


public partial class PlutonianPebbles : ProblemBase<long>
{
    List<long> stones = new List<long>();
    public PlutonianPebbles()
    {
    }

    protected override void Line(string line)
    {
        stones = line.Split(" ").Select(long.Parse).ToList();
    }

    public override long CalculateOne()
    {
        int cycles = 25;
        long count = 0;
        var memo = new Dictionary<(long, int), long>();
        foreach (var stone in stones)
        {
            count += 1;  // Count the stone itself
            CycleStone(stone, cycles, ref count, memo);
        }

        return count;
    }

    private void CycleStone(long stone, int remainingCycles, ref long count, Dictionary<(long stone, int remainingCycles), long> memo)
    {
        if (remainingCycles > 0)
        {
            // Check if we have seen this before, if we have, we use the memo outcome
            if (memo.ContainsKey((stone, remainingCycles)))
            {
                count += memo[(stone, remainingCycles)];
                return;
            }

            // Calculate the impact of this stone, and recursively calculate the depth below up to the number of cycles
            var nextStones = StepStone(stone);
            long thisCount = Math.Max(0, nextStones.Length - 1);

            long childrenCount = 0;
            foreach (var nextStone in nextStones)
            {
                CycleStone(nextStone, remainingCycles - 1, ref childrenCount, memo);
            }
            count += childrenCount + thisCount;

            // Memoize the output at this cycle for use later
            memo[(stone, remainingCycles)] = childrenCount + thisCount;
        }
    }



    private long[] StepStone(long stone)
    {
        var digits = stone.SignificantDigits();
        if (stone == 0)
        {
            // Rule 1 - If the stone is engraved with the number 0, it is replaced by a stone engraved with the number 1.
            return [1];
        }
        else if ((digits % 2) == 0)
        {
            // Rule 2 - If the stone is engraved with a number that has an even number of digits, it is replaced by two stones.
            // The left half of the digits are engraved on the new left stone, and the right half of the digits are engraved on the new right stone.
            // (The new numbers don't keep extra leading zeroes: 1000 would become stones 10 and 0.)
            var left = (int)(stone / Math.Pow(10, (digits / 2)));
            var right = (int)(stone % Math.Pow(10, (digits / 2)));
            return [left, right];
        }
        else
        {
            // Rule 3 - If none of the other rules apply, the stone is replaced by a new stone; the old stone's number multiplied by 2024 is engraved on the new stone.
            return [stone * 2024];
        }
    }

    public override long CalculateTwo()
    {
        int cycles = 75;
        long count = 0;
        var memo = new Dictionary<(long, int), long>();
        foreach (var stone in stones)
        {
            count += 1;  // Count the stone itself
            CycleStone(stone, cycles, ref count, memo);
        }

        return count;
    }


}
