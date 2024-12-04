using System.Text.RegularExpressions;

var data = File.ReadAllText("input.txt");
var validOps = Regex.Matches(data, @"mul\((\d+),(\d+)\)");
var result = validOps
    .Select(op => int.Parse(op.Groups[1].Value) * int.Parse(op.Groups[2].Value))
    .Sum();

Console.WriteLine(result);