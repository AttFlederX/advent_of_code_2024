class Equation
{
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
        var perms = GeneratePermutations();
        return GeneratePermutations().Any(perm => CalculateForPermutation(perm) == Result);
    }

    private char[][] GeneratePermutations()
    {
        var ops = _operands.Length - 1;
        var num = (int)Math.Pow(2, ops);
        var perms = new char[num][];

        for (int i = 0; i < num; i++)
        {
            perms[i] = new char[ops];
            for (int j = 0; j < ops; j++)
            {
                var opIdx = i / (int)Math.Pow(2, j) % 2;
                perms[i][j] = opIdx == 1 ? '*' : '+';
            }
        }

        return perms;
    }

    private long CalculateForPermutation(char[] permutation)
    {
        long res = _operands.First();
        for (int i = 0; i < _operands.Length - 1; i++)
        {
            if (permutation[i] == '+')
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