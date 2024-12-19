namespace Command.Lib.Alg;

/// <summary>
/// A standard binary search implementation that uses a custom predicate to find the target value
/// </summary>
public class BinarySearch
{
    public BinarySearch(int min, int max)
    {
        Min = min;
        Max = max;
    }

    public int Min { get; }
    public int Max { get; }

    /// <summary>
    /// Find the first element that satisfies the predicate
    /// </summary>
    /// <param name="predicate">Action that returns negative if the target value is less, positive if the target value is more, or 0 if the values match</param>
    /// <returns>Index of found element</returns>
    public int Find(Func<int, int> predicate)
    {
        int min = Min;
        int max = Max;
        while (min < max)
        {
            int mid = min + (max - min) / 2;
            var result = predicate(mid);
            if (result < 0)
            {
                max = mid;
            }
            else if (result > 0)
            {
                min = mid + 1;
            }
            else
            {
                return mid;
            }
        }

        return -1; /* Not Found */
    }
}
