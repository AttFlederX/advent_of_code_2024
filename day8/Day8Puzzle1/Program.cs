var lines = File.ReadAllLines("input.txt");
var start = DateTime.Now;

var map = new Map(lines);
var res = map.FindAntiNodes().Distinct().Count();

var duration = DateTime.Now - start;
Console.WriteLine($"Result: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");



