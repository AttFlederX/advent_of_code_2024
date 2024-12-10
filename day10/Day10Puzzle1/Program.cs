var lines = File.ReadAllLines("input.txt");
var start = DateTime.Now;

var topoMap = new TopoMap(lines);
var res = topoMap.FindTrailHeadScores().Sum();

var duration = DateTime.Now - start;
Console.WriteLine($"Result: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");

