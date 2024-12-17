
using Command.Framework;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;

namespace Command.Problems._2024;


public partial class ChronospatialComputer : ProblemBase<string>
{
    Dictionary<string, long> registers = new();
    List<int> instructions = new();
    public ChronospatialComputer()
    {
    }

    protected override void Line(string line)
    {
        var mathRegister = Regex.Match(line, @"Register ([A-Z]): (\d+)");
        if (mathRegister.Success)
        {
            registers[mathRegister.Groups[1].Value] = int.Parse(mathRegister.Groups[2].Value);
        }

        var programCode = Regex.Match(line, @"Program: (.*)");
        if (programCode.Success)
        {
            instructions = programCode.Groups[1].Value.Split(",").Select(r => int.Parse(r.Trim())).ToList();
        }
    }

    public override string CalculateOne()
    {
        List<long> output = new();
        RunVM(output);

        return String.Join(",", output);
    }

    private void RunVM(List<long> output)
    {
        int programCounter = 0;
        while (programCounter < instructions.Count)
        {
            var instruction = instructions[programCounter];
            long literal = instructions[programCounter + 1];
            long combo = instructions[programCounter + 1] switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                4 => registers["A"],
                5 => registers["B"],
                6 => registers["C"],
                _ => 0
            };

            switch (instruction)
            {
                case 0: // adv
                    {
                        registers["A"] = (long)Math.Truncate(registers["A"] / Math.Pow(2, combo));
                    }
                    break;
                case 1: // bxl
                    {
                        registers["B"] = registers["B"] ^ literal;
                    }
                    break;
                case 2: // bst
                    {
                        registers["B"] = (combo % 8);
                    }
                    break;
                case 3: // jnz
                    if (registers["A"] != 0)
                    {
                        programCounter = (int)literal;
                        continue;
                    }
                    break;
                case 4: // bxc
                    {
                        registers["B"] = registers["B"] ^ registers["C"];
                    }
                    break;
                case 5: // out
                    {
                        output.Add(combo % 8);
                    }
                    break;
                case 6: // bdv
                    {
                        registers["B"] = (long)Math.Truncate(registers["A"] / Math.Pow(2, combo));
                    }
                    break;
                case 7: // cdv
                    {
                        registers["C"] = (long)Math.Truncate(registers["A"] / Math.Pow(2, combo));
                    }
                    break;
            }

            programCounter += 2;
        }
    }

    bool final = false;
    public override void MakeFinal() => final = true;
    public override string CalculateTwo()
    {
        if (!final) return "Not final";

        // Running the VM with numbers from 0 up helps you determine the step change in values from left to right
        // Powers of 8 affect bits in groups of 3 due without affecting bits in lower or higher groups

        // 2,4,1,3,7,5,1,5,0,3,4,2,5,5,3,0
        // 0|  1  |  4  |  7  |  10 |  13 |

        // They are incremented from group 5 to group 0, from largest steps in a to smallest
        var strides = new long[] { 13, 10, 7, 4, 1, 0 };
        var multipliers = new long[] { 0, 0, 0, 0, 0, 0 };
        List<long> output = new();
        {
            for (int i = 0; ; i++)
            {
                output.Clear();
                registers["A"] = strides.Zip(multipliers).Select(r => (long)(Math.Pow(8, r.First) * r.Second)).Sum();
                RunVM(output);
                if (output.Count < 16) multipliers[0]++;
                else
                if (output[15] != 0 || output[14] != 3 || output[13] != 5) multipliers[0]++;
                else
                if (output[12] != 5 || output[11] != 2 || output[10] != 4) multipliers[1]++;
                else
                if (output[9] != 3 || output[8] != 0 || output[7] != 5) multipliers[2]++;
                else
                if (output[6] != 1 || output[5] != 5 || output[4] != 7) multipliers[3]++;
                else
                if (output[3] != 3 || output[2] != 1 || output[1] != 4) multipliers[4]++;
                else
                if (output[0] != 2) multipliers[5]++;
                else
                    break;
            }
        }

        return String.Join(",", output) + " " + strides.Zip(multipliers).Select(r => (long)(Math.Pow(8, r.First) * r.Second)).Sum();
    }

}
