using Command.Framework;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
using System.Text.RegularExpressions;

namespace Command.Problems._2023;

enum Color { red, green, blue }
record Play(Color color, int count);
record Subset(List<Play> plays);
record Game(int id, List<Subset> Subsets);

public partial class CubeConundrum : ProblemBase<int>
{
    List<Game> gameRecords = new();

    public CubeConundrum()
    {
    }

    [GeneratedRegex(@"^Game (\d+):\s(?<games>.*)$", RegexOptions.Singleline)]
    private static partial Regex LineRegex();

    [GeneratedRegex(@"^\s?(\d+)\s(red|green|blue)$", RegexOptions.Singleline)]
    private static partial Regex PlayRegex();

    protected override void Line(string line)
    {
        var match = LineRegex().Match(line);
        if (match.Success)
        {
            List<Subset> subsetRecords = new();
            var subsets = match.Groups["games"].Value.Split(";", StringSplitOptions.RemoveEmptyEntries);
            foreach (var subset in subsets)
            {
                var games = subset.Split(",", StringSplitOptions.RemoveEmptyEntries);
                List<Play> subsetPlays = new();
                foreach (var game in games)
                {
                    var playMatch = PlayRegex().Match(game);
                    Debug.Assert(playMatch.Success, $"Failed to match {game}");

                    var playRecord = new Play(Enum.Parse<Color>(playMatch.Groups[2].Value), int.Parse(playMatch.Groups[1].Value));
                    subsetPlays.Add(playRecord);
                }
                subsetRecords.Add(new Subset(subsetPlays));
            }

            gameRecords.Add(new Game(int.Parse(match.Groups[1].Value), subsetRecords));
        }
    }

    public override int CalculateOne()
    {
        var sum = 0;
        foreach (var game in gameRecords)
        {
            foreach (var subset in game.Subsets)
            {
                var redCount = subset.plays.Where(r => r.color == Color.red).Sum(r => r.count);
                var greenCount = subset.plays.Where(r => r.color == Color.green).Sum(r => r.count);
                var blueCount = subset.plays.Where(r => r.color == Color.blue).Sum(r => r.count);
                if (redCount > 12) goto nextGame;
                if (greenCount > 13) goto nextGame;
                if (blueCount > 14) goto nextGame;
            }

            sum += game.id;

        nextGame:
            continue;

        }

        return sum;
    }

    public override int CalculateTwo()
    {
        var sum = 0;


        foreach (var game in gameRecords)
        {
            var minRed = 0;
            var minGreen = 0;
            var minBlue = 0;
            foreach (var subset in game.Subsets)
            {
                var redCount = subset.plays.Where(r => r.color == Color.red).Sum(r => r.count);
                var greenCount = subset.plays.Where(r => r.color == Color.green).Sum(r => r.count);
                var blueCount = subset.plays.Where(r => r.color == Color.blue).Sum(r => r.count);
                minRed = Math.Max(minRed, redCount);
                minGreen = Math.Max(minGreen, greenCount);
                minBlue = Math.Max(minBlue, blueCount);
            }

            var power = minRed * minGreen * minBlue;
            sum += power;
        }

        return sum;
    }


}
