using System.Text.RegularExpressions;

class ClawMachine
{
    private double[,] _actionOffsets = new double[2, 2];
    private double[] _prize = new double[2];

    public ClawMachine(string[] lines)
    {
        var buttonA = Regex.Match(lines[0], @"Button A: X\+(\d+), Y\+(\d+)");
        var buttonB = Regex.Match(lines[1], @"Button B: X\+(\d+), Y\+(\d+)");
        var prize = Regex.Match(lines[2], @"Prize: X=(\d+), Y=(\d+)");

        _actionOffsets[0, 0] = double.Parse(buttonA.Groups[1].Value);
        _actionOffsets[0, 1] = double.Parse(buttonB.Groups[1].Value);
        _actionOffsets[1, 0] = double.Parse(buttonA.Groups[2].Value);
        _actionOffsets[1, 1] = double.Parse(buttonB.Groups[2].Value);

        _prize[0] = double.Parse(prize.Groups[1].Value);
        _prize[1] = double.Parse(prize.Groups[2].Value);
    }

    public int CalculateCheapestWin()
    {
        double eps = Math.Pow(10, -3);
        double quot = _actionOffsets[1, 0] / _actionOffsets[0, 0];
        double b = (_prize[1] - (_prize[0] * quot)) / (_actionOffsets[1, 1] - (_actionOffsets[0, 1] * quot));
        if (b % 1 >= eps && b % 1 <= 1 - eps) return 0;

        double a = (_prize[0] / _actionOffsets[0, 0]) - ((_actionOffsets[0, 1] / _actionOffsets[0, 0]) * b);
        if (a % 1 >= eps && a % 1 <= 1 - eps) return 0;

        System.Console.WriteLine($"A: {a}, B: {b}");
        return (int)(3 * Math.Round(a) + Math.Round(b));
    }
}