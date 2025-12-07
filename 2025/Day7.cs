public class Day7
{
    public static void Solve()
    {
        SolvePart1();
        SolvePart2();
    }

    private static string[] ReadInput()
    {
        return System
            .IO.File.ReadAllText("Day7_Sample.txt")
            .Split("\n", StringSplitOptions.RemoveEmptyEntries);
    }

    private static void SolvePart1()
    {
        int result = 0;
        var beamIndexes = new HashSet<int>();
        foreach (string line in ReadInput())
        {
            if (line.IndexOf("S") != -1)
            {
                beamIndexes.Add(line.IndexOf("S"));
            }
            foreach (int beamIndex in new HashSet<int>(beamIndexes))
            {
                if (line[beamIndex] == '^')
                {
                    result += 1;
                    beamIndexes.Remove(beamIndex);
                    beamIndexes.Add(beamIndex + 1);
                    beamIndexes.Add(beamIndex - 1);
                }
            }
        }
        Console.WriteLine($"Part 1 Answer: {result}");
    }

    public static int Factorial(int n)
    {
        return Enumerable.Range(1, n).Aggregate(1, (acc, x) => acc * x);
    }

    private static void SolvePart2()
    {
        long result = 0;
        var lines = ReadInput();
        var beamProbs = new List<int>(Enumerable.Repeat(0, lines[0].Length));
        foreach (string line in lines.Take(5))
        {
            Console.WriteLine(line);
            if (line.IndexOf("S") != -1)
            {
                beamProbs[line.IndexOf("S")] += 1;
            }

            for (int i = 0; i < beamProbs.Count(); i++)
            {
                int beamProb = beamProbs[i];
                if (line[i] == '^')
                {
                    beamProbs[i] -= 1;
                    beamProbs[i - 1] += 1;
                    beamProbs[i + 1] += 1;
                }
            }
            Console.WriteLine(string.Join("", beamProbs));
        }
        result = beamProbs.Sum();
        Console.WriteLine($"Part 2 Answer: {result}");
    }
}
