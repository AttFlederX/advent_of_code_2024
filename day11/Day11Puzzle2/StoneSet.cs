
class StoneSet
{
    private Dictionary<long, long> _stones;

    public long StoneCount => _stones.Select(s => s.Value).Sum();

    public StoneSet(string input)
    {
        _stones = input.Split(' ').Select(long.Parse).GroupBy(s => s).ToDictionary(g => g.Key, g => (long)g.Count());
    }

    public void Blink()
    {
        Dictionary<long, long> newMap = [];
        foreach (var stone in _stones)
        {
            if (stone.Key == 0)
            {
                newMap.UpsertValue(1, stone.Value);
            }
            else
            {
                var valueLength = GetNumberOfDigits(stone.Key);
                if (valueLength % 2 == 0)
                {
                    newMap.UpsertValue((long)(stone.Key / Math.Pow(10, valueLength / 2)), stone.Value);
                    newMap.UpsertValue((long)(stone.Key % Math.Pow(10, valueLength / 2)), stone.Value);
                }
                else
                {
                    newMap.UpsertValue(stone.Key * 2024, stone.Value);
                }
            }
        }

        _stones = newMap;
    }

    public void Print()
    {
        Console.WriteLine();
        foreach (var stone in _stones)
        {
            Console.Write($"[{stone.Key}: {stone.Value}] ");
        }
        Console.WriteLine();
    }

    private int GetNumberOfDigits(long stone)
    {
        var count = 0;
        while (stone > 0)
        {
            stone /= 10;
            count++;
        }

        return count;
    }
}

public static class DictionaryExtensions
{
    public static void UpsertValue(this Dictionary<long, long> dictionary, long key, long value)
    {
        if (dictionary.TryGetValue(key, out long currentValue))
        {
            dictionary[key] = currentValue + value;
        }
        else
        {
            dictionary[key] = value;
        }
    }
}