class Equation
{
    private static readonly string[] operators = ["+", "*", "||"];
    private static readonly int operatorOptions = operators.Length;

    public long Result { get; private set; }
    private int[] _operands;

    public Equation(string lines)
    {
        var splitEq = lines.Split(": ");

        Result = long.Parse(splitEq.First());
        _operands = splitEq.Last().Split(" ").Select(op => int.Parse(op)).ToArray();
    }

    public bool Validate()
    {
        return GeneratePermutations().Any(perm => CalculateForPermutation(perm) == Result);
    }

    private string[][] GeneratePermutations()
    {
        var numOfOperators = _operands.Length - 1;
        var num = (int)Math.Pow(operatorOptions, numOfOperators);
        var perms = new string[num][];

        for (int i = 0; i < num; i++)
        {
            perms[i] = new string[numOfOperators];
            for (int j = 0; j < numOfOperators; j++)
            {
                var opIdx = i / (int)Math.Pow(operatorOptions, j) % operatorOptions;
                perms[i][j] = operators[opIdx];
            }
        }

        return perms;
    }

    private long CalculateForPermutation(string[] permutation)
    {
        long res = _operands.First();
        for (int i = 0; i < _operands.Length - 1; i++)
        {
            if (permutation[i] == "||")
            {
                res = long.Parse($"{res}{_operands[i + 1]}");
            }
            else if (permutation[i] == "+")
            {
                res += _operands[i + 1];
            }
            else
            {
                res *= _operands[i + 1];
            }
        }

        return res;
    }
}