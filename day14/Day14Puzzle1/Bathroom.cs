using System.Text.RegularExpressions;
using Point = (int Row, int Col);
record GuardConfig(Point Position, Point Velocity);

class Bathroom
{
    private Regex _guardConfigRegex = new(@"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)");

    private GuardConfig[] _guards;
    private int _width;
    private int _height;

    public int SafetyFactor
    {
        get
        {
            var topLeftQuad = 0;
            var topRightQuad = 0;
            var bottomLeftQuad = 0;
            var bottomRightQuad = 0;

            for (int i = 0; i < _height; i++)
            {
                if (i == _height / 2) continue;
                for (int j = 0; j < _width; j++)
                {
                    if (j == _width / 2) continue;

                    var count = _guards.Count(gp => gp.Position == (i, j));
                    if (count > 0)
                    {
                        if (i < _height / 2 && j < _width / 2) topLeftQuad += count;
                        if (i < _height / 2 && j > _width / 2) topRightQuad += count;
                        if (i > _height / 2 && j < _width / 2) bottomLeftQuad += count;
                        if (i > _height / 2 && j > _width / 2) bottomRightQuad += count;
                    }
                }
            }

            return topLeftQuad * topRightQuad * bottomLeftQuad * bottomRightQuad;
        }
    }

    public Bathroom(string[] lines, int width, int height)
    {
        _guards = lines.Select(l =>
        {
            var match = _guardConfigRegex.Match(l);

            return new GuardConfig((int.Parse(match.Groups[2].Value), int.Parse(match.Groups[1].Value)),
                (int.Parse(match.Groups[4].Value), int.Parse(match.Groups[3].Value)));
        }).ToArray();

        _width = width;
        _height = height;
    }

    public void MoveGuards()
    {
        _guards = _guards.Select(gp => gp with { Position = UpdateGuardPosition(gp) }).ToArray();
    }

    private Point UpdateGuardPosition(GuardConfig guardPosition)
    {
        int newRow = guardPosition.Position.Row + guardPosition.Velocity.Row;
        int newCol = guardPosition.Position.Col + guardPosition.Velocity.Col;

        if (newRow < 0) newRow = _height + newRow;
        else if (newRow >= _height) newRow %= _height;

        if (newCol < 0) newCol = _width + newCol;
        else if (newCol >= _width) newCol %= _width;

        return (newRow, newCol);
    }
}