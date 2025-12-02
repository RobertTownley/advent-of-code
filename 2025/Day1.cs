public class Day1
{
    public static void Solve()
    {
        SolvePart1();
        SolvePart2();
    }

    private static string[] ReadInput()
    {
        return System.IO.File.ReadAllLines("Day1_Input.txt");
    }

    private static void SolvePart1()
    {
        int start = 50;
        int zeroCount = 0;
        foreach (string line in ReadInput())
        {
            char letter = line[0];
            int number = int.Parse(line[1..]);
            int end = letter == 'L' ? start - number : start + number;
            end = Modulo(end, 100);
            if (end == 0) zeroCount += 1;
            start = end;
        }
        Console.WriteLine($"Part 1: Answer is {zeroCount}");
    }

    private static void SolvePart2()
    {
        int start = 50;
        int zeroCount = 0;
        foreach (string line in ReadInput())
        {
            char letter = line[0];
            int number = int.Parse(line[1..]);
            int end = letter == 'L' ? start - number : start + number;

            if (end < 0)
            {
                zeroCount += IntDiv(end, -100);
                if (start != 0)
                    zeroCount += 1;
                end = Modulo(end, 100);
            }
            else if (end > 99)
            {
                zeroCount += IntDiv(end, 100);
                end = Modulo(end, 100);
            }
            else if (end == 0)
                zeroCount += 1;
            start = end;
        }
        Console.WriteLine($"Part 2: Answer is {zeroCount}");
    }

    public static int IntDiv(int dividend, int divisor)
    {
        int quotient = dividend / divisor;
        int remainder = dividend % divisor;

        if ((remainder != 0) && ((dividend < 0) != (divisor < 0)))
            quotient--;

        return quotient;
    }

    public static int Modulo(int a, int n)
    {
        int r = a % n;
        return r < 0 ? r + n : r;
    }
}
