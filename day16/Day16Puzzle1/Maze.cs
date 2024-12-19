using Vertex = (Node Node, Direction Direction);

class Node(NodeType Type, int Row, int Col)
{
    public NodeType Type { get; } = Type;
    public int Row { get; } = Row;
    public int Col { get; } = Col;
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

    public int FindLowestScore()
    {
        var direction = Direction.East;
        Vertex[] flatMap = _map
            .SelectMany(r => r)
            .Select(n => Enum.GetValues<Direction>()
            .Select(d => (n, d)))
            .SelectMany(v => v)
            .ToArray();

        HashSet<Vertex> visitedVertices = [];
        Dictionary<Vertex, Vertex> parents = [];
        Dictionary<Vertex, int> scores = [];

        scores.Add((_start, Direction.East), 0);

        while (true)
        {
            var filteredFlatMap = flatMap.Where(v => !visitedVertices.Contains(v));
            var minScore = filteredFlatMap.Min(v => scores.GetValueOrDefault(v, int.MaxValue));
            var vertex = filteredFlatMap.First(v => scores.GetValueOrDefault(v, int.MaxValue) == minScore);

            if (vertex.Node.Type == NodeType.End)
                return scores[vertex];

            if (parents.TryGetValue(vertex, out var parent))
            {
                direction = GetTravelDirection(parent.Node, vertex.Node);
            }

            visitedVertices.Add(vertex);

            Vertex[] nextVertices = [
                (_map[vertex.Node.Row - 1][vertex.Node.Col], Direction.North),
                (_map[vertex.Node.Row + 1][vertex.Node.Col], Direction.South),
                (_map[vertex.Node.Row][vertex.Node.Col - 1], Direction.West),
                (_map[vertex.Node.Row][vertex.Node.Col + 1], Direction.East),
            ];

            foreach (var nextVertex in nextVertices)
            {
                if (nextVertex.Node.Type != NodeType.Wall && !visitedVertices.Contains(nextVertex))
                {
                    var nextWeight = scores[vertex] + 1;

                    if (nextVertex.Direction != direction)
                    {
                        nextWeight += 1000;
                    }

                    if (nextWeight < scores.GetValueOrDefault(nextVertex, int.MaxValue))
                    {
                        if (!scores.TryAdd(nextVertex, nextWeight))
                        {
                            scores[nextVertex] = nextWeight;
                        }
                        if (!parents.TryAdd(nextVertex, vertex))
                        {
                            parents[nextVertex] = vertex;
                        }
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

    // private void PrintMaze(Node result)
    // {
    //     var vertex = result;
    //     Stack<Node> path = [];

    //     while (vertex != null)
    //     {
    //         path.Push(vertex);
    //         vertex = vertex.Parent!;
    //     }

    //     for (int i = 0; i < _map.Length; i++)
    //     {
    //         for (int j = 0; j < _map.Length; j++)
    //         {
    //             if (path.Contains(_map[i][j])) Console.Write('O');
    //             else if (_map[i][j].Type == NodeType.Path) Console.Write('.');
    //             else if (_map[i][j].Type == NodeType.Wall) Console.Write('#');
    //         }
    //         Console.WriteLine();
    //     }
    // }
}