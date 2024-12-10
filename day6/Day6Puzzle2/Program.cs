var lines = File.ReadAllLines("input.txt");
var start = DateTime.Now;
var res = 0;

var lab = new Lab(lines);
var path = lab.FindRegularGuardPath();
if (path == null)
{
    Console.WriteLine("Guard is stuck in a loop");
}
else
{
    Console.WriteLine($"Regular path length: {path!.Count}");

    var candidates = path!.Skip(1).ToList();
    var idx = 0;

    // Console.WriteLine($"Found ${candidates.Count} obstacle candidates");
    foreach (var location in candidates)
    {
        idx++;
        if (lab.TestObstacle(new(location.X, location.Y)))
        {
            // Console.WriteLine($"Candidate #{idx} ({location.X}, {location.Y}): Valid obstacle location found");
            res++;
        }
    }
}

var duration = DateTime.Now - start;
Console.WriteLine($"\nResult: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");

