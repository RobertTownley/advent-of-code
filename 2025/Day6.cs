public class Day6
{
    public static void Solve()
    {
        SolvePart1();
        SolvePart2();
    }

    private static string ReadInput()
    {
        return System.IO.File.ReadAllText("Day6_Input.txt");
    }

    private static void SolvePart1()
    {
        long result = 0;
        int rowCounter = 0;
        var problems = new Dictionary<int, Dictionary<int, int>>();

        string[] lines = ReadInput().Split("\n", StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            int columnCounter = 0;
            foreach (string entry in line.Split(" ", StringSplitOptions.RemoveEmptyEntries))
            {
                if (entry == "*" || entry == "+")
                {
                    List<int> values = problems[columnCounter].Values.ToList();
                    result +=
                        entry == "*" ? values.Aggregate(1L, (acc, x) => acc * x) : values.Sum();
                }
                else
                {
                    if (!problems.ContainsKey(columnCounter))
                        problems[columnCounter] = new Dictionary<int, int>();
                    problems[columnCounter][rowCounter] = int.Parse(entry);
                }
                columnCounter += 1;
            }
            rowCounter += 1;
        }
        Console.WriteLine($"Part 1 Answer: {result}");
    }

    private static void SolvePart2()
    {
        long result = 0;
        string[] lines = ReadInput().Split("\n", StringSplitOptions.RemoveEmptyEntries);
        int columns = lines[0].Length;

        var currentValues = new List<int>();

        for (int x = columns - 1; x >= 0; x--)
        {
            string lineVal = "";
            foreach (string line in lines)
            {
                char entry = line[x];
                if (entry == '+' || entry == '*')
                {
                    currentValues.Add(int.Parse(lineVal.Replace(" ", "")));
                    result +=
                        entry == '+'
                            ? currentValues.Sum()
                            : currentValues.Aggregate(1L, (acc, x) => acc * x);
                    currentValues.Clear();
                }
                else if (entry != ' ')
                {
                    lineVal += entry;
                }
            }

            char lastChar = lines[lines.Length - 1][x];
            if (lineVal.Length > 0 && lastChar != '*' && lastChar != '+')
            {
                currentValues.Add(int.Parse(lineVal.Replace(" ", "")));
            }
        }
        Console.WriteLine($"Part 2 Answer: {result}");
    }
}
