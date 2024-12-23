namespace Command.Framework;

public interface IProblem<TReturn>
{
    void Load(Stream stream);
    TReturn CalculateOne(bool exampleData);
    TReturn CalculateTwo(bool exampleData);
}
