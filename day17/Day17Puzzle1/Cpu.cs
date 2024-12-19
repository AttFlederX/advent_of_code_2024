class Cpu
{
    enum Instruction
    {
        ADV = 0,
        BXL = 1,
        BST = 2,
        JNZ = 3,
        BXC = 4,
        OUT = 5,
        BDV = 6,
        CDV = 7
    }

    private int _InsPtr;
    private int _A;
    private int _B;
    private int _C;

    public Cpu(string[] lines)
    {
        _InsPtr = 0;

        _A = int.Parse(lines[0].Split(": ").Last());
        _B = int.Parse(lines[1].Split(": ").Last());
        _C = int.Parse(lines[2].Split(": ").Last());
    }

    public string RunProgram(string input)
    {
        var program = input.Replace(",", "").Select(c => c - '0').ToArray();
        List<int> stdOut = [];

        while (_InsPtr < program.Length)
        {
            var ins = (Instruction)program[_InsPtr];
            _InsPtr++;

            var op = program[_InsPtr];
            _InsPtr++;

            switch (ins)
            {
                case Instruction.ADV:
                    op = ParseCombo(op);
                    _A = (int)(_A / Math.Pow(2, op));
                    break;
                case Instruction.BXL:
                    _B ^= op;
                    break;
                case Instruction.BST:
                    _B = ParseCombo(op) % 8;
                    break;
                case Instruction.JNZ:
                    if (_A != 0)
                        _InsPtr = op;
                    break;
                case Instruction.BXC:
                    _B ^= _C;
                    break;
                case Instruction.OUT:
                    stdOut.Add(ParseCombo(op) % 8);
                    break;
                case Instruction.BDV:
                    op = ParseCombo(op);
                    _B = (int)(_A / Math.Pow(2, op));
                    break;
                case Instruction.CDV:
                    op = ParseCombo(op);
                    _C = (int)(_A / Math.Pow(2, op));
                    break;
            }
        }

        return string.Join(',', stdOut);
    }

    private int ParseCombo(int op)
    {
        if (op < 4) return op;
        if (op == 4) return _A;
        if (op == 5) return _B;
        if (op == 6) return _C;

        return -1;
    }
}