var lines = File.ReadAllLines("test2.txt");
var start = DateTime.Now;

var maze = new Maze(lines);
var res = maze.FindLowestScore();

var duration = DateTime.Now - start;
Console.WriteLine($"\nResult: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");