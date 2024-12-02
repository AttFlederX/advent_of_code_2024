var lines = File.ReadAllLines("input.txt").Select(line => line.Split("   "));
var parseFunc = (int i) => lines.Select(line => int.Parse(line[i])).Order().ToList();

var leftList = parseFunc(0);
var rightList = parseFunc(1);

var distance = leftList.Zip(rightList, (lv, rv) => Math.Abs(lv - rv)).Sum();

Console.WriteLine(distance);