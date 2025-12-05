public class Day5
{
    public static void Solve()
    {
        SolvePart1();
        SolvePart2();
    }

    private static string ReadInput()
    {
        return System.IO.File.ReadAllText("Day5_Input.txt");
    }

    private static void SolvePart1()
    {
        long result = 0;
        string[] lines = ReadInput().Split("\n");
        var ranges = new List<string>();
        var ingredientIds = new List<long>();
        bool finishedParsingRanges = false;

        foreach (string line in lines)
        {
            if (line.Length == 0)
                finishedParsingRanges = true;
            else
            {
                if (finishedParsingRanges)
                    ingredientIds.Add(long.Parse(line));
                else
                    ranges.Add(line);
            }
        }

        bool isFresh(long ingredientId, List<string> ranges)
        {
            foreach (string range in ranges)
            {
                string[] parts = range.Split("-");
                long start = long.Parse(parts[0]);
                long end = long.Parse(parts[1]);
                if (start <= ingredientId && end >= ingredientId)
                    return true;
            }
            return false;
        }

        foreach (long ingredientId in ingredientIds)
            if (isFresh(ingredientId, ranges))
                result += 1;

        Console.WriteLine($"Part 1 Answer: {result}");
    }

    private struct Interval
    {
        public long Start { get; }
        public long End { get; }

        public Interval(long start, long end)
        {
            Start = start;
            End = end;
        }
    }

    private static void SolvePart2()
    {
        List<string> lines = new HashSet<string>(ReadInput().Split("\n"))
            .ToList()
            .FindAll(x => x.Contains("-"));

        var intervals = lines
            .Select(line => line.Split("-"))
            .Select(parts => new Interval(long.Parse(parts[0]), long.Parse(parts[1])))
            .OrderBy(interval => interval.Start)
            .ToList();

        var nonOverlapping = new List<Interval> { intervals[0] };

        foreach (var interval in intervals.Skip(1))
        {
            var current = nonOverlapping[nonOverlapping.Count - 1];
            if (interval.Start <= current.End + 1)
                nonOverlapping[nonOverlapping.Count - 1] = new Interval(
                    current.Start,
                    Math.Max(current.End, interval.End)
                );
            else
                nonOverlapping.Add(interval);
        }
        long result = nonOverlapping.Sum(interval => interval.End - interval.Start + 1);
        Console.WriteLine($"Part 2 Answer: {result}");
    }
}
