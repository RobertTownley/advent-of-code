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
        int result = 0;
        var lines = ReadInput();
        var map = new Map();
        foreach (var line in lines)
        {
            var parts = line.Split(":", 2);
            map[parts[0]] = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        }
        result = findPathsTo("you", "out", map);
        Console.WriteLine($"Part 1 Answer: {result}");
    }

    private static Dictionary<string, int> pathCounts = new Dictionary<string, int>();
    private static Dictionary<string, (int, bool, bool)> pathDetails =
        new Dictionary<string, (int, bool, bool)>();
    private static Dictionary<string, bool> visitsDac = new Dictionary<string, bool>();
    private static Dictionary<string, bool> visitsFft = new Dictionary<string, bool>();

    private static int findPathsTo(string start, string end, Map map)
    {
        var pathKey = $"{start}-{end}";
        if (pathCounts.ContainsKey((pathKey)))
            return pathCounts[pathKey];

        int counts = 0;
        foreach (var node in map[start])
        {
            if (map[node].Contains(end))
            {
                counts += 1;
            }
            else
            {
                counts += findPathsTo(node, end, map);
            }
        }
        pathCounts[pathKey] = counts;
        return counts;
    }

    private static (int, bool, bool) findPathsToWithStops(string start, string end, Map map)
    {
        var pathKey = $"{start}-{end}";
        if (pathDetails.ContainsKey((pathKey)))
            return pathDetails[pathKey];

        int counts = 0;
        bool visitsDac = false;
        bool visitsFft = false;
        foreach (var node in map[start])
        {
            if (map[node].Contains(end))
            {
                counts += 1;
                visitsDac = node == "dac" || end == "dac";
                visitsFft = node == "fft" || end == "fft";
            }
            else
            {
                (int toAdd, visitsDac, visitsFft) = findPathsToWithStops(node, end, map);
                counts += toAdd;
            }
        }
        pathDetails[pathKey] = (counts, visitsDac, visitsFft);
        return pathDetails[pathKey];
    }

    private static void SolvePart2()
    {
        int result = 0;
        var lines = ReadInput();
        var map = new Map();
        foreach (var line in lines)
        {
            var parts = line.Split(":", 2);
            map[parts[0]] = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        }
        (result, _, _) = findPathsToWithStops("svr", "out", map);
        Console.WriteLine($"Part 2 Answer: {result}");
    }
}
