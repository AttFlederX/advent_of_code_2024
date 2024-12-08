var lines = File.ReadAllLines("input.txt");
var start = DateTime.Now;

var map = lines.Select(l => l.ToCharArray()).ToArray();

var guardPosX = 0;
var guardPosY = 0;

// find initial guard pos
for (int y = 0; y < map.Length; y++)
{
    for (int x = 0; x < map[y].Length; x++)
    {
        if (map[y][x] == '^')
        {
            guardPosX = x;
            guardPosY = y;
            break;
        }
    }
}

List<int[]> visitedPoints = [
    [guardPosX, guardPosY]
];
var direction = GuardDirection.Up;

while (true)
{
    var nextPosX = guardPosX;
    var nextPosY = guardPosY;

    switch (direction)
    {
        case GuardDirection.Up:
            nextPosY--;
            break;
        case GuardDirection.Right:
            nextPosX++;
            break;
        case GuardDirection.Down:
            nextPosY++;
            break;
        case GuardDirection.Left:
            nextPosX--;
            break;
    }


    if (nextPosX < 0 || nextPosX >= map[0].Length || nextPosY < 0 || nextPosY >= map.Length)
    {
        break;
    }

    if (map[nextPosY][nextPosX] != '#')
    {
        guardPosX = nextPosX;
        guardPosY = nextPosY;

        int[] point = [guardPosX, guardPosY];
        if (!visitedPoints.Any(vp => vp.SequenceEqual(point)))
        {
            visitedPoints.Add(point);
        }
    }
    else
    {
        direction = direction == GuardDirection.Left ? GuardDirection.Up : direction + 1;
    }
}

var duration = DateTime.Now - start;
Console.WriteLine($"Result: {visitedPoints.Count}");
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");

enum GuardDirection
{
    Up,
    Right,
    Down,
    Left,
}