using System.Diagnostics;

namespace Command.Framework;

public static class ProblemRunner<TProblem, TReturn> where TProblem : IProblem<TReturn>, new()
{
    public static void Run(string fBase)
    {
        Console.WriteLine(typeof(TProblem).FullName);
        Console.WriteLine(new string('-', typeof(TProblem).FullName?.Length ?? 0));
        var problem = new TProblem();
        var exampleFileA = $"./in/{fBase}0a.txt";
        var exampleFileB = $"./in/{fBase}0b.txt";
        Console.WriteLine("\nExample\n-------");

        if (File.Exists(exampleFileA))
        {
            var start = Stopwatch.GetTimestamp();
            var stream = new FileStream(exampleFileA, FileMode.Open, FileAccess.Read);
            var hh = Activator.CreateInstance<TProblem>();
            hh.Load(stream);
            Console.Write("1) ");
            Console.WriteLine(hh.CalculateOne());
            Console.WriteLine($"   Took {Stopwatch.GetElapsedTime(start).TotalMilliseconds}");
        }
        else
        {
            Console.WriteLine($"\nNo example file found {exampleFileA}.");
        }

        if (File.Exists(exampleFileB))
        {
            var start = Stopwatch.GetTimestamp();
            var stream = new FileStream(exampleFileB, FileMode.Open, FileAccess.Read);
            var hh = Activator.CreateInstance<TProblem>();
            hh.Load(stream);
            Console.Write("2) ");
            Console.WriteLine(hh.CalculateTwo());
            Console.WriteLine($"   Took {Stopwatch.GetElapsedTime(start).TotalMilliseconds}");
        }
        else if (File.Exists(exampleFileA))
        {
            var stream = new FileStream(exampleFileA, FileMode.Open, FileAccess.Read);
            var hh = Activator.CreateInstance<TProblem>();
            hh.Load(stream);
            var start = Stopwatch.GetTimestamp();
            Console.Write("2) ");
            Console.WriteLine(hh.CalculateTwo());
            Console.WriteLine($"   Took {Stopwatch.GetElapsedTime(start).TotalMilliseconds}");
        }

        var problemFile = $"./in/{fBase}1a.txt";
        if (File.Exists(problemFile))
        {
            var start = Stopwatch.GetTimestamp();
            Console.WriteLine("\nProblem\n-------");
            var stream = new FileStream(problemFile, FileMode.Open, FileAccess.Read);
            var hh = Activator.CreateInstance<TProblem>();
            hh.Load(stream);
            Console.Write("1) ");
            Console.WriteLine(hh.CalculateOne());
            Console.WriteLine($"   Took {Stopwatch.GetElapsedTime(start).TotalMilliseconds}");
            start = Stopwatch.GetTimestamp();
            Console.Write("2) ");
            Console.WriteLine(hh.CalculateTwo());
            Console.WriteLine($"   Took {Stopwatch.GetElapsedTime(start).TotalMilliseconds}");
        }
        else
        {
            Console.WriteLine($"\nNo problem file found {problemFile}.");
        }

        Console.WriteLine("\n");
    }
}
