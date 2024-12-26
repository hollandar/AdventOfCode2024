using Command.Framework;

namespace Command.Problems._2024;

public record KeyCombination(char a, char b, string sequence)
{
    public static implicit operator KeyCombination((char a, char b, string sequence) key) => new KeyCombination(key.a, key.b, key.sequence);
}


public partial class KeypadConundrum : ProblemBase<long>
{
    static List<KeyCombination> directionCombinations = new List<KeyCombination> {
        ('A', '>', "vA"),
        ('A', 'v', "v<A"), // ?
        ('A', 'v', "<vA"), // ?
        ('A', '<', "v<<A"),
        ('A', '<', "<v<A"),
        ('A', '^', "<A"),
        ('A', 'A', "A"),
        ('^', '>', "v>A"), // ?
        ('^', '>', ">vA"), // ?
        ('^', 'v', "vA"),
        ('^', '<', "v<A"),
        ('^', '^', "A"),
        ('^', 'A', ">A"),
        ('>', '>', "A"),
        ('>', 'v', "<A"),
        ('>', '<', "<<A"),
        ('>', '^', "<^A"), // ?
        ('>', '^', "^<A"), // ?
        ('>', 'A', "^A"),
        ('v', '>', ">A"),
        ('v', 'v', "A"),
        ('v', '<', "<A"),
        ('v', '^', "^A"),
        ('v', 'A', "^>A"), // ?
        ('v', 'A', ">^A"), // ?
        ('<', '>', ">>A"),
        ('<', 'v', ">A"),
        ('<', '<', "A"),
        ('<', '^', ">^A"),
        ('<', 'A', ">>^A"),
        ('<', 'A', ">^>A"),
    };

    static List<KeyCombination> keypadCombinations = new List<KeyCombination> {
        ('A', 'A', "A"),
        ('A', '0', "<A"),
        ('A', '1', "^<<A"),
        ('A', '1', "<^<A"),
        //('A', '2', "^<A"),
        ('A', '3', "^A"),
        ('A', '4', "^^<<A"),
        ('A', '4', "^<<^A"),
        ('A', '4', "^<^<A"),
        ('A', '4', "<^<^A"),
        ('A', '5', "<^^A"),
        ('A', '5', "^^<A"),
        ('A', '5', "^<^A"),
        ('A', '6', "^^A"),
        //('A', '7', "^^^<<A"),
        //('A', '8', "^^^<A"),
        ('A', '9', "^^^A"),
        ('0', 'A', ">A"),
        //('0', '0', "A"),
        //('0', '1', "^<A"),
        ('0', '2', "^A"),
        //('0', '3', "^>A"),
        //('0', '4', "^^<A"),
        //('0', '5', "^^A"),
        //('0', '6', "^^>A"),
        //('0', '7', "^^^<A"),
        //('0', '8', "^^^A"),
        //('0', '9', "^^^>A"),
        //('1', 'A', ">>vA"),
        //('1', '0', ">A"),
        //('1', '1', "A"),
        //('1', '2', ">A"),
        //('1', '3', ">>A"),
        //('1', '4', "^A"),
        //('1', '5', "^>A"),
        //('1', '6', ">>^A"),
        ('1', '7', "^^A"),
        //('1', '8', "^^>A"),
        ('1', '9', ">>^^A"),
        ('1', '9', "^^>>A"),
        ('1', '9', "^>^>A"),
        ('1', '9', ">^>^A"),
        //('2', 'A', "v>A"),
        //('2', '0', "vA"),
        //('2', '1', "<A"),
        //('2', '2', "A"),
        //('2', '3', ">A"),
        //('2', '4', "^<A"),
        //('2', '5', "^A"),
        //('2', '6', "^>A"),
        //('2', '7', "^^<A"),
        //('2', '8', "^^A"),
        ('2', '9', "^^>A"),
        ('2', '9', ">^^A"),
        ('2', '9', "^>^A"),
        //('3', 'A', "vA"),
        //('3', '0', "v<A"),
        ('3', '1', "<<A"),
        //('3', '2', "<A"),
        //('3', '3', "A"),
        ('3', '4', "<<^A"),
        ('3', '4', "^<<A"),
        ('3', '4', "<^<A"),
        //('3', '5', "^<A"),
        //('3', '6', "^A"),
        ('3', '7', "<<^^A"),
        ('3', '7', "^^<<A"),
        ('3', '7', "^<^<A"),
        ('3', '7', "<^<^A"),
        //('3', '8', "^^<A"),
        //('3', '9', "^^A"),
        ('4', 'A', ">>vvA"),
        ('4', 'A', ">vv>A"),
        ('4', 'A', "v>v>A"),
        ('4', 'A', ">v>vA"),
        //('4', '0', ">vvA"),
        //('4', '1', "vA"),
        //('4', '2', "v>A"),
        //('4', '3', ">>vA"),
        //('4', '4', "A"),
        ('4', '5', ">A"),
        //('4', '6', ">>A"),
        //('4', '7', "^A"),
        //('4', '8', "^>A"),
        ('4', '9', "^>>A"),
        ('4', '9', ">>^A"),
        ('4', '9', ">^>A"),
        //('5', 'A', "vv>A"),
        //('5', '0', "vvA"),
        //('5', '1', "v<A"),
        //('5', '2', "vA"),
        //('5', '3', "v>A"),
        //('5', '4', "<A"),
        //('5', '5', "A"),
        ('5', '6', ">A"),
        //('5', '7', "^<A"),
        ('5', '8', "^A"),
        //('5', '9', "^>A"),
        ('6', 'A', "vvA"),
        //('6', '0', "vv<A"),
        //('6', '1', "v<<A"),
        //('6', '2', "v<A"),
        //('6', '3', "vA"),
        ('6', '4', "<<A"),
        //('6', '5', "<A"),
        //('6', '6', "A"),
        ('6', '7', "<<^A"),
        ('6', '7', "^<<A"),
        ('6', '7', "<^<A"),
        //('6', '8', "^<A"),
        //('6', '9', "^A"),
        //('7', 'A', ">>vvvA"),
        ('7', '0', ">vvvA"),
        ('7', '0', "vv>vA"),
        ('7', '0', "v>vvA"),
        //('7', '1', "vvA"),
        //('7', '2', ">vvA"),
        //('7', '3', ">>vvA"),
        //('7', '4', "vA"),
        //('7', '5', ">vA"),
        //('7', '6', "v>>A"),
        //('7', '7', "A"),
        //('7', '8', ">A"),
        ('7', '9', ">>A"),
        //('8', 'A', ">vvvA"),
        ('8', '0', "vvvA"),
        //('8', '1', "vv<A"),
        //('8', '2', "vvA"),
        //('8', '3', ">vvA"),
        //('8', '4', "v<A"),
        //('8', '5', "vA"),
        ('8', '6', "v>A"),
        ('8', '6', ">vA"),
        //('8', '7', "<A"),
        //('8', '8', "A"),
        //('8', '9', ">A"),
        ('9', 'A', "vvvA"),
        //('9', '0', "vvv<A"),
        //('9', '1', "vv<<A"),
        //('9', '2', "vv<A"),
        //('9', '3', "vvA"),
        //('9', '4', "v<<A"),
        //('9', '5', "v<A"),
        ('9', '6', "vA"),
        //('9', '7', "<<A"),
        ('9', '8', "<A"),
        //('9', '9', "A"),
    };

    List<string> codes { get; set; } = new();
    public KeypadConundrum()
    {
    }

    protected override void Line(string line)
    {
        codes.Add(line);
    }

    public IEnumerable<KeyCombination> ExpandDirection(char from, char to)
    {
        return directionCombinations.Where(r => r.a == from && r.b == to);
    }

    public IEnumerable<KeyCombination> ExpandKeypad(char from, char to)
    {
        return keypadCombinations.Where(r => r.a == from && r.b == to);
    }

    public IEnumerable<(char first, char second)> Pairs(string s)
    {
        for (int i = 0; i < s.Length - 1; i++)
        {
            yield return (s[i], s[i + 1]);
        }
    }


    public IEnumerable<string> SplitAt(string c, char n)
    {
        var mem = c.AsMemory();
        int pi = 0;
        for (int i = 1; i < mem.Length; i++)
        {
            if (mem.Span[i] == n)
            {
                yield return mem.Slice(pi, i - pi + 1).ToString();
                pi = i + 1;
            }

        }
    }

    public string Concat(IEnumerable<string> strings)
    {
        return String.Join("", strings);
    }

    public IEnumerable<string> ExpandKeypad(string keys)
    {
        var pairs = Pairs(keys);
        var combinations = ExpandKeypad(String.Empty, pairs);
        return combinations;
    }

    public IEnumerable<string> ExpandKeypad(string content, IEnumerable<(char a, char b)> pairs)
    {
        if (!pairs.Any())
        {
            yield return content;
        }
        else
        {
            var pair = pairs.First();
            foreach (var combination in ExpandKeypad(pair.a, pair.b))
            {
                var results = ExpandKeypad(content + combination.sequence, pairs.Skip(1));
                foreach (var result in results) yield return result;
            }
        }

    }

    Dictionary<(string, int), long> memo = new();
    public long ExpandSegment(string instructions, int depth)
    {
        if (memo.ContainsKey((instructions, depth)))
        {
            return memo[(instructions, depth)];
        }

        if (depth == 0)
        {
            //Console.Write(instructions);
            return instructions.Length;
        }

        var segments = SplitAt(instructions, 'A');
        long length = 0;
        foreach (var segment in segments)
        {
            var subInstructions = ExpandInstructions(segment);
            long minLength = long.MaxValue;

            foreach (var sub in subInstructions)
            {
                var subLength = ExpandSegment(sub, depth - 1);
                minLength = Math.Min(minLength, subLength);
            }

            length += minLength;

        }

        //memo[(instructions, depth)] = length;
        return length;
    }

    public IEnumerable<string> ExpandInstructions(string instruction)
    {
        var pairs = Pairs(instruction);
        IEnumerable<string> results = ExpandDirection('A', instruction[0]).Select(r => r.sequence).ToList();
        foreach (var pair in pairs)
        {
            var interim = ExpandDirection(pair.first, pair.second).Select(r => r.sequence).ToList();
            results = Combine(results, interim);
        }

        return results;
    }

    public IEnumerable<string> Combine(IEnumerable<string> a, IEnumerable<string> b)
    {
        foreach (var aa in a)
        {
            foreach (var bb in b)
            {
                yield return aa + bb;
            }
        }
    }

    public override long CalculateOne(bool exampleData)
    {
        long length = 0;
        foreach (var code in codes)
        {
            var expanded = ExpandKeypad("A" + code);
            var minCodeLength = long.MaxValue;
            foreach (var expand in expanded)
            {
                var codeLength = ExpandSegment(expand, 2);
                minCodeLength = Math.Min(minCodeLength, codeLength);
            }
            Console.WriteLine($"{code} {minCodeLength}");
            var numeric = long.Parse(code.Substring(0, 3));
            length += minCodeLength * numeric;
        }

        return length;
    }

    public override long CalculateTwo(bool exampleData)
    {
        long length = 0;
        foreach (var code in codes)
        {
            var expanded = ExpandKeypad("A" + code);
            var minCodeLength = long.MaxValue;
            foreach (var expand in expanded)
            {
                var codeLength = ExpandSegment(expand, 3);
                minCodeLength = Math.Min(minCodeLength, codeLength);
            }
            Console.WriteLine($"{code} {minCodeLength}");
            var numeric = long.Parse(code.Substring(0, 3));
            length += minCodeLength * numeric;
        }

        return length;
    }
}

