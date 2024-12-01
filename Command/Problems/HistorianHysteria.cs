﻿using Command.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Command.Problems;

internal class HistorianHysteria : ProblemBase<int>
{
    List<int> l1 = new List<int>();
    List<int> l2 = new List<int>();
    public HistorianHysteria()
    {
    }

    protected override void Line(string line)
    {
        var items = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        l1.Add(int.Parse(items[0]));
        l2.Add(int.Parse(items[1]));
    }

    public override int CalculateOne()
    {
        int cdist = 0;
        Debug.Assert(l1.Count == l2.Count);
        l1.Sort();
        l2.Sort();

        for (int i = 0; i < l1.Count; i++)
        {
            var v1 = l1[i];
            var v2 = l2[i];
            cdist += Math.Max(v1, v2) - Math.Min(v1, v2);
        }

        return cdist;
    }

    public override int CalculateTwo()
    {
        int cdist = 0;
        Debug.Assert(l1.Count == l2.Count);

        for (int i = 0; i < l1.Count; i++)
        {
            var v1 = l1[i];
            var v2 = l2.Where(r => r == v1).Count();
            cdist += v1 * v2;
        }

        return cdist;
    }


}
