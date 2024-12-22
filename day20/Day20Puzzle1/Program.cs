var lines = File.ReadAllLines("input.txt");
var start = DateTime.Now;

var raceTrack = new RaceTrack(lines);
var res = raceTrack.FindViableCheats(100);

var duration = DateTime.Now - start;
Console.WriteLine($"\nResult: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");