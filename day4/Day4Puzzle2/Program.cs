var lines = File.ReadAllLines("input.txt");
var start = DateTime.Now;
var res = 0;

var mtx = lines.Select(l => l.ToCharArray()).ToArray();

for (int i = 0; i < mtx.Length; i++)
{
    for (int j = 0; j < mtx[i].Length; j++)
    {
        if (mtx[i][j] == 'M' || mtx[i][j] == 'S')
        {
            res += FindMatchesForCell(i, j);
        }
    }
}

var duration = DateTime.Now - start;
Console.WriteLine(res);
Console.WriteLine($"Time: {duration.TotalMilliseconds} ms");


int FindMatchesForCell(int i, int j)
{
    if (mtx[i][j] == 'M')
    {
        if (MatrixElementAtOrDefault(i + 1, j + 1) != 'A' ||
            MatrixElementAtOrDefault(i + 2, j + 2) != 'S' ||
            !CheckOppositeDiagonal(i, j))
        {
            return 0;
        }
    }
    else if (mtx[i][j] == 'S')
    {
        if (MatrixElementAtOrDefault(i + 1, j + 1) != 'A' ||
            MatrixElementAtOrDefault(i + 2, j + 2) != 'M' ||
            !CheckOppositeDiagonal(i, j))
        {
            return 0;
        }
    }

    return 1;
}

bool CheckOppositeDiagonal(int i, int j)
{
    if (MatrixElementAtOrDefault(i, j + 2) != 'M' && MatrixElementAtOrDefault(i, j + 2) != 'S')
    {
        return false;
    }

    if (MatrixElementAtOrDefault(i, j + 2) == 'M')
    {
        if (MatrixElementAtOrDefault(i + 2, j) != 'S')
        {
            return false;
        }
    }
    else if (MatrixElementAtOrDefault(i, j + 2) == 'S')
    {
        if (MatrixElementAtOrDefault(i + 2, j) != 'M')
        {
            return false;
        }
    }

    return true;
}

char? MatrixElementAtOrDefault(int i, int j)
{
    return mtx.ElementAtOrDefault(i)?.ElementAtOrDefault(j);
}