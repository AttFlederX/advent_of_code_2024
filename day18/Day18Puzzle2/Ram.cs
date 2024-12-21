
record Address(int Row, int Col);

class Ram
{
    private int _size;
    private Address[] _fallenBytes;

    public Ram(int size, string[] lines)
    {
        _size = size;
        _fallenBytes = lines.Select(l =>
        {
            var coords = l.Split(',');
            return new Address(int.Parse(coords[0]), int.Parse(coords[1]));
        }).ToArray();
    }

    public Address FindCutOffByte(int from = 1)
    {
        var byteNum = from;
        while (HasEscapeRoute(byteNum) && byteNum <= _fallenBytes.Length)
            byteNum++;

        return _fallenBytes[byteNum - 1];
    }

    public bool HasEscapeRoute(int bytes)
    {
        return BreadthSearch(_fallenBytes.Take(bytes), new(0, 0));
    }

    private bool BreadthSearch(IEnumerable<Address> fallenBytes, Address from)
    {
        var searchQueue = new Queue<Address>();
        List<Address> visitedAddresses = [from];

        searchQueue.Enqueue(from);

        while (searchQueue.Count > 0)
        {
            var address = searchQueue.Dequeue();

            if (address.Row == _size - 1 && address.Col == _size - 1)
            {
                return true;
            }

            Address[] adjacentAddresses = [
                address with {Row = address.Row - 1},
                address with {Row = address.Row + 1},
                address with {Col = address.Col - 1},
                address with {Col = address.Col + 1},
            ];

            foreach (var adjacentAddress in adjacentAddresses)
            {
                if (adjacentAddress.Row >= 0 && adjacentAddress.Row < _size &&
                    adjacentAddress.Col >= 0 && adjacentAddress.Col < _size &&
                    !visitedAddresses.Contains(adjacentAddress) &&
                    !fallenBytes.Contains(adjacentAddress))
                {
                    visitedAddresses.Add(adjacentAddress);
                    searchQueue.Enqueue(adjacentAddress);
                }
            }
        }

        return false;
    }
}