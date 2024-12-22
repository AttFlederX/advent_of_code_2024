
class TowelPicker
{
    private string[] _patterns;
    public TowelPicker(string patterns)
    {
        _patterns = [.. patterns.Split(", ").OrderBy(p => p.Length)];
    }

    public object FindValidCombinations(IEnumerable<string> designs)
    {
        var res = 0;

        foreach (var design in designs)
        {
            var patternMatch = design;
            foreach (var pattern in _patterns)
            {
                patternMatch = patternMatch.Replace(pattern, "");
                if (patternMatch.Length == 0)
                {
                    break;
                }
            }

            if (patternMatch.Length == 0)
            {
                res++;
            }
        }

        return res;
    }
}