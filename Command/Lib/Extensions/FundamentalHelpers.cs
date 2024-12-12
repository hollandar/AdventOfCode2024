namespace Command.Lib.Extensions;

public static class FundamentalHelpers
{
    public static int SignificantDigits(this long number)
    {
        if (number == 0)
            return 1;
        else
            return (int)Math.Floor(Math.Log10(Math.Abs(number)) + 1);
    }

    public static int SignificantDigits(this int number) => SignificantDigits((long)number);
}
