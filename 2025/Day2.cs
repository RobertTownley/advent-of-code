public class Day2
{
    public static void Solve()
    {
        SolvePart1();
        SolvePart2();
    }

    private static string ReadInput()
    {
        return System.IO.File.ReadAllText("Day2_Input.txt");
    }

    private static bool isValidOne(long value)
    {
        string valueStr = value.ToString();
        int midpoint = valueStr.Length / 2;
        string first = valueStr.Substring(0, midpoint);
        string second = valueStr.Substring(midpoint);
        return first != second;
    }

    private static void SolvePart1()
    {

        string[] ranges = ReadInput().Split(",");
        long result = 0;
        foreach (string range in ranges)
        {
            string[] bounds = range.Split("-");
            long start = long.Parse(bounds[0]);
            long end = long.Parse(bounds[1]);
            for (long value = start; value <= end; value++)
            {
                if (!isValidOne(value))
                {
                    result += value;
                }

            }
        }
        Console.WriteLine($"Part 1 Answer: {result}");
    }

    private static bool isValidTwo(long value)
    {
        string valueStr = value.ToString();

        for (int size = 0; size < valueStr.Length; size++)
        {
            // Split the string into chunks of this length and 
            // see if they all match
            List<string> chunks = chunkify(valueStr, size + 1);
            if (chunks.Count() == 1)
                continue;

            bool allEqual = chunks.All(x => x == chunks[0]);
            if (allEqual)
            {
                return false;
            }
        }
        return true;
    }

    public static List<string> chunkify(string valueStr, int size)
    {
        List<string> chunks = [];
        string current = "";
        foreach (char c in valueStr)
        {
            current += c.ToString();
            if (current.Length == size)
            {
                chunks.Add(current);
                current = "";
            }
        }
        if (current.Length > 0)
            chunks.Add(current);
        return chunks;

    }

    private static void SolvePart2()
    {
        long result = 0;
        string[] ranges = ReadInput().Split(",");
        foreach (string range in ranges)
        {
            string[] bounds = range.Split("-");
            long start = long.Parse(bounds[0]);
            long end = long.Parse(bounds[1]);
            for (long value = start; value <= end; value++)
            {
                if (!isValidTwo(value))
                {
                    Console.WriteLine($"Invalid: {value}");
                    result += value;
                }

            }
        }
        Console.WriteLine($"Part 2 Answer: {result}");
    }
}
