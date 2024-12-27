using Command.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Command.Problems._2024;

public partial class MonkeyMarket : ProblemBase<long>
{
    List<long> secrets = new();
    public MonkeyMarket()
    {
    }

    protected override void Line(string line)
    {
        secrets.Add(long.Parse(line));
    }

    public override long CalculateOne(bool exampleData)
    {
        long count = 0;
        foreach (var secret in secrets)
        {
            long newSecret = secret;
            for (int i = 0; i < 2000; i++)
            {
                newSecret = NextSecret(newSecret);
            }
            count += newSecret;
        }
        return count;
    }

    public override long CalculateTwo(bool exampleData)
    {
        long count = 0;
        Dictionary<int, List<(long price, long change)>> changes = new();
        for (int i = 0; i < secrets.Count; i++)
        {
            long last = 0;
            var secret = secrets[i];
            for (int j = 0; j < 2001; j++) // 2000 price changes + 1 initial price
            {
                var next = Lsd(secret);
                var x = next - last;
                if (!changes.ContainsKey(i))
                {
                    changes[i] = new([(next, x)]);
                }
                else
                {
                    changes[i].Add((next, x));
                }


                secret = NextSecret(secret);
                last = next;
            }

        }

        Dictionary<(long c1, long c2, long c3, long c4), long> scores = new();
        foreach (var player in changes.Keys)
        {
            var change = changes[player];
            HashSet<(long c1, long c2, long c3, long c4)> keys = new();
            for (int i = 4; i < change.Count; i++)
            {
                var key = (change[i - 3].change, change[i - 2].change, change[i - 1].change, change[i].change);
                if (!keys.Contains(key))
                {
                    keys.Add(key);
                    if (!scores.ContainsKey(key))
                        scores[key] = change[i].price;
                    else
                        scores[key] += change[i].price;
                }
            }

        }

        var maxScore = scores.Max(r => r.Value);

        return maxScore;
    }

    long NextSecret(long secret)
    {
        secret = Prune(Mix(secret, secret * 64));
        secret = Prune(Mix(secret, (long)Math.Floor(secret / 32f)));
        return Prune(Mix(secret, secret * 2048));

    }
    long Mix(long secret, long num) => secret ^ num;
    long Prune(long secret) => secret % 16777216;
    long Lsd(long secret) => secret % 10;
}
