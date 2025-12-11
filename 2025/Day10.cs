// Disclaimer: I had to ask for non-trivial LLM help on part 2.
//
// Also for this one I'm not even sure I got there
// I didn't submit my part 2 answer to AoC though

using System.Text.RegularExpressions;
using Button = bool[];

public class Day10
{
    public static void Solve()
    {
        // SolvePart1();
        SolvePart2();
    }

    private static string[] ReadInput()
    {
        return System
            .IO.File.ReadAllText("Day10_Input.txt")
            .Split("\n", StringSplitOptions.RemoveEmptyEntries);
    }

    private static void SolvePart1()
    {
        int result = 0;
        var lines = ReadInput();
        foreach (var line in lines)
        {
            var (diagram, availableButtons, _) = getOptions(line);
            result += exploreButtonPresses(diagram, availableButtons);
        }

        Console.WriteLine($"Part 1 Answer: {result}");
    }

    private static (Button, bool[][], int[]) getOptions(string line)
    {
        string diagramStr = line.Split(" ", 2)[0];
        bool[] diagram = diagramStr
            .Substring(1, diagramStr.Length - 2)
            .ToCharArray()
            .Select(c => c == '#' ? true : false)
            .ToArray();

        Button[] availableButtons = line.Replace(diagramStr, "")
            .Split("{", 2)[0]
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(x => Regex.Replace(x, @"[()]", ""))
            .Select(x => x.Split(",").Select(int.Parse).ToArray())
            .Select(x =>
                Enumerable.Range(0, diagram.Length).Select((_, i) => x.Contains(i)).ToArray()
            )
            .ToArray();

        string joltageStr = line.Split("{", 2)[1];
        int[] joltages = joltageStr
            .Substring(0, joltageStr.Length - 1)
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        return (diagram, availableButtons, joltages);
    }

    private static int exploreButtonPresses(Button diagram, Button[] availableButtons)
    {
        if (diagram.All(x => x == false))
            return 0;

        int pressCount = 1;
        while (true)
        {
            var possiblePresses = getCombinations(availableButtons, pressCount);
            foreach (var possiblePress in possiblePresses)
            {
                var sequence = computeSequence(possiblePress, diagram.Length);
                if (sequence.SequenceEqual(diagram))
                    return pressCount;
            }
            pressCount += 1;
        }
    }

    private static int exploreJoltages(int[] joltages, Button[] availableButtons)
    {
        if (joltages.All(j => j == 0))
            return 0;

        int[] currentState = new int[joltages.Length];
        int totalPresses = 0;

        while (!currentState.SequenceEqual(joltages))
        {
            int bestButton = -1;
            double bestEfficiency = 0;

            for (int i = 0; i < availableButtons.Length; i++)
            {
                int positionsHelped = 0;
                int minRemaining = int.MaxValue;
                bool canPress = true;

                for (int j = 0; j < joltages.Length; j++)
                {
                    if (availableButtons[i][j])
                    {
                        int remaining = joltages[j] - currentState[j];
                        if (remaining <= 0)
                        {
                            canPress = false;
                            break;
                        }
                        positionsHelped++;
                        minRemaining = Math.Min(minRemaining, remaining);
                    }
                }

                if (canPress && positionsHelped > 0)
                {
                    double efficiency = positionsHelped * Math.Min(minRemaining, 10);

                    if (efficiency > bestEfficiency)
                    {
                        bestEfficiency = efficiency;
                        bestButton = i;
                    }
                }
            }

            if (bestButton == -1)
                return -1;

            int timesToPress = int.MaxValue;
            for (int j = 0; j < joltages.Length; j++)
                if (availableButtons[bestButton][j])
                {
                    int remaining = joltages[j] - currentState[j];
                    timesToPress = Math.Min(timesToPress, remaining);
                }

            for (int j = 0; j < joltages.Length; j++)
                if (availableButtons[bestButton][j])
                    currentState[j] += timesToPress;
            totalPresses += timesToPress;
        }

        return totalPresses;
    }

    public static int[] computeJoltageAfterPresses(List<Button> sequence, int size)
    {
        int[] start = new int[size];
        foreach (var button in sequence)
        {
            for (int i = 0; i < size; i++)
            {
                if (button[i])
                {
                    start[i] += 1;
                }
            }
        }
        return start;
    }

    public static Button computeSequence(List<Button> sequence, int size)
    {
        Button start = new bool[size];
        foreach (var button in sequence)
        {
            for (int i = 0; i < size; i++)
            {
                start[i] ^= button[i];
            }
        }
        return start;
    }

    public static IEnumerable<List<Button>> getCombinations(Button[] availableButtons, int size)
    {
        if (size == 0)
        {
            yield return new List<Button>();
            yield break;
        }

        if (availableButtons.Length == 0)
            yield break;

        int n = availableButtons.Length;
        int totalCombinations = (int)Math.Pow(n, size);

        for (int i = 0; i < totalCombinations; i++)
        {
            var combination = new List<Button>();
            int index = i;

            for (int j = 0; j < size; j++)
            {
                int buttonIndex = index % n;
                combination.Add(availableButtons[buttonIndex]);
                index /= n;
            }

            yield return combination;
        }
    }

    private static void SolvePart2()
    {
        int result = 0;
        var lines = ReadInput();
        foreach (var line in lines)
        {
            var (_, availableButtons, joltages) = getOptions(line);
            result += exploreJoltages(joltages, availableButtons);
        }
        Console.WriteLine($"Part 2 Answer: {result}");
    }
}
