public class Day8
{
    public static void Solve()
    {
        SolvePart1();
        SolvePart2();
    }

    private record Coord(int X, int Y, int Z);

    private static Dictionary<int, HashSet<int>> buildPairMap(
        IEnumerable<(int i, int j, double distance)> pairs,
        int coordsCount
    )
    {
        return pairs
            .SelectMany(pair => new[] { (from: pair.i, to: pair.j), (from: pair.j, to: pair.i) })
            .GroupBy(e => e.from)
            .ToDictionary(g => g.Key, g => g.Select(e => e.to).ToHashSet());
    }

    private static HashSet<int> findJunction(
        int start,
        Dictionary<int, HashSet<int>> pairMap,
        HashSet<int> connected
    )
    {
        if (connected.Contains(start))
            return new HashSet<int>();

        var component = new HashSet<int> { start };
        var queue = new Queue<int>(new[] { start });
        connected.Add(start);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            if (pairMap.TryGetValue(node, out var others))
            {
                foreach (var other in others.Where(n => !connected.Contains(n)))
                {
                    connected.Add(other);
                    component.Add(other);
                    queue.Enqueue(other);
                }
            }
        }

        return component;
    }

    private static List<HashSet<int>> buildCircuits(
        Dictionary<int, HashSet<int>> pairMap,
        int coordsCount
    )
    {
        var connected = new HashSet<int>();
        return Enumerable
            .Range(0, coordsCount)
            .Select(i => findJunction(i, pairMap, connected))
            .Where(c => c.Count > 0)
            .ToList();
    }

    private static Coord[] ReadInput()
    {
        var lines = System
            .IO.File.ReadAllText("Day8_Input.txt")
            .Split("\n", StringSplitOptions.RemoveEmptyEntries);

        var coords = new List<Coord>();
        foreach (var line in lines)
        {
            var parts = line.Split(',').Select(int.Parse).ToList();
            coords.Add(new Coord(parts[0], parts[1], parts[2]));
        }
        return coords.ToArray();
    }

    private static double getDistance(Coord a, Coord b)
    {
        long x = a.X - b.X;
        long y = a.Y - b.Y;
        long z = a.Z - b.Z;
        return Math.Sqrt(x * x + y * y + z * z);
    }

    private static List<(int i, int j, double distance)> buildPairs(Coord[] coords)
    {
        var pairs = new List<(int i, int j, double distance)>();

        for (int i = 0; i < coords.Length; i++)
        for (int j = i + 1; j < coords.Length; j++)
        {
            var dist = getDistance(coords[i], coords[j]);
            pairs.Add((i, j, dist));
        }

        pairs.Sort((a, b) => a.distance.CompareTo(b.distance));
        return pairs;
    }

    private static void SolvePart1()
    {
        var coords = ReadInput();
        var pairs = buildPairs(coords).Take(1000);
        var pairMap = buildPairMap(pairs, coords.Length);
        var circuits = buildCircuits(pairMap, coords.Length);

        var result = circuits
            .OrderByDescending(c => c.Count)
            .Take(3)
            .Select(c => c.Count)
            .Aggregate((a, b) => a * b);

        Console.WriteLine($"Part 1 Answer: {result}");
    }

    private static void SolvePart2()
    {
        var coords = ReadInput();
        var pairs = buildPairs(coords);

        var (lastBoxA, lastBoxB, _) = pairs
            .Select((pair, index) => (pair, pairCount: index + 1))
            .First(x =>
            {
                var pairMap = buildPairMap(pairs.Take(x.pairCount), coords.Length);
                var circuits = buildCircuits(pairMap, coords.Length);
                return circuits.Count == 1;
            })
            .pair;

        long result = coords[lastBoxA].X * coords[lastBoxB].X;
        Console.WriteLine($"Part 2 Answer: {result}");
    }
}
