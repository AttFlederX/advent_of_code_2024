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
                List<Plot> neighborPlots = [];
                if (i > 0 && _plotMap[i - 1][j].Plant == _plotMap[i][j].Plant)
                {
                    neighborPlots.Add(_plotMap[i - 1][j]);
                }
                if (j > 0 && _plotMap[i][j - 1].Plant == _plotMap[i][j].Plant)
                {
                    neighborPlots.Add(_plotMap[i][j - 1]);
                }
                if (i < _size - 1 && _plotMap[i + 1][j].Plant == _plotMap[i][j].Plant)
                {
                    neighborPlots.Add(_plotMap[i + 1][j]);
                }
                if (j < _size - 1 && _plotMap[i][j + 1].Plant == _plotMap[i][j].Plant)
                {
                    neighborPlots.Add(_plotMap[i][j + 1]);
                }

                var plotWithRegion = neighborPlots.FirstOrDefault(p => p.RegionId >= 0);

                if (plotWithRegion != null)
                {
                    if (_plotMap[i][j].RegionId != plotWithRegion.RegionId)
                    {
                        CopyRegionInfo(i, j, plotWithRegion);
                    }

                    foreach (var np in neighborPlots)
                    {
                        if (np.RegionId != plotWithRegion.RegionId)
                        {
                            CopyRegionInfo(np.Row, np.Col, plotWithRegion);
                        }
                    }
                }
                else
                {
                    if (_plotMap[i][j].RegionId < 0)
                    {
                        _plotMap[i][j] = _plotMap[i][j] with { RegionId = regionId };
                        _regionMap.Add(regionId, new Region(_plotMap[i][j].Plant, [_plotMap[i][j]]));

                        regionId++;
                    }

                    neighborPlots.ForEach(np => CopyRegionInfo(np.Row, np.Col, _plotMap[i][j]));
                }
            }
        }

        _regionMap = _regionMap.Where(r => r.Value.Area > 0).ToDictionary();
    }

    public long CalculatePrice()
    {
        return _regionMap.Values.Select(r => r.Area * r.Perimeter).Sum();
    }

    private void CopyRegionInfo(int toRow, int toCol, Plot from)
    {
        _plotMap[toRow][toCol] = _plotMap[toRow][toCol] with { RegionId = from.RegionId };
        _regionMap[_plotMap[toRow][toCol].RegionId].Plots.Add(_plotMap[toRow][toCol]);
    }
}

class Region(char plant, List<Plot> plots)
{
    public char Plant { get; private set; } = plant;
    public List<Plot> Plots { get; private set; } = plots;

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