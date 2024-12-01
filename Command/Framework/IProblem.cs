namespace Command.Framework;

public interface IProblem<TReturn>
{
    void Load(Stream stream);
    TReturn CalculateOne();
    TReturn CalculateTwo();
}
