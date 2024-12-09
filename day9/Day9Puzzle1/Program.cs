var disk = File.ReadAllText("input.txt");
var start = DateTime.Now;

var fs = new FileSystem(disk);
fs.Compact();
var res = fs.Checksum;

var duration = DateTime.Now - start;
Console.WriteLine($"Result: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");

