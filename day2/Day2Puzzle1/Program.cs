var lines = File.ReadAllLines("input.txt");
var safeReports = lines.Select(line => line.Split(" ").Select(v => int.Parse(v))).Select(lvs =>
{
    var lineValues = lvs.ToList();
    if (lineValues.Count < 2) return 0;

    var isDesc = lineValues[1] < lineValues[0];
    for (int i = 1; i < lineValues.Count; i++)
    {
        var diff = lineValues[i] - lineValues[i - 1];

        if (
            diff == 0 ||
            (isDesc && diff > 0) ||
            (!isDesc && diff < 0) ||
            Math.Abs(diff) > 3
        )
        {
            return 0;
        }
    }

    return 1;
}).Sum();

Console.WriteLine($"# of safe reports: {safeReports}");