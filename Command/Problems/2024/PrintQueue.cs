using Command.Framework;
using Command.Problems._2023;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Command.Problems._2024;


record PageRule(int x, int y);
record PageList(List<int> pages)
{
    public bool IsBefore(int a, int b)
    {
        if (!Contains(a) || !Contains(b))
        {
            throw new Exception();
        }

        return pages.IndexOf(a) < pages.IndexOf(b);
    }

    public bool Contains(int a)
    {
        return pages.Contains(a);
    }

    public override string ToString()
    {
        return string.Join(',', pages);
    }
}

public partial class PrintQueue : ProblemBase<int>
{
    List<PageRule> pageRules = new();
    List<PageList> pages = new();
    public PrintQueue()
    {
    }

    bool handlingPageRules = true;
    protected override void Line(string line)
    {
        if (String.IsNullOrWhiteSpace(line))
        {
            handlingPageRules = false;
            return;
        }

        if (handlingPageRules)
        {
            var parts = line.Split('|');
            var x = int.Parse(parts[0]);
            var y = int.Parse(parts[1]);
            pageRules.Add(new PageRule(x, y));
        }
        else
        {
            var parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            pages.Add(new PageList(parts));
        }
    }


    public override int CalculateOne()
    {
        var midSum = 0;
        foreach (var pageList in pages)
        {
            var allRulesValid = PageListIsValid(pageList);

            if (allRulesValid)
            {
                var midPoint = pageList.pages.Count / 2;
                var midValue = pageList.pages[midPoint];
                midSum += midValue;
            }
        }

        return midSum;
    }

    public override int CalculateTwo()
    {
        var midSum = 0;
        foreach (var pageList in pages)
        {
            if (PageListIsValid(pageList))
            {
                continue;
            }

            while (!PageListIsValid(pageList))
                foreach (var pageRule in pageRules)
                {
                    if (PageListIsValid(pageList))
                        break;

                    if (pageList.Contains(pageRule.x) && pageList.Contains(pageRule.y))
                    {
                        if (!pageList.IsBefore(pageRule.x, pageRule.y))
                        {
                            var xIndex = pageList.pages.IndexOf(pageRule.x);
                            var yIndex = pageList.pages.IndexOf(pageRule.y);
                            pageList.pages[xIndex] = pageRule.y;
                            pageList.pages[yIndex] = pageRule.x;
                        }
                    }
                }

            if (PageListIsValid(pageList))
            {
                var midPoint = pageList.pages.Count / 2;
                var midValue = pageList.pages[midPoint];
                midSum += midValue;
            }

        }

        return midSum;
    }

    private bool PageListIsValid(PageList pageList)
    {
        var allRulesValid = true;
        foreach (var pageRule in pageRules)
        {
            if (pageList.Contains(pageRule.x) && pageList.Contains(pageRule.y))
            {
                allRulesValid &= pageList.IsBefore(pageRule.x, pageRule.y);
            }
        }

        return allRulesValid;
    }
}
