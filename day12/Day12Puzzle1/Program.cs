var lines = File.ReadAllLines("test3.txt");
var start = DateTime.Now;

var garden = new Garden(lines);
var res = garden.CalculatePrice();

var duration = DateTime.Now - start;
Console.WriteLine($"\nResult: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");

