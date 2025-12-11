// Disclaimer: I had to ask for non-trivial LLM help on part 2.
//
// Also for this one I'm not even sure I got there
// I didn't submit my part 2 answer to AoC though

public class Day9
{
    public static void Solve()
    {
        SolvePart1();
        SolvePart2();
    }

    private record Coord(int x, int y);

    private static List<Coord> ReadInput()
    {
        return System
            .IO.File.ReadAllText("Day9_Sample.txt")
            .Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(line =>
            {
                int[] parts = line.Split(",").Select(int.Parse).ToArray();
                return new Coord(parts[0], parts[1]);
            })
            .ToList();
    }

    private static List<List<Coord>> buildPairings(List<Coord> coords)
    {
        var results = new List<List<Coord>>();
        for (int i = 0; i < coords.Count() - 1; i++)
        {
            var others = coords.Where(coord => coord != coords[i]);
            foreach (var other in others)
            {
                var result = new List<Coord>() { coords[i], other };
                results.Add(result);
            }
        }
        return results;
    }

    private static int getSize(List<Coord> pairing)
    {
        var a = pairing[0];
        var b = pairing[1];
        var x = Math.Abs(a.x - b.x) + 1;
        var y = Math.Abs(a.y - b.y) + 1;
        return x * y;
    }

    private static void SolvePart1()
    {
        int result = 0;
        var coords = ReadInput();
        var pairings = buildPairings(coords);
        foreach (var pairing in pairings)
        {
            var size = getSize(pairing);
            if (size > result)
                result = size;
        }
        Console.WriteLine($"Part 1 Answer: {result}");
    }

    private static bool surrounded(long x, long y, List<Coord> coords)
    {
        var sharesX = coords.FindAll(c => c.x == x);
        var sharesY = coords.FindAll(c => c.y == y);
        var hasLesserX = sharesY.Exists(c => c.x <= x);
        var hasLesserY = sharesX.Exists(c => c.x <= x);

        return false;
    }

    private static bool onlyGreen(List<Coord> pairing, List<Coord> coords)
    {
        var coord = pairing[0];
        var other = pairing[1];
        int minX = Math.Min(coord.x, other.x);
        int maxX = Math.Max(coord.x, other.x);
        int minY = Math.Min(coord.y, other.y);
        int maxY = Math.Max(coord.y, other.y);

        for (int x = minX; x < maxX; x++)
        for (int y = minY; y < maxY; y++)
            if (!surrounded(x, y, coords))
            {
                return false;
            }
        return true;
    }

    private static void SolvePart2()
    {
        int result = 0;
        var coords = ReadInput();
        var pairings = buildPairings(coords);

        foreach (var pairing in pairings)
        {
            var size = getSize(pairing);
            if (size > result && onlyGreen(pairing, coords))
            {
                result = size;
            }
        }
        Console.WriteLine($"Part 2 Answer: {result}");
    }
}
