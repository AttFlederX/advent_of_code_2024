
record Address(int Row, int Col);

class Ram
{
    private int _size;
    private Address[] _fallenBytes;

    public Ram(int size, int bytes, string[] lines)
    {
        _size = size;
        _fallenBytes = lines.Take(bytes).Select(l =>
        {
            var coords = l.Split(',');
            return new Address(int.Parse(coords[0]), int.Parse(coords[1]));
        }).ToArray();
    }

    public int FindShortestEscapeRoute()
    {
        return BreadthSearch(new(0, 0));
    }

    private int BreadthSearch(Address from)
    {
        var searchQueue = new Queue<Address>();
        var isVisited = new Dictionary<Address, bool>();
        var parents = new Dictionary<Address, Address>();

        searchQueue.Enqueue(from);

        while (searchQueue.Count > 0)
        {
            var address = searchQueue.Dequeue();

            if (address.Row == _size - 1 && address.Col == _size - 1)
            {
                return GetPathLength(address, parents);
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
                    !isVisited.GetValueOrDefault(address, false) &&
                    !_fallenBytes.Contains(adjacentAddress))
                {
                    isVisited.Add(adjacentAddress, true);
                    parents.Add(adjacentAddress, address);
                    searchQueue.Enqueue(adjacentAddress);
                }
            }
        }

        return -1;
    }

    private int GetPathLength(Address address, Dictionary<Address, Address> parents)
    {
        var length = 0;
        var addr = address;

        while (true)
        {
            length++;

            if (parents.TryGetValue(addr, out var nextAddr))
            {
                addr = nextAddr;
            }
            else
            {
                break;
            }
        }

        return length;
    }
}