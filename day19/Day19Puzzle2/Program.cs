var lines = File.ReadAllLines("input.txt");
var start = DateTime.Now;

var towelPicker = new TowelPicker(lines.First());
var res = towelPicker.FindAllValidCombinations(lines.Skip(2));

var duration = DateTime.Now - start;
Console.WriteLine($"\nResult: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");