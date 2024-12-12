using System.Formats.Asn1;

record Plot(char Plant, int Row, int Col, int RegionId);

class Garden
{
    private int _size;
    private Plot[][] _plotMap;
    private Dictionary<int, Region> _regionMap = [];

    public Garden(string[] lines)
    {
        _size = lines.Length;
        _plotMap = lines.Select((l, i) => l.Select((c, j) => new Plot(c, i, j, -1)).ToArray()).ToArray();

        var regionId = 0;
        for (int i = 0; i < _size; i++)
        {
            for (int j = 0; j < _size; j++)
            {
                var plot = _plotMap[i][j];
                if (plot.RegionId >= 0) continue;

                _regionMap.Add(regionId, new Region(plot.Plant));
                FindNeighbors(plot.Plant, regionId, i, j);

                regionId++;
            }
        }

        _regionMap = _regionMap.Where(r => r.Value.Area > 0).ToDictionary();
    }

    public long CalculatePrice()
    {
        return _regionMap.Values.Select(r => r.Area * r.Perimeter).Sum();
    }

    private void FindNeighbors(char plant, int regionId, int row, int col)
    {
        var plot = _plotMap[row][col];
        if (plot.Plant != plant || plot.RegionId == regionId) return;

        _plotMap[row][col] = plot with { RegionId = regionId };
        _regionMap[regionId].Plots.Add(_plotMap[row][col]);

        if (row > 0)
        {
            FindNeighbors(plant, regionId, row - 1, col);
        }
        if (col > 0)
        {
            FindNeighbors(plant, regionId, row, col - 1);
        }
        if (row < _size - 1)
        {
            FindNeighbors(plant, regionId, row + 1, col);
        }
        if (col < _size - 1)
        {
            FindNeighbors(plant, regionId, row, col + 1);
        }
    }
}

class Region(char plant)
{
    public char Plant { get; private set; } = plant;
    public List<Plot> Plots { get; private set; } = [];

    public int Area => Plots.Count;

    public int Perimeter
    {
        get
        {
            var perimeter = 0;

            foreach (var plot in Plots)
            {
                if (!Plots.Contains(plot with { Row = plot.Row - 1 })) perimeter++;
                if (!Plots.Contains(plot with { Row = plot.Row + 1 })) perimeter++;
                if (!Plots.Contains(plot with { Col = plot.Col - 1 })) perimeter++;
                if (!Plots.Contains(plot with { Col = plot.Col + 1 })) perimeter++;
            }

            return perimeter;
        }
    }
}