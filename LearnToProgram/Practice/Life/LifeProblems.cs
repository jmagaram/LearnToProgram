using LearnToProgram.Practice.Writing;

namespace LearnToProgram.Practice.Life;

// The Game of Life pages in the Projects area. Students write the rule for one cell, and the board on the
// page calls their NewCell for every cell to make the next generation. The tests for each problem are in
// LifeSource, under the same Id. TestCount must match the number of tests there.
//
// Life 1 is a single row of cells (each cell has 2 neighbours). Each new generation is drawn under the one
// before, so the rule "alive if exactly one neighbour is alive" draws a Sierpinski triangle.
// Life 2 is Conway's Game of Life on a grid (each cell has 8 neighbours).
public static class LifeProblems
{
    const string TodoFalse = "// TODO\nreturn false;";
    const string Todo0 = "// TODO\nreturn 0;";

    // ---------- Life 1: one row ----------

    public static readonly WriteProblem[] Row =
    {
        new("1", "true if i is inside the array and cells[i] is alive. " +
                 "Outside the array (i is -1, or i is cells.Length) counts as dead.",
            "bool", "IsAlive", "bool[] cells, int i", TodoFalse, 8),
        new("2", "How many of the two neighbours of cell i (the cells at i - 1 and i + 1) are alive? " +
                 "Use IsAlive, so the cells at the ends work too.",
            "int", "CountNeighbors", "bool[] cells, int i", Todo0, 8),
        new("3", "The rule: will cell i be alive in the next generation? " +
                 "It is alive when exactly one of its two neighbours is alive.",
            "bool", "NewCell", "bool[] cells, int i", TodoFalse, 7),
    };

    public static readonly WriteCourse RowCourse =
        new("Life in a row", Row, LifeSource.Header, LifeSource.RowHarness, HintFor, TipsFor);

    // Other rules to try on the board. They use the student's IsAlive and CountNeighbors.
    public static readonly (string Name, string Code)[] RowRules =
    {
        ("Odd count (the cell counts too)", """
            int count = CountNeighbors(cells, i);
            if (IsAlive(cells, i))
            {
                count = count + 1;
            }
            return count % 2 == 1;
            """),
        ("Rule 30 (looks random)", """
            bool left = IsAlive(cells, i - 1);
            bool me = IsAlive(cells, i);
            bool right = IsAlive(cells, i + 1);
            if (left && !me && !right) return true;
            if (!left && (me || right)) return true;
            return false;
            """),
        ("Rule 110", """
            bool left = IsAlive(cells, i - 1);
            bool me = IsAlive(cells, i);
            bool right = IsAlive(cells, i + 1);
            if (left && me && right) return false;
            if (left && !me && !right) return false;
            if (!left && !me && !right) return false;
            return true;
            """),
        ("Grow (alive if any neighbour is alive)", """
            return IsAlive(cells, i) || CountNeighbors(cells, i) > 0;
            """),
    };

    // ---------- Life 2: the grid ----------

    const string IsAliveExample = """
        if (row < 0 || row >= grid.GetLength(0))
        {
            return false;
        }
        if (col < 0 || col >= grid.GetLength(1))
        {
            return false;
        }
        return grid[row, col];
        """;

    public static readonly WriteProblem[] Grid =
    {
        new("1", "true if (row, col) is inside the grid and that cell is alive. Outside the grid counts as dead. " +
                 "This one is already done, so you can see how it works.",
            "bool", "IsAlive", "bool[,] grid, int row, int col", IsAliveExample, 7, IsExample: true),
        new("2", "How many of the 8 cells around (row, col) are alive? Don't count the cell itself. " +
                 "Use IsAlive, so cells at the edge work too.",
            "int", "CountNeighbors", "bool[,] grid, int row, int col", Todo0, 8),
        new("3", "The rule: will cell (row, col) be alive in the next generation? " +
                 "A live cell stays alive if it has 2 or 3 live neighbours. " +
                 "A dead cell comes alive if it has exactly 3 live neighbours. Every other cell is dead.",
            "bool", "NewCell", "bool[,] grid, int row, int col", TodoFalse, 9),
    };

    public static readonly WriteCourse GridCourse =
        new("Game of Life", Grid, LifeSource.Header, LifeSource.GridHarness, HintFor, TipsFor);

    public static readonly (string Name, string Code)[] GridRules =
    {
        ("HighLife (born with 3 or 6)", """
            int n = CountNeighbors(grid, row, col);
            if (IsAlive(grid, row, col))
            {
                return n == 2 || n == 3;
            }
            return n == 3 || n == 6;
            """),
        ("Seeds (born with 2, nobody survives)", """
            if (IsAlive(grid, row, col))
            {
                return false;
            }
            return CountNeighbors(grid, row, col) == 2;
            """),
        ("Maze (survive with 1 to 5)", """
            int n = CountNeighbors(grid, row, col);
            if (IsAlive(grid, row, col))
            {
                return n >= 1 && n <= 5;
            }
            return n == 3;
            """),
        ("Day and Night", """
            int n = CountNeighbors(grid, row, col);
            if (IsAlive(grid, row, col))
            {
                return n == 3 || n == 4 || n == 6 || n == 7 || n == 8;
            }
            return n == 3 || n == 6 || n == 7 || n == 8;
            """),
    };

    // ---------- Tips ----------

    static string? HintFor(string id, string message) => id switch
    {
        "CS1955" when message.Contains("Length") =>
            "Length has no ( ). Write cells.Length, not cells.Length().",
        "CS0022" =>
            "The grid needs two indexes: grid[row, col]. The row of cells needs one: cells[i].",
        "CS0029" or "CS0266" when message.Contains("'int' to 'bool'") =>
            "This method returns true or false. Use a comparison, like n == 3.",
        "CS0029" or "CS0266" when message.Contains("'bool' to 'int'") =>
            "This method returns a number, not true or false.",
        "CS0019" when message.Contains("'bool' and 'int'") || message.Contains("'int' and 'bool'") =>
            "One side is true/false and the other is a number. Did you mean CountNeighbors(...) instead of IsAlive(...)?",
        "CS7036" =>
            "This method needs more values in the ( ). For example IsAlive(grid, row, col), not IsAlive(row, col).",
        "CS0103" => "C# cares about upper and lower case, and only knows names that exist in this method " +
                    "(like its parameters) or that you created with a variable.",
        _ => null,
    };

    static IEnumerable<string> TipsFor(WriteResult r)
    {
        var got = r.Rows.Where(x => !x.Ok).Select(x => x.Got).ToList();
        if (got.Any(g => g.Contains("IndexOutOfRangeException")))
            yield return "Your code looked outside the cells. Use IsAlive, which checks the edges for you.";
        if (got.Any(g => g.StartsWith("never ends: a loop")))
            yield return "A loop never stopped. Check that the loop variable changes each time, like i = i + 1.";
        if (got.Any(g => g.StartsWith("never ends: a method")))
            yield return "Your method keeps calling itself. Make sure it stops.";
        if (got.Any(g => g.StartsWith("changed ")))
            yield return "Your code changed the cells. It should only look at them and return an answer.";
        if (got.Any(g => g.Contains("NotImplementedException")))
            yield return "This calls one of your other methods that isn't done yet or doesn't compile.";
    }
}
