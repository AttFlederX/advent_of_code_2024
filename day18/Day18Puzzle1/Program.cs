var lines = File.ReadAllLines("test.txt");
var start = DateTime.Now;

var ram = new Ram(7, 12, lines);
var res = ram.FindShortestEscapeRoute();

var duration = DateTime.Now - start;
Console.WriteLine($"\nResult: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");