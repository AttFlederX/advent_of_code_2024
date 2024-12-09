var disk = File.ReadAllText("input.txt");
var start = DateTime.Now;

var fs = new FileSystem(disk);
fs.Defragment();
var res = fs.Checksum;

var duration = DateTime.Now - start;
Console.WriteLine($"Result: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");

fs.PrintDiskMap();
