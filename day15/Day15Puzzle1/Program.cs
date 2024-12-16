var lines = File.ReadAllLines("input.txt");
var start = DateTime.Now;

var warehouse = new Warehouse(lines.TakeWhile(l => l.Length > 0).ToArray());
warehouse.MoveRobot(lines.SkipWhile(l => l.Length > 0).Aggregate((l1, l2) => l1.Trim() + l2.Trim()));
var res = warehouse.BoxPosition;

var duration = DateTime.Now - start;
Console.WriteLine($"\nResult: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");