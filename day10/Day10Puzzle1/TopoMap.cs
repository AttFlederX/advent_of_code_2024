using System.Text;

record struct MapPoint(int Row, int Col);

class TopoMap
{
    private int _mapSize;
    private int[][] _topoMap;
    private List<MapPoint> _trailheads = [];

    public TopoMap(IEnumerable<string> lines)
    {
        _mapSize = lines.Count();
        _topoMap = lines.Select((l, i) => l.Select((c, j) =>
            {
                if (c == '0') _trailheads.Add(new(i, j));
                return c - '0';
            })
        .ToArray())
        .ToArray();
    }

    public List<int> FindTrailHeadScores()
    {
        return _trailheads.Select(FindScore).ToList();
    }

    private int FindScore(MapPoint trailhead)
    {
        var reachedSummits = new HashSet<MapPoint>();

        DepthSearch(trailhead, reachedSummits);

        return reachedSummits.Count;
    }

    private void DepthSearch(MapPoint point, HashSet<MapPoint> reachedSummits)
    {
        if (_topoMap[point.Row][point.Col] == 9)
        {
            reachedSummits.Add(point);
            return;
        }

        MapPoint[] adjacentPoints = [
            point with {Row = point.Row - 1},
            point with {Row = point.Row + 1},
            point with {Col = point.Col - 1},
            point with {Col = point.Col + 1},
        ];

        foreach (var pt in adjacentPoints)
        {
            if (pt.Row >= 0 && pt.Row < _mapSize &&
                pt.Col >= 0 && pt.Col < _mapSize &&
                _topoMap[pt.Row][pt.Col] - _topoMap[point.Row][point.Col] == 1)
            {
                DepthSearch(pt, reachedSummits);
            }
        }
    }
}