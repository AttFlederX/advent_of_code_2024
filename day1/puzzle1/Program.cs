var lines = File.ReadAllLines("input.txt").Select(line => line.Split("   "));

var leftList = lines.Select(line => int.Parse(line[0])).Order().ToList();
var rightList = lines.Select(line => int.Parse(line[1])).Order().ToList();

var distance = 0;
for (int i = 0; i < leftList.Count; i++)
{
    distance += leftList[i] < rightList[i] ? rightList[i] - leftList[i] : leftList[i] - rightList[i];
}

Console.WriteLine(distance);