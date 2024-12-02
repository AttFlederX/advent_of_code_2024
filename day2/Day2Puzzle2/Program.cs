
var lines = File.ReadAllLines("input.txt");
var safeReports = lines.Select(line => line.Split(" ").Select(v => int.Parse(v))).Select(lvs =>
{
    var lineValues = lvs.ToList();
    if (TestReport(lineValues))
    {
        return 1;
    }
    else
    {
        for (int i = 0; i < lineValues.Count; i++)
        {
            // test with ith value removed
            if (TestReport(lineValues.Take(i).Concat(lineValues.Skip(i + 1)).ToList()))
            {
                return 1;
            }
        }
    }

    return 0;
}).Sum();

Console.WriteLine($"# of safe reports w/ dampening: {safeReports}");


static bool TestReport(List<int> report)
{
    if (report.Count < 2) return false;

    var isDesc = report[1] < report[0];
    for (int i = 1; i < report.Count; i++)
    {
        var diff = report[i] - report[i - 1];

        if (
            diff == 0 ||
            isDesc && diff > 0 ||
            !isDesc && diff < 0 ||
            Math.Abs(diff) > 3
        )
        {
            return false;
        }
    }

    return true;
}
