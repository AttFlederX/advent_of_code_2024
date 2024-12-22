var lines = File.ReadAllLines("test.txt");
var start = DateTime.Now;

var towelPicker = new TowelPicker(lines.First());
var res = towelPicker.FindValidCombinations(lines.Skip(2));

var duration = DateTime.Now - start;
Console.WriteLine($"\nResult: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");