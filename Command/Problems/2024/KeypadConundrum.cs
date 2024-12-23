using Command.Framework;
using Command.Lib.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using System.Transactions;

namespace Command.Problems._2024;

enum KeyAction : short { Up, Down, Left, Right, Press }

public partial class KeypadConundrum : ProblemBase<long>
{
    List<string> codes { get; set; } = new();
    TextMap codePad = new();
    TextMap dirPad = new();
    public KeypadConundrum()
    {
        codePad.Add("789");
        codePad.Add("456");
        codePad.Add("123");
        codePad.Add(" 0A");

        dirPad.Add(" ^A");
        dirPad.Add("<v>");
    }

    protected override void Line(string line)
    {
        codes.Add(line);
    }


    public override long CalculateOne(bool exampleData)
    {
        int cycles = 2;
        long count = 0;
        foreach (var code in codes)
        {
            IEnumerable<KeyAction> finalPath = Array.Empty<KeyAction>();
            var current = 'A';
            foreach (var c in code)
            {
                var actions = Paths(codePad, current, c);
                var actionCounts = actions.Select(r => (r, ExpandAndCountMemo(r, cycles, null)));
                var min = actionCounts.Min(r => r.Item2);
                var thisPath = actionCounts.Where(r => r.Item2 == min).Select(r => r.r).First();
                finalPath = finalPath.Concat(thisPath);
                current = c;
            }

            KeyAction[] expansion = finalPath.ToArray();
            for (int i = cycles; i > 0; i--)
            {
                expansion = Expand(expansion).ToArray();
            }
            var outcome = expansion.LongLength;

            var builder = new StringBuilder();
            var keypad = new Keypad(c => builder.Append(c));
            var directionPad1 = new DirectionPad(keypad);
            var directionPad2 = new DirectionPad(directionPad1);
            new KeypadPlayer(directionPad2).Play(PathToString(expansion));
            Debug.Assert(builder.ToString() == code);

            var numeric = int.Parse(code.Substring(0, 3));
            count += numeric * outcome;
            Console.WriteLine($"'{code}': {outcome} * {numeric}");
        }
        return count;
    }

    public override long CalculateTwo(bool exampleData)
    {
        var cycles = 25;
        long count = 0;
        foreach (var code in codes)
        {
            IEnumerable<KeyAction> finalPath = Array.Empty<KeyAction>();
            var current = 'A';
            foreach (var c in code)
            {
                var actions = Paths(codePad,current, c);
                var actionCounts = actions.Select(r => (r, ExpandAndCountMemo(r, cycles, null)));
                var min = actionCounts.Min(r => r.Item2);
                var thisPath = actionCounts.Where(r => r.Item2 == min).Select(r => r.r).First();
                finalPath = finalPath.Concat(thisPath);
                current = c;
            }

            KeyAction[] expansion = finalPath.ToArray();
            var outcome = ExpandAndCountMemo(expansion, cycles, null);

            var numeric = int.Parse(code.Substring(0, 3));
            count += (long)(numeric) * outcome;
            Console.WriteLine($"'{code}': {outcome} * {numeric}");
        }

        return count;
    }

    string PathToString(IEnumerable<KeyAction> actions)
    {
        var builder = new StringBuilder();
        foreach (var a in actions)
        {
            builder.Append(a switch
            {
                KeyAction.Up => '^',
                KeyAction.Down => 'v',
                KeyAction.Left => '<',
                KeyAction.Right => '>',
                KeyAction.Press => 'A',
                _ => throw new Exception()

            });
        }

        return builder.ToString();

    }

    IEnumerable<KeyAction> Expand(IEnumerable<KeyAction> actions)
    {
        var actionsEnumerator = actions.GetEnumerator();
        var last = KeyAction.Press;
        while (actionsEnumerator.MoveNext())
        {
            var action = actionsEnumerator.Current;
            switch (last, action)
            {
                case (KeyAction.Press, KeyAction.Up):
                    yield return KeyAction.Left;
                    yield return KeyAction.Press;
                    break;

                case (KeyAction.Press, KeyAction.Down):
                    yield return KeyAction.Left;
                    yield return KeyAction.Down;
                    yield return KeyAction.Press;
                    break;

                case (KeyAction.Press, KeyAction.Left):
                    yield return KeyAction.Down;
                    yield return KeyAction.Left;
                    yield return KeyAction.Left;
                    yield return KeyAction.Press;
                    break;

                case (KeyAction.Press, KeyAction.Right):
                    yield return KeyAction.Down;
                    yield return KeyAction.Press;
                    break;

                case (KeyAction.Up, KeyAction.Press):
                    yield return KeyAction.Right;
                    yield return KeyAction.Press;
                    break;

                case (KeyAction.Up, KeyAction.Down):
                    yield return KeyAction.Down;
                    yield return KeyAction.Press;
                    break;

                case (KeyAction.Up, KeyAction.Left):
                    yield return KeyAction.Down;
                    yield return KeyAction.Left;
                    yield return KeyAction.Press;
                    break;

                case (KeyAction.Up, KeyAction.Right):
                    yield return KeyAction.Down;
                    yield return KeyAction.Right;
                    yield return KeyAction.Press;
                    break;

                case (KeyAction.Down, KeyAction.Press):
                    yield return KeyAction.Right;
                    yield return KeyAction.Up;
                    yield return KeyAction.Press;
                    break;

                case (KeyAction.Down, KeyAction.Up):
                    yield return KeyAction.Up;
                    yield return KeyAction.Press;
                    break;

                case (KeyAction.Down, KeyAction.Left):
                    yield return KeyAction.Left;
                    yield return KeyAction.Press;
                    break;

                case (KeyAction.Down, KeyAction.Right):
                    yield return KeyAction.Right;
                    yield return KeyAction.Press;
                    break;

                case (KeyAction.Left, KeyAction.Press):
                    yield return KeyAction.Right;
                    yield return KeyAction.Right;
                    yield return KeyAction.Up;
                    yield return KeyAction.Press;
                    break;

                case (KeyAction.Left, KeyAction.Up):
                    yield return KeyAction.Right;
                    yield return KeyAction.Up;
                    yield return KeyAction.Press;
                    break;

                case (KeyAction.Left, KeyAction.Down):
                    yield return KeyAction.Right;
                    yield return KeyAction.Press;
                    break;

                case (KeyAction.Left, KeyAction.Right):
                    yield return KeyAction.Right;
                    yield return KeyAction.Right;
                    yield return KeyAction.Press;
                    break;

                case (KeyAction.Right, KeyAction.Press):
                    yield return KeyAction.Up;
                    yield return KeyAction.Press;
                    break;

                case (KeyAction.Right, KeyAction.Up):
                    yield return KeyAction.Left;
                    yield return KeyAction.Up;
                    yield return KeyAction.Press;
                    break;

                case (KeyAction.Right, KeyAction.Down):
                    yield return KeyAction.Left;
                    yield return KeyAction.Press;
                    break;

                case (KeyAction.Right, KeyAction.Left):
                    yield return KeyAction.Left;
                    yield return KeyAction.Left;
                    yield return KeyAction.Press;
                    break;

                case (var a, var b) when a == b:
                    yield return KeyAction.Press;
                    break;


                default:
                    throw new Exception("Unknown action.");
            }
            last = action;
        }
    }

    Dictionary<(string, int), long> memo = new();

    long ExpandAndCountMemo(KeyAction[] actions, int count, KeypadPlayer? player)
    {
        memo.Clear();
        return ExpandAndCount(actions, count, player);
    }

    long ExpandAndCount(KeyAction[] actions, int count, KeypadPlayer player)
    {
        var key = PathToString(actions);

        if (count == 0)
        {
            Debug.Assert(actions.Last() == KeyAction.Press);
            if (player != null) player.Play(key);
            return actions.Length;
        }

        if (memo.ContainsKey((key, count)))
        {
            return memo[(key, count)];
        }

        var expanded = Expand(actions).ToArray();
        var expandedKey = PathToString(expanded);

        var blocks = SplitIntoBlocks(expanded);
        long result = 0;
        foreach (var block in blocks)
        {
            result += ExpandAndCount(block, count - 1, player);
        }

        memo[(key, count)] = result;

        return result;
    }

    IEnumerable<KeyAction[]> SplitIntoBlocks(IEnumerable<KeyAction> actions)
    {
        List<KeyAction> block = new();
        var enumerator = actions.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var action = enumerator.Current;
            if (action == KeyAction.Press)
            {
                block.Add(action);
                yield return block.ToArray();
                block.Clear();
            }
            else
            {
                block.Add(action);
            }
        }
    }

    record Step(Point point, int distance, KeyAction[] actions);
    private IEnumerable<KeyAction[]> Paths(TextMap map, char start, char end)
    {
        var startPoint = map.FindFirst(start).Value;
        var endPoint = map.FindFirst(end).Value;

        var queue = new Queue<Step>();
        queue.Enqueue(new Step(startPoint, 0, []));
        while (queue.Any())
        {
            var current = queue.Dequeue();
            if (current.distance > startPoint.AbsoluteDistance(endPoint)) continue;
            if (!map.Bounds.Contains(current.point) || map[current.point] == ' ') continue;
            if (current.point == endPoint)
            {
                yield return [.. current.actions, KeyAction.Press];
            }
            else
            {
                queue.Enqueue(new Step(current.point.North(), current.distance + 1, [.. current.actions, KeyAction.Up]));
                queue.Enqueue(new Step(current.point.South(), current.distance + 1, [.. current.actions, KeyAction.Down]));
                queue.Enqueue(new Step(current.point.West(), current.distance + 1, [.. current.actions, KeyAction.Left]));
                queue.Enqueue(new Step(current.point.East(), current.distance + 1, [.. current.actions, KeyAction.Right]));
            }
        }
    }

}

public interface IKeypad
{
    void Up();
    void Down();
    void Left();
    void Right();
    void Press();
}

class Keypad : IKeypad
{
    TextMap map = new();
    Point point;
    private Action<char> output;

    public Keypad(Action<char> output)
    {
        map.Add("789");
        map.Add("456");
        map.Add("123");
        map.Add(" 0A");
        point = map.FindFirst('A').Value;
        this.output = output;
    }
    public void Down()
    {
        if (map.Bounds.Contains(point.South()) && map[point.South()] != ' ') point = point.South();
        else throw new Exception();
    }
    public void Up()
    {
        if (map.Bounds.Contains(point.North()) && map[point.North()] != ' ') point = point.North();
        else throw new Exception();
    }
    public void Left()
    {
        if (map.Bounds.Contains(point.West()) && map[point.West()] != ' ') point = point.West();
        else throw new Exception();
    }
    public void Right()
    {
        if (map.Bounds.Contains(point.East()) && map[point.East()] != ' ') point = point.East();
        else throw new Exception();
    }
    public void Press()
    {
        output(map[point]);
    }
}

class DirectionPad : IKeypad
{
    TextMap map = new();
    Point point;
    IKeypad childPad;
    public DirectionPad(IKeypad childPad)
    {
        map.Add(" ^A");
        map.Add("<v>");
        point = map.FindFirst('A').Value;
        this.childPad = childPad;
    }

    public void Down()
    {
        if (map.Bounds.Contains(point.South()) && map[point.South()] != ' ') point = point.South();
        else throw new Exception();
    }
    public void Up()
    {
        if (map.Bounds.Contains(point.North()) && map[point.North()] != ' ') point = point.North();
        else throw new Exception();
    }
    public void Left()
    {
        if (map.Bounds.Contains(point.West()) && map[point.West()] != ' ') point = point.West();
        else throw new Exception();
    }
    public void Right()
    {
        if (map.Bounds.Contains(point.East()) && map[point.East()] != ' ') point = point.East();
        else throw new Exception();
    }
    public void Press()
    {
        if (map[point] == '^') childPad.Up();
        else if (map[point] == 'v') childPad.Down();
        else if (map[point] == '>') childPad.Right();
        else if (map[point] == '<') childPad.Left();
        else if (map[point] == 'A') childPad.Press();
        else
        {
            throw new Exception();
        }
    }
}

class KeypadPlayer(IKeypad keypad)
{
    public void Play(string code)
    {
        foreach (var c in code)
        {
            switch (c)
            {
                case '^': keypad.Up(); break;
                case 'v': keypad.Down(); break;
                case '<': keypad.Left(); break;
                case '>': keypad.Right(); break;
                case 'A': keypad.Press(); break;
                default: throw new Exception();
            }
        }
    }
}
