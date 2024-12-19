class Node(NodeType Type, int Row, int Col)
{
    public NodeType Type { get; } = Type;
    public int Row { get; } = Row;
    public int Col { get; } = Col;
}

class Vertex(Node Node, Direction Direction)
{
    public Node Node { get; } = Node;
    public Direction Direction { get; } = Direction;
    public int Score { get; set; } = int.MaxValue;
    public Vertex? Parent { get; set; }
    public List<Vertex> Alternates { get; set; } = [];
    public bool IsDiscovered { get; set; }
}

enum NodeType
{
    Start,
    Path,
    Wall,
    End,
}

enum Direction
{
    North,
    South,
    West,
    East,
}

class Maze
{
    private Node[][] _map;
    private Node _start;

    public Maze(string[] lines)
    {
        _start = new(NodeType.Start, 0, 0);
        _map = lines.Select((l, i) => l.Select((c, j) =>
        {
            var type = GetVertexType(c);
            var point = new Node(type, i, j);
            if (c == 'S')
            {
                _start = point;
            }

            return point;
        }).ToArray()).ToArray();
    }

    public int FindSpectatingSpots()
    {
        var end = FindLowestScorePaths();
        List<Vertex> spots = [];

        ExploreVertex(end, spots);

        return spots.Count;
    }

    private void ExploreVertex(Vertex vertex, List<Vertex> spots)
    {
        if (!spots.Any(v => v.Node.Row == vertex.Node.Row && v.Node.Col == vertex.Node.Col))
            spots.Add(vertex);

        if (vertex.Parent != null)
        {
            ExploreVertex(vertex.Parent, spots);
        }

        foreach (var alt in vertex.Alternates)
        {
            ExploreVertex(alt, spots);
        }
    }

    public Vertex FindLowestScorePaths()
    {
        var direction = Direction.East;
        Vertex[] flatMap = _map
            .SelectMany(r => r)
            .Where(n => n.Type != NodeType.Wall)
            .Select(n => Enum.GetValues<Direction>()
            .Select(d => new Vertex(n, d)))
            .SelectMany(v => v)
            .ToArray();

        flatMap.First(v => v.Node == _start && v.Direction == Direction.East).Score = 0;

        while (true)
        {
            var filteredFlatMap = flatMap.Where(v => !v.IsDiscovered);
            var minScore = filteredFlatMap.Min(v => v.Score);
            var vertex = filteredFlatMap.First(v => v.Score == minScore);

            if (vertex.Node.Type == NodeType.End)
                return vertex;

            if (vertex.Parent != null)
            {
                direction = GetTravelDirection(vertex.Parent.Node, vertex.Node);
            }

            vertex.IsDiscovered = true;

            Vertex[] nextVertices = filteredFlatMap.Where(v =>
                (v.Node.Row == vertex.Node.Row - 1 && v.Node.Col == vertex.Node.Col && v.Direction == Direction.North) ||
                (v.Node.Row == vertex.Node.Row + 1 && v.Node.Col == vertex.Node.Col && v.Direction == Direction.South) ||
                (v.Node.Row == vertex.Node.Row && v.Node.Col == vertex.Node.Col - 1 && v.Direction == Direction.West) ||
                (v.Node.Row == vertex.Node.Row && v.Node.Col == vertex.Node.Col + 1 && v.Direction == Direction.East)
            ).ToArray();

            foreach (var nextVertex in nextVertices)
            {
                if (nextVertex != null)
                {
                    var nextScore = vertex.Score + 1;

                    if (nextVertex.Direction != direction)
                    {
                        nextScore += 1000;
                    }

                    if (nextScore == nextVertex.Score)
                    {
                        nextVertex.Alternates.Add(vertex);
                    }
                    if (nextScore < nextVertex.Score)
                    {
                        nextVertex.Score = nextScore;

                        nextVertex.Alternates.Clear();
                        nextVertex.Parent = vertex;
                    }
                }
            }
        }
    }

    private NodeType GetVertexType(char c)
    {
        switch (c)
        {
            case 'S':
                return NodeType.Start;
            case '.':
                return NodeType.Path;
            case '#':
                return NodeType.Wall;
            case 'E':
                return NodeType.End;
        }

        return default;
    }

    private Direction GetTravelDirection(Node prevVertex, Node nextVertex)
    {
        if (nextVertex.Row < prevVertex.Row) return Direction.North;
        if (nextVertex.Row > prevVertex.Row) return Direction.South;
        if (nextVertex.Col < prevVertex.Col) return Direction.West;
        if (nextVertex.Col > prevVertex.Col) return Direction.East;

        return default;
    }
}