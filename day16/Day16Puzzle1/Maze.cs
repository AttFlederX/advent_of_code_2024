using Point = (int Row, int Col);
record VisitedPoint(int Row, int Col, Direction direction);

enum Direction
{
    North,
    South,
    West,
    East,
}

class Maze
{
    private int _size;
    private char[][] _map;
    private Point _start;

    public Maze(string[] lines)
    {
        _size = lines.Length;
        _map = lines.Select((l, i) => l.Select((c, j) =>
        {
            if (c == 'S')
            {
                _start = (i, j);
            }

            return c;
        }).ToArray()).ToArray();
    }

    public int FindLowestScore()
    {
        var scores = new SortedSet<int>();

        MakeMove(_start, Direction.East, 1, scores, []);
        MakeMove(_start, Direction.North, 1000, scores, []);

        return scores.Min();
    }

    private void MakeMove(Point point, Direction direction, int score, SortedSet<int> scores, HashSet<Point> history)
    {
        if (scores.Count > 0 && score > scores.First())
        {
            return;
        }

        if (history.Contains(point))
        {
            return;
        }
        history.Add(point);

        if (_map[point.Row][point.Col] == 'E')
        {
            scores.Add(score);
            return;
        }

        Dictionary<Direction, Point> nextPoints = [];
        if (direction != Direction.North)
            nextPoints.Add(Direction.South, (point.Row + 1, point.Col));
        if (direction != Direction.South)
            nextPoints.Add(Direction.North, (point.Row - 1, point.Col));
        if (direction != Direction.West)
            nextPoints.Add(Direction.East, (point.Row, point.Col + 1));
        if (direction != Direction.East)
            nextPoints.Add(Direction.West, (point.Row, point.Col - 1));

        foreach (var nextPoint in nextPoints)
        {
            if (_map[nextPoint.Value.Row][nextPoint.Value.Col] != '#')
            {
                MakeMove(nextPoint.Value, nextPoint.Key, score + (direction == nextPoint.Key ? 1 : 1001), scores, [.. history, point]);
            }
        }
    }

    private Direction GetOppositeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.North:
                return Direction.South;
            case Direction.South:
                return Direction.North;
            case Direction.East:
                return Direction.West;
            case Direction.West:
                return Direction.East;
        }

        return default;
    }
}