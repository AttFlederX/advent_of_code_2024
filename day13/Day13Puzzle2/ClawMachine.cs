using System.Text.RegularExpressions;

class ClawMachine
{
    private decimal[,] _actionOffsets = new decimal[2, 2];
    private decimal[] _prize = new decimal[2];

    public ClawMachine(string[] lines)
    {
        var buttonA = Regex.Match(lines[0], @"Button A: X\+(\d+), Y\+(\d+)");
        var buttonB = Regex.Match(lines[1], @"Button B: X\+(\d+), Y\+(\d+)");
        var prize = Regex.Match(lines[2], @"Prize: X=(\d+), Y=(\d+)");

        _actionOffsets[0, 0] = decimal.Parse(buttonA.Groups[1].Value);
        _actionOffsets[0, 1] = decimal.Parse(buttonB.Groups[1].Value);
        _actionOffsets[1, 0] = decimal.Parse(buttonA.Groups[2].Value);
        _actionOffsets[1, 1] = decimal.Parse(buttonB.Groups[2].Value);

        _prize[0] = decimal.Parse(prize.Groups[1].Value + 10000000000000);
        _prize[1] = decimal.Parse(prize.Groups[2].Value + 10000000000000);
    }

    public decimal CalculateCheapestWin()
    {
        decimal eps = 0;
        decimal quot = _actionOffsets[1, 0] / _actionOffsets[0, 0];
        decimal b = (_prize[1] - (_prize[0] * quot)) / (_actionOffsets[1, 1] - (_actionOffsets[0, 1] * quot));
        if (b % 1 >= eps && b % 1 <= 1 - eps) return 0;

        decimal a = (_prize[0] / _actionOffsets[0, 0]) - ((_actionOffsets[0, 1] / _actionOffsets[0, 0]) * b);
        if (a % 1 >= eps && a % 1 <= 1 - eps) return 0;

        System.Console.WriteLine($"A: {a}, B: {b}");
        return 3 * Math.Round(a) + Math.Round(b);
    }
}