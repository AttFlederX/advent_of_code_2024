using System.Data;

var lines = File.ReadAllLines("input.txt");
var start = DateTime.Now;
var res = 0;

var pageOrderLines = lines
    .Where(l => l.Contains('|'))
    .Select(l => l.Split('|')
    .Select(val => int.Parse(val))
    .ToList()
);

var beforeMap = new Dictionary<int, List<int>>();
var afterMap = new Dictionary<int, List<int>>();

foreach (var pageOrderLine in pageOrderLines)
{
    var beforeValue = pageOrderLine[0];
    var afterValue = pageOrderLine[1];

    if (!beforeMap.TryAdd(beforeValue, [afterValue]))
    {
        beforeMap[beforeValue].Add(afterValue);
    }
    if (!afterMap.TryAdd(afterValue, [beforeValue]))
    {
        afterMap[afterValue].Add(beforeValue);
    }
}

var updateLines = lines
    .Where(l => l.Contains(','))
    .Select(l => l.Split(',')
    .Select(val => int.Parse(val))
    .ToList()
);

foreach (var update in updateLines)
{
    if (IsValidUpdate(update))
    {
        res += update[update.Count / 2];
    }
}

var duration = DateTime.Now - start;
Console.WriteLine($"\nResult: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");

bool IsValidUpdate(List<int> update)
{
    for (int i = 0; i < update.Count; i++)
    {
        for (int j = 0; j < i; j++)
        {
            if (beforeMap.GetValueOrDefault(update[i])?.Contains(update[j]) ?? false)
            {
                return false;
            }
        }
        for (int j = i + 1; j < update.Count; j++)
        {
            if (afterMap.GetValueOrDefault(update[i])?.Contains(update[j]) ?? false)
            {
                return false;
            }
        }
    }

    return true;
}