﻿namespace Command.Framework;

public abstract class ProblemBase<TReturn> : IProblem<TReturn>
{
    public void Load(Stream stream)
    {
        var reader = new StreamReader(stream);
        var line = reader.ReadLine();
        do
        {
            if (line is not null)
            {
                Line(line);
            }

            line = reader.ReadLine();
        } while (line is not null);

    }

    protected abstract void Line(string line);
    public abstract TReturn CalculateOne(bool exampleData);
    public abstract TReturn CalculateTwo(bool exampleData);
}
