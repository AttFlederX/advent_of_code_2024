var lines = File.ReadAllLines("input.txt");

var start = DateTime.Now;
var mtx = lines.Select(l => l.ToCharArray()).ToArray();
var res = 0;

for (int i = 0; i < mtx.Length; i++)
{
    for (int j = 0; j < mtx[i].Length; j++)
    {
        if (mtx[i][j] == 'X' || mtx[i][j] == 'S')
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
    var matches = 0;

    if (HasHorizontalMatch(i, j)) matches++;
    if (HasVerticalMatch(i, j)) matches++;
    if (HasOutwardDiagonalMatch(i, j)) matches++;
    if (HasInwardDiagonalMatch(i, j)) matches++;

    return matches;
}

bool HasHorizontalMatch(int i, int j)
{
    if (mtx[i][j] == 'X')
    {
        if (MatrixElementAtOrDefault(i, j + 1) != 'M' ||
            MatrixElementAtOrDefault(i, j + 2) != 'A' ||
            MatrixElementAtOrDefault(i, j + 3) != 'S')
        {
            return false;
        }
    }
    else if (mtx[i][j] == 'S')
    {
        if (MatrixElementAtOrDefault(i, j + 1) != 'A' ||
            MatrixElementAtOrDefault(i, j + 2) != 'M' ||
            MatrixElementAtOrDefault(i, j + 3) != 'X')
        {
            return false;
        }
    }

    return true;
}

bool HasVerticalMatch(int i, int j)
{
    if (mtx[i][j] == 'X')
    {
        if (MatrixElementAtOrDefault(i + 1, j) != 'M' ||
            MatrixElementAtOrDefault(i + 2, j) != 'A' ||
            MatrixElementAtOrDefault(i + 3, j) != 'S')
        {
            return false;
        }
    }
    else if (mtx[i][j] == 'S')
    {
        if (MatrixElementAtOrDefault(i + 1, j) != 'A' ||
            MatrixElementAtOrDefault(i + 2, j) != 'M' ||
            MatrixElementAtOrDefault(i + 3, j) != 'X')
        {
            return false;
        }
    }

    return true;
}

bool HasOutwardDiagonalMatch(int i, int j)
{
    if (mtx[i][j] == 'X')
    {
        if (MatrixElementAtOrDefault(i + 1, j + 1) != 'M' ||
            MatrixElementAtOrDefault(i + 2, j + 2) != 'A' ||
            MatrixElementAtOrDefault(i + 3, j + 3) != 'S')
        {
            return false;
        }
    }
    else if (mtx[i][j] == 'S')
    {
        if (MatrixElementAtOrDefault(i + 1, j + 1) != 'A' ||
            MatrixElementAtOrDefault(i + 2, j + 2) != 'M' ||
            MatrixElementAtOrDefault(i + 3, j + 3) != 'X')
        {
            return false;
        }
    }

    return true;
}

bool HasInwardDiagonalMatch(int i, int j)
{
    if (mtx[i][j] == 'X')
    {
        if (MatrixElementAtOrDefault(i + 1, j - 1) != 'M' ||
            MatrixElementAtOrDefault(i + 2, j - 2) != 'A' ||
            MatrixElementAtOrDefault(i + 3, j - 3) != 'S')
        {
            return false;
        }
    }
    else if (mtx[i][j] == 'S')
    {
        if (MatrixElementAtOrDefault(i + 1, j - 1) != 'A' ||
            MatrixElementAtOrDefault(i + 2, j - 2) != 'M' ||
            MatrixElementAtOrDefault(i + 3, j - 3) != 'X')
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