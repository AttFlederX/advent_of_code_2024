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
                        double x1 = locs[i].Column;
                        double x2 = locs[j].Column;
                        double y1 = locs[i].Row;
                        double y2 = locs[j].Row;

                        for (int x = 0; x < _mapSize; x++)
                        {
                            var y = y1 + (y2 - y1) / (x2 - x1) * (x - x1);
                            if (y >= 0 && y < _mapSize && y - (int)y < Math.Pow(10, -10))
                            {
                                antiNodes.Add(new((int)y, x));
                            }
                        }
                    }
                }

                return antiNodes;
            })
            .SelectMany(l => l)
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