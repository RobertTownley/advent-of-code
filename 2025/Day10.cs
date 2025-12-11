using System.Text.RegularExpressions;
using Button = bool[];

public class Day10
{
    public static void Solve()
    {
        SolvePart1();
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
        foreach (var line in lines.Take(1))
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

        int pressCount = 1;
        while (true)
        {
            var possiblePresses = getCombinations(availableButtons, pressCount);
            foreach (var sequence in possiblePresses)
            {
                var joltage = computeJoltageAfterPresses(sequence, joltages.Length);
                if (joltage.SequenceEqual(joltages))
                {
                    Console.WriteLine($"FOUND AT {pressCount}");
                    return pressCount;
                }
            }
            pressCount += 1;
        }
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
        Console.WriteLine($"Looking for combos of size {size}");
        if (size == 0)
        {
            yield return new List<Button>();
            yield break;
        }

        if (availableButtons.Length == 0)
        {
            yield break;
        }

        // For combinations with repetition, we generate all possible sequences
        // by choosing any button for each position using an iterative approach
        int n = availableButtons.Length;
        int totalCombinations = (int)Math.Pow(n, size);

        for (int i = 0; i < totalCombinations; i++)
        {
            var combination = new List<Button>();
            int index = i;

            // Convert the index to a base-n number to get the button sequence
            for (int pos = 0; pos < size; pos++)
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
        int counter = 1;
        foreach (var line in lines)
        {
            Console.WriteLine($"Line {counter}/{lines.Length}: {line}");
            var (_, availableButtons, joltages) = getOptions(line);
            result += exploreJoltages(joltages, availableButtons);
        }
        Console.WriteLine($"Part 2 Answer: {result}");
    }
}
