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
}

class Warehouse
{
    private int _size;
    private WarehousePosition[][] _map;
    private (int Row, int Col) _robot;

    public int BoxPosition => _map
        .Select(r => r
            .Where(wp => wp.Entity == WarehouseEntity.Box)
            .Select(wp => wp.Row * 100 + wp.Col)
            .Sum())
        .Sum();

    public Warehouse(string[] lines)
    {
        _size = lines.Length;
        _map = lines
            .Select((l, i) => l
                .Select((c, j) =>
                {
                    var entity = GetEntity(c);
                    if (entity == WarehouseEntity.Robot)
                    {
                        _robot = (i, j);
                    }

                    return new WarehousePosition(entity, i, j);
                })
                .ToArray())
            .ToArray();
    }

    public void MoveRobot(string moveSequence)
    {

        foreach (var move in moveSequence)
        {
            WarehousePosition currentPosition = _map[_robot.Row][_robot.Col];
            List<WarehousePosition> nextPositions = [];
            switch (move)
            {
                case '^':
                    nextPositions = _map
                        .Where((r, i) => i < _robot.Row)
                        .Select((r, i) => r[_robot.Col])
                        .Reverse()
                        .ToList();
                    break;
                case 'v':
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
                        nextEmptyPosition.Entity = WarehouseEntity.Box;
                        nextRobotPosition.Entity = WarehouseEntity.Robot;
                        currentPosition.Entity = WarehouseEntity.None;

                        _robot = (nextRobotPosition.Row, nextRobotPosition.Col);
                    }
                }
            }

            //Console.WriteLine($"\n{move}");
            //PrintMap();
        }
    }

    public void PrintMap()
    {
        for (int i = 0; i < _size; i++)
        {
            for (int j = 0; j < _size; j++)
            {
                switch (_map[i][j].Entity)
                {
                    case WarehouseEntity.Robot:
                        Console.Write("@");
                        break;
                    case WarehouseEntity.Box:
                        Console.Write("O");
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