using Map = System.Collections.Generic.Dictionary<string, string[]>;

public class Day11
{
    public static void Solve()
    {
        SolvePart1();
        SolvePart2();
    }

    private static string[] ReadInput()
    {
        return System
            .IO.File.ReadAllText("Day11_Input.txt")
            .Split("\n", StringSplitOptions.RemoveEmptyEntries);
    }

    private static void SolvePart1()
    {
        long result = 0;
        var lines = ReadInput();
        var map = new Map();
        foreach (var line in lines)
        {
            var parts = line.Split(":", 2);
            map[parts[0]] = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        }
        result = findPathsTo("you", "out", map, []);
        Console.WriteLine($"Part 1 Answer: {result}");
    }

    private static Dictionary<string, long> pathCounts = new Dictionary<string, long>();

    private static long findPathsTo(string start, string end, Map map, string[] requiredVisits)
    {
        var pathKey = $"{start}-{end}-{string.Join("_", requiredVisits)}";
        if (pathCounts.ContainsKey((pathKey)))
            return pathCounts[pathKey];

        long counts = 0;
        foreach (var node in map[start])
        {
            if (node == end && requiredVisits.Length == 0)
            {
                counts += 1;
            }
            else if (!map.ContainsKey(node))
            {
                continue;
            }
            else
            {
                var newRequirements = requiredVisits.Where(n => n != node).ToArray();
                counts += findPathsTo(node, end, map, newRequirements);
            }
        }
        pathCounts[pathKey] = counts;
        return counts;
    }

    private static void SolvePart2()
    {
        var lines = ReadInput();
        var map = new Map();
        foreach (var line in lines)
        {
            var parts = line.Split(":", 2);
            map[parts[0]] = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        }
        long result = findPathsTo("svr", "out", map, ["fft", "dac"]);
        Console.WriteLine($"Part 2 Answer: {result}");
    }
}
