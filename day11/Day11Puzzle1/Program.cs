var lines = File.ReadAllText("input.txt");
var start = DateTime.Now;

var set = new StoneSet(lines);
set.Print();
for (int i = 0; i < 25; i++)
{
    set.Blink();
    Console.WriteLine($"\nBlink #{i + 1}: {set.StoneCount}");
    set.Print();
}
var res = set.StoneCount;

var duration = DateTime.Now - start;
Console.WriteLine($"\nResult: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");

