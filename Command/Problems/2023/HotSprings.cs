using Command.Framework;
using System.Collections;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace Command.Problems._2023;

public record PipeConfiguration(string springs, int[] conditions);

public partial class HotSprings : ProblemBase<ulong>
{
    List<PipeConfiguration> pipeConfigurations = new();
    public HotSprings()
    {
    }

    [GeneratedRegex(@"^([.?#]+)\s((?:\d+|,)+)$")]
    private partial Regex LineRegex();

    protected override void Line(string line)
    {
        var match = LineRegex().Match(line);
        if (match.Success)
        {
            var springs = match.Groups[1].Value;
            var conditions = match.Groups[2].Value.Split(',').Select(int.Parse).ToArray();
            var config = new PipeConfiguration(springs, conditions);
            pipeConfigurations.Add(config);
        }
    }

    public override ulong CalculateOne(bool exampleData)
    {
        ulong count = 0;
        foreach (var configuration in pipeConfigurations)
        {
            var settable = Settable(configuration.springs);
            var offOn = OffOn(configuration.springs);

            ulong configurationCount = 0;
            DetermineConfigurations(settable, offOn, configuration.conditions, ref configurationCount);

            count += configurationCount;
        }

        return count;
    }

    BitArray Settable(string sptings)
    {
        return new BitArray(sptings.Select(r => r == '?').ToArray());
    }

    BitArray OffOn(string sptings)
    {
        return new BitArray(sptings.Select(r => r == '#').ToArray());
    }

    HashSet<string> foundConfigurations = new();
    void DetermineConfigurations(BitArray settable, BitArray offOn, int[] conditions, ref ulong configurationCount)
    {
        foundConfigurations.Clear();
        DetermineConfiguration(0, settable, offOn, conditions, ref configurationCount);
    }


    void DetermineConfiguration(int position, BitArray settable, BitArray offOn, int[] conditions, ref ulong configurationCount)
    {
        if (position == settable.Length)
        {
            if (MatchConfiguration(offOn, conditions))
            {
                configurationCount += 1;
            }

            return;
        }

        if (settable[position])
        {
            if (!MatchPartialConfiguration(offOn, position, conditions))
            {
                return;
            }

            // set it
            offOn[position] = true;
            DetermineConfiguration(position + 1, settable, offOn, conditions, ref configurationCount);

            // dont set it
            offOn[position] = false;
            DetermineConfiguration(position + 1, settable, offOn, conditions, ref configurationCount);
        }
        else
        {
            DetermineConfiguration(position + 1, settable, offOn, conditions, ref configurationCount);
        }
    }

    bool MatchConfiguration(BitArray onOff, int[] configuration)
    {
        int insideCount = 0;
        int configurationPosition = 0;
        for (int i = 0; i < onOff.Length; i++)
        {
            if (onOff[i])
            {
                insideCount++;
            }

            if (!onOff[i] && insideCount > 0)
            {
                if (configuration[configurationPosition] != insideCount)
                {
                    return false;
                }

                configurationPosition++;
                insideCount = 0;

                if (configurationPosition == configuration.Length)
                {
                    for (int j = i; j < onOff.Length; j++)
                    {
                        if (onOff[j])
                        {
                            return false;
                        }
                    };

                    return true;
                }
            }
        }

        if (configurationPosition < configuration.Length - 1)
        {
            return false;
        }

        if (configuration[configurationPosition] == insideCount)
        {
            return true;
        }

        return false;
    }

    bool MatchPartialConfiguration(BitArray onOff, int position, int[] configuration)
    {
        int insideCount = 0;
        int configurationPosition = 0;
        for (int i = 0; i < position; i++)
        {
            if (onOff[i])
            {
                insideCount++;
            }

            if (!onOff[i] && insideCount > 0)
            {
                if (configuration[configurationPosition] != insideCount)
                {
                    return false;
                }

                configurationPosition++;
                insideCount = 0;

                if (configurationPosition == configuration.Length)
                {
                    return true;
                }
            }
        }

        return true;
    }

    public override ulong CalculateTwo(bool exampleData)
    {
        ulong count = 0;
        int step = 0;
        foreach (var configuration in pipeConfigurations)
        {
            Console.WriteLine(step++);
            var newConditions = Array.Empty<int>();
            for (int i = 0; i < 5; i++)
            {
                newConditions = newConditions.Concat(configuration.conditions).ToArray();
            }
            var newSprings = String.Join("?", Enumerable.Repeat(configuration.springs, 5));
            var settable = Settable(newSprings);
            var offOn = OffOn(newSprings);

            ulong configurationCount = 0;
            DetermineConfigurations(settable, offOn, newConditions, ref configurationCount);

            count += configurationCount;
        }

        return count;
    }


}
