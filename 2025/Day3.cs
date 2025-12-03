public class Day3
{
    public static void Solve()
    {
        SolvePart1();
        SolvePart2();
    }

    private static string[] ReadInput()
    {
        return System.IO.File
          .ReadAllText("Day3_Input.txt")
          .Split("\n", StringSplitOptions.RemoveEmptyEntries);
    }

    private static void SolvePart1()
    {
        int result = 0;
        foreach (string line in ReadInput())
        {
            char largestDigit = line.Substring(0, line.Length - 1).Max();
            string substr = line.Split(largestDigit, 2)[1];
            char secondLargest = substr.Max();
            result += int.Parse($"{largestDigit}{secondLargest}");
        }
        Console.WriteLine($"Part 1 Answer: {result}");
    }

    private static void SolvePart2()
    {
        int bankSize = 12;
        long result = 0;
        foreach (string line in ReadInput())
        {
            string jolt = "";
            string subline = line;
            while (jolt.Length < bankSize)
            {
                int eligibleEnd = subline.Length - (bankSize - jolt.Length - 1);
                string eligibleDigits = subline.Substring(0, eligibleEnd);
                char largest = eligibleDigits.Max();
                jolt += largest;
                subline = subline.Split(largest, 2)[1];
            }
            result += long.Parse($"{jolt}");
        }
        Console.WriteLine($"Part 2 Answer: {result}");
    }
}
