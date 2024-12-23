class TowelPicker(string patterns)
{
    private string[] _patterns = [.. patterns.Split(", ").OrderBy(p => p.Length)];

    public object FindValidCombinations(IEnumerable<string> designs)
    {
        Dictionary<string, string[]> designsMap = [];

        foreach (var design in designs)
        {
            designsMap.Add(design, _patterns.Where(design.Contains).ToArray());
        }
        designsMap = designsMap.Where(kv => kv.Value.Length > 0).ToDictionary();

        var res = 0;
        Dictionary<string, bool> cache = [];
        foreach (var kv in designsMap)
        {
            if (ValidateDesign(kv.Key, kv.Value, cache))
                res++;
        }

        return res;
    }

    private bool ValidateDesign(string design, string[] patterns, Dictionary<string, bool> cache)
    {
        if (cache.TryGetValue(design, out var isValid))
            return isValid;

        if (design.Length == 0)
            return true;

        foreach (var option in patterns)
        {
            if (design.StartsWith(option) && design.Length >= option.Length)
            {
                if (ValidateDesign(design[option.Length..], patterns, cache))
                {
                    cache.Add(design, true);
                    return true;
                }
            }
        }

        cache.Add(design, false);
        return false;
    }
}