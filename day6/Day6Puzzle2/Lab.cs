readonly record struct ObstacleLocation(int X, int Y);

class Lab
{
    private int _mapSize;
    private GuardLocation _initialGuardLocation;
    private List<ObstacleLocation> _obstacles = new();

    public Lab(IEnumerable<string> lines)
    {
        var map = lines.Select(l => l.ToCharArray()).ToArray();
        _mapSize = map.Length;

        for (int y = 0; y < _mapSize; y++)
        {
            for (int x = 0; x < _mapSize; x++)
            {
                if (map[y][x] == '^')
                {
                    _initialGuardLocation = new(x, y, GuardDirection.Up);
                }
                else if (map[y][x] == '#')
                {
                    _obstacles.Add(new(x, y));
                }
            }
        }
    }

    public List<GuardLocation>? FindRegularGuardPath()
    {
        return FindGuardPath(_obstacles);
    }

    public bool TestObstacle(ObstacleLocation obstacle)
    {
        return FindGuardPath([.. _obstacles, obstacle]) == null;
    }

    private List<GuardLocation>? FindGuardPath(List<ObstacleLocation> obstacles)
    {
        List<GuardLocation> guardPath = [
            _initialGuardLocation,
        ];
        var guardLocation = _initialGuardLocation;

        while (true)
        {
            var nextLocation = guardLocation.Next();

            if (nextLocation.X < 0 ||
                nextLocation.X >= _mapSize ||
                nextLocation.Y < 0 ||
                nextLocation.Y >= _mapSize
            )
            {
                return guardPath;
            }

            if (!obstacles.Contains(new(nextLocation.X, nextLocation.Y)))
            {
                guardLocation = nextLocation;

                if (!guardPath.Any(vl => vl.PositionEquals(guardLocation)))
                {
                    guardPath.Add(guardLocation);
                }
            }
            else
            {
                var nextDirection = guardLocation.Direction == GuardDirection.Left
                    ? GuardDirection.Up
                    : guardLocation.Direction + 1;
                guardLocation = new(guardLocation.X, guardLocation.Y, nextDirection);

                if (guardPath.Contains(guardLocation))
                {
                    // same position & direction means a loop
                    return null;
                }
                else
                {
                    guardPath = [.. guardPath.SkipLast(1), guardLocation];
                }
            }
        }
    }
}