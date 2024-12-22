class TowelPicker(string patterns)
{
    private string[] _patterns = [.. patterns.Split(", ").OrderBy(p => p.Length)];

    public object FindAllValidCombinations(IEnumerable<string> designs)
    {
        Dictionary<string, string[]> designsMap = [];

        foreach (var design in designs)
        {
            designsMap.Add(design, _patterns.Where(design.Contains).ToArray());
        }
        designsMap = designsMap.Where(kv => kv.Value.Length > 0).ToDictionary();

        long res = 0;
        Dictionary<string, long> cache = [];
        foreach (var kv in designsMap)
        {
            res += ValidateDesign(kv.Key, kv.Value, cache);
        }

        return res;
    }

    private long ValidateDesign(string design, string[] patterns, Dictionary<string, long> cache)
    {
        if (cache.TryGetValue(design, out var validCombinations))
            return validCombinations;

        if (design.Length == 0)
            return 1;

        long totalCombinations = 0;
        foreach (var option in patterns)
        {
            if (design.StartsWith(option) && design.Length >= option.Length)
            {
                totalCombinations += ValidateDesign(design[option.Length..], patterns, cache);
            }
        }

        cache.Add(design, totalCombinations);
        return totalCombinations;
    }
}