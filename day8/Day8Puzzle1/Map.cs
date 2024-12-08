readonly record struct MapLocation(int Row, int Column);

class Map
{
    int _mapSize;
    Dictionary<char, List<MapLocation>> _freqMap;

    public Map(string[] lines)
    {
        var map = lines.Select(l => l.ToCharArray()).ToArray();

        _mapSize = lines.Length;
        _freqMap = ScanMap(map);
    }

    public List<MapLocation> FindAntiNodes()
    {
        return _freqMap.Keys
            .Select(freq =>
            {
                var locs = _freqMap[freq];
                List<MapLocation> antiNodes = new();

                for (int i = 0; i < locs.Count; i++)
                {
                    for (int j = i + 1; j < locs.Count; j++)
                    {
                        var deltaRow = locs[j].Row - locs[i].Row;
                        var deltaColumn = locs[j].Column - locs[i].Column;

                        antiNodes.AddRange([
                            new(locs[i].Row - deltaRow, locs[i].Column - deltaColumn),
                            new(locs[j].Row + deltaRow, locs[j].Column + deltaColumn)
                        ]);
                    }
                }

                return antiNodes;
            })
            .SelectMany(l => l)
            .Where(l => l.Row >= 0 && l.Row < _mapSize && l.Column >= 0 && l.Column < _mapSize)
            .ToList();
    }

    private Dictionary<char, List<MapLocation>> ScanMap(char[][] map)
    {
        var res = new Dictionary<char, List<MapLocation>>();
        for (int i = 0; i < _mapSize; i++)
        {
            for (int j = 0; j < _mapSize; j++)
            {
                if (map[i][j] == '.') continue;
                res[map[i][j]] = [.. res.GetValueOrDefault(map[i][j], []), new MapLocation(i, j)];
            }
        }

        return res;
    }

}