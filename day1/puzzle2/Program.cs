var lines = File.ReadAllLines("input.txt").Select(line => line.Split("   "));

var leftList = lines.Select(line => int.Parse(line[0])).Order().ToList();
var rightList = lines.Select(line => int.Parse(line[1])).Order().ToList();

var similarity = 0;
var freqMap = new Dictionary<int, int>();

foreach (var lv in leftList)
{
    if (freqMap.TryGetValue(lv, out int freq)) {
        similarity += lv * freq;
    } else {
        var lvFreq = rightList.Count(rv => rv == lv);
        similarity += lv * lvFreq;

        freqMap[lv] = lvFreq;
    }
}

Console.WriteLine(similarity);