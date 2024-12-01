using System.Diagnostics;

namespace Command.Framework;

public static class ProblemRunner<TProblem, TReturn> where TProblem : IProblem<TReturn>, new()
{
    public static void Run(string fBase)
    {
        Console.WriteLine(typeof(TProblem).Name);
        Console.WriteLine(new string('-', typeof(TProblem).Name.Length));
        var problem = new TProblem();
        var exampleFile = $"./in/{fBase}0.txt";
        if (File.Exists(exampleFile))
        {
            var start = Stopwatch.GetTimestamp();
            Console.WriteLine("\nExample\n-------");
            var stream = new FileStream(exampleFile, FileMode.Open, FileAccess.Read);
            var hh = Activator.CreateInstance<TProblem>();
            hh.Load(stream);
            Console.Write("1) ");
            Console.WriteLine(hh.CalculateOne());
            Console.Write("2) ");
            Console.WriteLine(hh.CalculateTwo());
            Console.WriteLine($"Took {Stopwatch.GetElapsedTime(start).TotalMilliseconds}");
        }
        else
        {
            Console.WriteLine($"No example file found {exampleFile}.");
        }

        var problemFile = $"./in/{fBase}1.txt";
        if (File.Exists(problemFile))
        {
            var start = Stopwatch.GetTimestamp();
            Console.WriteLine("\nProblem\n-------");
            var stream = new FileStream(problemFile, FileMode.Open, FileAccess.Read);
            var hh = Activator.CreateInstance<TProblem>();
            hh.Load(stream);
            Console.Write("1) ");
            Console.WriteLine(hh.CalculateOne());
            Console.Write("2) ");
            Console.WriteLine(hh.CalculateTwo());
            Console.WriteLine($"Took {Stopwatch.GetElapsedTime(start).TotalMilliseconds}");
        }
        else
        {
            Console.WriteLine($"No problem file found {exampleFile}.");
        }
    }
}
