var lines = File.ReadAllLines("input.txt");
var start = DateTime.Now;

var maze = new Maze(lines);
var res = maze.FindSpectatingSpots();

var duration = DateTime.Now - start;
Console.WriteLine($"\nResult: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");