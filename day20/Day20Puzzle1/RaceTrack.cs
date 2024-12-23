record Point(int Row, int Col);

class RaceTrack
{
    private int _size;
    private char[][] _map;
    private Point _start = new(0, 0);

    public RaceTrack(string[] lines)
    {
        _size = lines.Length;
        _map = lines.Select((l, i) => l.Select((c, j) =>
        {
            if (c == 'S')
                _start = new(i, j);

            return c;
        }).ToArray()).ToArray();
    }

    public int FindViableCheats(int threshold = 1)
    {
        Console.Clear();
        Console.WriteLine("Finding cheat options...");

        HashSet<Point> cheatOptions = [];
        var noCheatsTime = BreadthSearch(cheatOptions);
        List<int> cheatSaves = [];

        Console.WriteLine($"No cheats time: {noCheatsTime} ps");
        var idx = 0;
        foreach (var option in cheatOptions)
        {
            Console.SetCursorPosition(0, 2);
            Console.WriteLine($"Testing cheat #{idx}/{cheatOptions.Count}");

            var cheatTime = BreadthSearch(activeCheatOption: option);
            if (noCheatsTime - cheatTime >= threshold)
                cheatSaves.Add(noCheatsTime - cheatTime);

            idx++;
        }

        return cheatSaves.Count;
    }

    private int BreadthSearch(HashSet<Point>? cheatOptions = null, Point? activeCheatOption = null)
    {
        var searchQueue = new Queue<Point>();
        HashSet<Point> visitedAddresses = [_start];
        var parents = new Dictionary<Point, Point>();

        searchQueue.Enqueue(_start);

        while (searchQueue.Count > 0)
        {
            var point = searchQueue.Dequeue();

            if (_map[point.Row][point.Col] == 'E')
            {
                return GetPathLength(point, parents);
            }

            Point[][] adjacentPoints = [
                [point with {Row = point.Row - 1}, point with {Row = point.Row - 2}],
                [point with {Row = point.Row + 1}, point with {Row = point.Row + 2}],
                [point with {Col = point.Col - 1}, point with {Col = point.Col - 2}],
                [point with {Col = point.Col + 1}, point with {Col = point.Col + 2}],
            ];

            foreach (var adjacentPoint in adjacentPoints)
            {
                if (adjacentPoint[0].Row >= 0 && adjacentPoint[0].Row < _size &&
                    adjacentPoint[0].Col >= 0 && adjacentPoint[0].Col < _size &&
                    !visitedAddresses.Contains(adjacentPoint[0]))
                {
                    if (_map[adjacentPoint[0].Row][adjacentPoint[0].Col] == '#')
                    {
                        if (cheatOptions != null)
                        {
                            if (adjacentPoint[1].Row >= 0 && adjacentPoint[1].Row < _size &&
                                adjacentPoint[1].Col >= 0 && adjacentPoint[1].Col < _size &&
                                _map[adjacentPoint[1].Row][adjacentPoint[1].Col] != '#')
                            {
                                cheatOptions.Add(adjacentPoint[0]);
                            }
                            continue;
                        }
                        if (activeCheatOption != null)
                        {
                            if (adjacentPoint[0] != activeCheatOption)
                                continue;
                        }
                    }

                    visitedAddresses.Add(adjacentPoint[0]);
                    parents.Add(adjacentPoint[0], point);
                    searchQueue.Enqueue(adjacentPoint[0]);
                }
            }
        }

        return -1;
    }

    private int GetPathLength(Point point, Dictionary<Point, Point> parents)
    {
        var length = 0;
        var pt = point;

        while (true)
        {
            length++;

            if (parents.TryGetValue(pt, out var nextPt))
            {
                pt = nextPt;
            }
            else
            {
                break;
            }
        }

        return length - 1;
    }
}