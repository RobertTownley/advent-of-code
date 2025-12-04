using Grid = System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, bool>>;


public class Day4
{
    public static void Solve()
    {
        SolvePart1();
        SolvePart2();
    }

    private static string ReadInput()
    {
        return System.IO.File.ReadAllText("Day4_Input.txt");
    }

    private static Grid FormGrid()
    {

        var grid = new Grid();
        string input = ReadInput();
        string[] lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        for (int y = 0; y < lines.Length; y++)
        {
            string line = lines[y];
            grid[y] = new Dictionary<int, bool>();
            for (int x = 0; x < line.Length; x++)
            {
                char value = line[x];
                grid[y][x] = value.ToString() == "@" ? true : false;

            }
        }
        return grid;
    }

    private static void SolvePart1()
    {
        int result = 0;
        var grid = FormGrid();

        for (int y = 0; y < grid.Count; y++)
            for (int x = 0; x < grid[y].Count; x++)
                if (canBeRemoved(y, x, grid))
                    result += 1;
        Console.WriteLine($"Part 1 Answer: {result}");
    }

    private static void SolvePart2()
    {
        int result = 0;
        var grid = FormGrid();

        while (true)
        {
            var toRemove = new List<int[]>();
            for (int y = 0; y < grid.Count; y++)
                for (int x = 0; x < grid[y].Count; x++)
                    if (canBeRemoved(y, x, grid))
                        toRemove.Add([y, x]);

            if (toRemove.Count() > 0)
                foreach (int[] pairing in toRemove)
                {
                    int y = pairing[0];
                    int x = pairing[1];
                    if (grid[y][x] == true)
                    {
                        result += 1;
                        grid[y][x] = false;

                    }
                }
            else
                break;
        }

        Console.WriteLine($"Part 2 Answer: {result}");
    }

    private static bool canBeRemoved(int y, int x, Grid grid)
    {
        bool hasRoll = grid[y][x];
        if (!hasRoll)
            return false;

        int rollCount = 0;

        for (int deltaY = -1; deltaY < 2; deltaY++)
        {
            int yCoord = y + deltaY;
            if (!grid.ContainsKey(yCoord))
                continue;
            for (int deltaX = -1; deltaX < 2; deltaX++)
            {
                int xCoord = x + deltaX;
                if (deltaX == 0 && deltaY == 0)
                    continue;

                if (!grid[yCoord].ContainsKey(xCoord))
                    continue;
                if (grid[yCoord][xCoord])
                    rollCount += 1;
            }
        }
        return rollCount < 4;
    }
}
