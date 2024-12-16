var lines = File.ReadAllLines("input.txt");
var start = DateTime.Now;

var bathroom = new Bathroom(lines, 101, 103);
var res = 0;

while (res < 52084)
{
    bathroom.MoveGuards();
    res++;

    if (res > 7000 && bathroom.IsChristmasTree)
    {
        bathroom.Print();
        break;
    }

    Console.WriteLine($"Time elapsed: {res} seconds");
}

var duration = DateTime.Now - start;
Console.WriteLine($"\nResult: {res}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");