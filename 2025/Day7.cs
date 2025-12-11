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
            .IO.File.ReadAllText("Day7_Input.txt")
            .Split("\n", StringSplitOptions.RemoveEmptyEntries);
    }

    private static void SolvePart1()
    {
        int result = 0;
        var beamIndexes = new HashSet<int>();
        var lines = ReadInput();
        beamIndexes.Add(lines[0].IndexOf("S"));
        foreach (string line in ReadInput())
        {
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
        var lines = ReadInput();
        var beamProbs = new List<long>(Enumerable.Repeat(0L, lines[0].Length));
        beamProbs[lines[0].IndexOf("S")] = 1;

        foreach (string line in lines)
        {
            var newBeamProbs = new List<long>(Enumerable.Repeat(0L, beamProbs.Count));

            for (int i = 0; i < beamProbs.Count; i++)
            {
                long beamCount = beamProbs[i];
                if (beamCount > 0 && line[i] == '^')
                {
                    newBeamProbs[i - 1] += beamCount;
                    newBeamProbs[i + 1] += beamCount;
                }
                else
                    newBeamProbs[i] += beamCount;
            }

            beamProbs = newBeamProbs;
        }

        long result = beamProbs.Sum();
        Console.WriteLine($"Part 2 Answer: {result}");
    }
}
