enum GuardDirection
{
    Up,
    Right,
    Down,
    Left,
}

readonly record struct GuardLocation(int X, int Y, GuardDirection Direction)
{
    public GuardLocation Next()
    {
        var nextPosX = X;
        var nextPosY = Y;

        switch (Direction)
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

        return new(nextPosX, nextPosY, Direction);
    }

    public bool PositionEquals(GuardLocation guardLocation)
    {
        return X == guardLocation.X && Y == guardLocation.Y;
    }
    public bool PositionEquals(ObstacleLocation obstacleLocation)
    {
        return X == obstacleLocation.X && Y == obstacleLocation.Y;
    }
}