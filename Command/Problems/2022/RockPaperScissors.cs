using Command.Framework;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Command.Problems._2022;

enum Play { rock, paper, scissors }
enum Outcome { win, draw, lose }
record Game(Play p1, Play p2);
record GameOutcome(Play p1, Outcome o2);

public partial class RockPaperScissors : ProblemBase<int>
{
    List<Game> games = new();
    List<GameOutcome> outcomes = new();
    public RockPaperScissors()
    {
    }

    protected override void Line(string line)
    {
        var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        Debug.Assert(parts.Length == 2);

        var p1 = parts[0] switch
        {
            "A" => Play.rock,
            "B" => Play.paper,
            "C" => Play.scissors,
            _ => throw new Exception("Invalid play")
        };

        var p2 = parts[1] switch
        {
            "X" => Play.rock,
            "Y" => Play.paper,
            "Z" => Play.scissors,
            _ => throw new Exception("Invalid play")
        };

        games.Add(new Game(p1, p2));

        var o2 = parts[1] switch
        {
            "X" => Outcome.lose,
            "Y" => Outcome.draw,
            "Z" => Outcome.win,
            _ => throw new Exception("Invalid outcome")
        };

        outcomes.Add(new GameOutcome(p1, o2));
    }

    public override int CalculateOne(bool exampleData)
    {
        int sum = 0;
        
        foreach (var game in games)
        {
            var s1 = game.p1 switch
            {
                Play.rock => 1,
                Play.paper => 2,
                Play.scissors => 3,
                _ => throw new NotImplementedException()
            };

            var s2 = game.p2 switch
            {
                Play.rock => 1,
                Play.paper => 2,
                Play.scissors => 3,
                _ => throw new NotImplementedException()
            };

            sum += s2;

            if (Win(game.p2, game.p1)) sum += 6;
            else if (Draw(game.p2,game.p1)) sum += 3;
            
        }

        return sum;
    }

    bool Win(Play p1, Play p2)
    {
        return p1 switch
        {
            Play.rock => p2 == Play.scissors,
            Play.scissors => p2 == Play.paper,
            Play.paper => p2 == Play.rock,
            _ => throw new NotImplementedException()
        };
    }

    bool Draw(Play p1, Play p2) => p1 == p2;

    public override int CalculateTwo(bool exampleData)
    {
        int sum = 0;

        foreach (var game in outcomes)
        {
            var s1 = game.p1 switch
            {
                Play.rock => 1,
                Play.paper => 2,
                Play.scissors => 3,
                _ => throw new NotImplementedException()
            };

            var p2 = game.o2 switch
            {
                Outcome.lose => Enum.GetValues<Play>().Where(v => !Win(v, game.p1) && !Draw(v, game.p1)).Single(),
                Outcome.draw => Enum.GetValues<Play>().Where(v => Draw(v, game.p1)).Single(),
                Outcome.win => Enum.GetValues<Play>().Where(v => Win(v, game.p1)).Single(),
                _ => throw new NotImplementedException()
            };

            var s2 = p2 switch
            {
                Play.rock => 1,
                Play.paper => 2,
                Play.scissors => 3,
                _ => throw new NotImplementedException()
            };

            sum += s2;

            if (Win(p2, game.p1)) sum += 6;
            else if (Draw(p2, game.p1)) sum += 3;

        }

        return sum;
    }


}
