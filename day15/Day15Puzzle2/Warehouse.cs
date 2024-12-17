
enum WarehouseEntity
{
    None,
    Robot,
    Box,
    Wall
}

class WarehousePosition(WarehouseEntity Entity, int Row, int Col)
{
    public WarehouseEntity Entity { get; set; } = Entity;
    public int Row { get; } = Row;
    public int Col { get; } = Col;
    public WarehousePosition? BoxHalf { get; set; }
}

class Warehouse
{
    private int _size;
    private WarehousePosition[][] _map;
    private (int Row, int Col) _robot;

    public int BoxPosition => _map
        .Select(r => r
            .Where(wp => wp.Entity == WarehouseEntity.Box && wp.Col < wp.BoxHalf!.Col)
            .Select(wp => wp.Row * 100 + wp.Col)
            .Sum())
        .Sum();

    public Warehouse(string[] lines)
    {
        _size = lines.Length * 2;
        _map = lines
            .Select((l, i) => l.ToCharArray()
                .Select<char, WarehousePosition[]>((c, j) =>
                {
                    var entity = GetEntity(c);
                    if (entity == WarehouseEntity.Robot)
                    {
                        _robot = (i, j * 2);
                        return [
                            new WarehousePosition(entity, i, j*2),
                            new WarehousePosition(WarehouseEntity.None, i, j*2+1),
                        ];
                    }
                    else if (entity == WarehouseEntity.Box)
                    {
                        var leftBoxHalf = new WarehousePosition(entity, i, j * 2);
                        var rightBoxHalf = new WarehousePosition(entity, i, j * 2 + 1);

                        leftBoxHalf.BoxHalf = rightBoxHalf;
                        rightBoxHalf.BoxHalf = leftBoxHalf;

                        return [leftBoxHalf, rightBoxHalf];
                    }

                    return [
                        new WarehousePosition(entity, i, j*2),
                        new WarehousePosition(entity, i, j*2+1),
                    ];
                })
                .SelectMany(wp => wp)
                .ToArray())
            .ToArray();
    }

    public void MoveRobot(string moveSequence)
    {

        foreach (var move in moveSequence)
        {
            WarehousePosition currentPosition = _map[_robot.Row][_robot.Col];
            List<WarehousePosition> nextPositions = [];
            var isVertical = false;
            switch (move)
            {
                case '^':
                    isVertical = true;
                    nextPositions = _map
                        .Where((r, i) => i < _robot.Row)
                        .Select((r, i) => r[_robot.Col])
                        .Reverse()
                        .ToList();
                    break;
                case 'v':
                    isVertical = true;
                    nextPositions = _map
                        .Where((r, i) => i > _robot.Row)
                        .Select((r, i) => r[_robot.Col])
                        .ToList();
                    break;
                case '<':
                    nextPositions = _map[_robot.Row]
                        .Where((wp, j) => j < _robot.Col)
                        .Reverse()
                        .ToList();
                    break;
                case '>':
                    nextPositions = _map[_robot.Row]
                        .Where((wp, j) => j > _robot.Col)
                        .ToList();
                    break;
            }

            var nextRobotPosition = nextPositions.First();
            if (nextRobotPosition.Entity == WarehouseEntity.None)
            {
                nextRobotPosition.Entity = WarehouseEntity.Robot;
                currentPosition.Entity = WarehouseEntity.None;

                _robot = (nextRobotPosition.Row, nextRobotPosition.Col);
            }
            else if (nextRobotPosition.Entity == WarehouseEntity.Box)
            {
                var nextEmptyPositionIdx = nextPositions.FindIndex(wp => wp.Entity == WarehouseEntity.None);
                if (nextEmptyPositionIdx > 0)
                {
                    var nextEmptyPosition = nextPositions[nextEmptyPositionIdx];
                    var nextWallPositionIdx = nextPositions.FindIndex(wp => wp.Entity == WarehouseEntity.Wall);

                    if (nextWallPositionIdx < 0 || nextWallPositionIdx > nextEmptyPositionIdx)
                    {
                        bool isMovable = true;
                        if (isVertical)
                        {
                            HashSet<WarehousePosition> boxes = [];
                            foreach (var position in nextPositions.TakeWhile(np => np != nextEmptyPosition))
                            {
                                boxes.Add(position);
                                boxes.Add(position.BoxHalf!);

                                isMovable &= FindFurtherBoxes(boxes, position.BoxHalf!, move == '^');

                                if (!isMovable) break;
                            }

                            if (isMovable)
                            {
                                if (move == '^')
                                {
                                    var boxesList = boxes.ToArray().OrderBy(b => b.Row).GroupBy(b => b.Row).ToList();
                                    foreach (var boxRow in boxesList)
                                    {
                                        foreach (var box in boxRow)
                                        {
                                            var boxHalf = _map[box.Row][box.Col].BoxHalf!;

                                            _map[box.Row - 1][box.Col].Entity = WarehouseEntity.Box;
                                            _map[box.Row - 1][box.Col].BoxHalf = _map[boxHalf.Row - 1][boxHalf.Col];

                                            _map[box.Row][box.Col].Entity = WarehouseEntity.None;
                                            _map[box.Row][box.Col].BoxHalf = null;
                                        }
                                    }
                                }
                                else
                                {
                                    var boxesList = boxes.ToArray().OrderByDescending(b => b.Row).GroupBy(b => b.Row).ToList();
                                    foreach (var boxRow in boxesList)
                                    {
                                        foreach (var box in boxRow)
                                        {
                                            var boxHalf = _map[box.Row][box.Col].BoxHalf!;

                                            _map[box.Row + 1][box.Col].Entity = WarehouseEntity.Box;
                                            _map[box.Row + 1][box.Col].BoxHalf = _map[boxHalf.Row + 1][boxHalf.Col];

                                            _map[box.Row][box.Col].Entity = WarehouseEntity.None;
                                            _map[box.Row][box.Col].BoxHalf = null;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            nextEmptyPosition.Entity = WarehouseEntity.Box;
                            for (int i = nextEmptyPositionIdx; i > 0; i--)
                            {
                                nextPositions[i].BoxHalf = i % 2 == 0 ? nextPositions[i - 1] : nextPositions[i + 1];
                            }
                        }

                        if (isMovable)
                        {
                            nextRobotPosition.Entity = WarehouseEntity.Robot;
                            nextRobotPosition.BoxHalf = null;
                            currentPosition.Entity = WarehouseEntity.None;

                            _robot = (nextRobotPosition.Row, nextRobotPosition.Col);
                        }
                    }
                }
            }

            // Console.Clear();
            // Console.WriteLine($"\n{move}");
            // PrintMap();
            // Thread.Sleep(500);
        }
    }

    public void PrintMap()
    {
        for (int i = 0; i < _size / 2; i++)
        {
            for (int j = 0; j < _size; j++)
            {
                switch (_map[i][j].Entity)
                {
                    case WarehouseEntity.Robot:
                        Console.Write("@");
                        break;
                    case WarehouseEntity.Box:
                        Console.Write(_map[i][j].Col < _map[i][j].BoxHalf!.Col ? "[" : "]");
                        break;
                    case WarehouseEntity.Wall:
                        Console.Write("#");
                        break;
                    default:
                        Console.Write(".");
                        break;
                }
            }
            Console.WriteLine();
        }
    }

    private bool FindFurtherBoxes(HashSet<WarehousePosition> boxes, WarehousePosition box, bool isUp)
    {
        var furtherPosition = _map[isUp ? box.Row - 1 : box.Row + 1][box.Col];

        if (furtherPosition.Entity == WarehouseEntity.Wall) return false;
        if (furtherPosition.Entity == WarehouseEntity.None) return true;

        boxes.Add(furtherPosition);
        boxes.Add(furtherPosition.BoxHalf!);

        return FindFurtherBoxes(boxes, furtherPosition, isUp) && FindFurtherBoxes(boxes, furtherPosition.BoxHalf!, isUp);
    }

    private WarehouseEntity GetEntity(char input)
    {
        switch (input)
        {
            case '@':
                return WarehouseEntity.Robot;
            case 'O':
                return WarehouseEntity.Box;
            case '#':
                return WarehouseEntity.Wall;
            default:
                return WarehouseEntity.None;
        }
    }
}