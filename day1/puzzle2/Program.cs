var lines = File.ReadAllLines("input.txt").Select(line => line.Split("   "));
var parseFunc = (int i) => lines.Select(line => int.Parse(line[i])).ToList();

var leftList = parseFunc(0);
var rightList = parseFunc(1);

var freqMap = rightList.GroupBy(rv => rv).ToDictionary(g => g.Key, g => g.Count());
var similarity = leftList.Select(lv => lv * freqMap.GetValueOrDefault(lv)).Sum();

Console.WriteLine(similarity);