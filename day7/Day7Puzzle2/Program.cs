var lines = File.ReadAllLines("input.txt");
var start = DateTime.Now;

var res = lines
    .Select(l => new Equation(l))
    .Where(eq => eq.Validate())
    .Select(eq => eq.Result)
    .Sum();

var duration = DateTime.Now - start;
Console.WriteLine($"Result: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");
