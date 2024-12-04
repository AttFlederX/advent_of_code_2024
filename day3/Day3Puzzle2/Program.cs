using System.Text.RegularExpressions;

var data = File.ReadAllText("input.txt").Replace("\n", string.Empty);
//var data = @"xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))don't()mul(28,11)aaa";
var enabledInstructions = Regex.Replace(data, @"don't\(\).*?(do\(\)|$)", string.Empty);

var validOps = Regex.Matches(enabledInstructions, @"mul\((\d+),(\d+)\)");
var result = validOps
    .Select(op => new List<int> { int.Parse(op.Groups[1].Value), int.Parse(op.Groups[2].Value) })
    .Select(arr => arr[0] * arr[1])
    .Sum();

Console.WriteLine(result);