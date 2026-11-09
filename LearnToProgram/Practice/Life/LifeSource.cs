namespace LearnToProgram.Practice.Life;

// C# source that is compiled in the browser together with the students' methods.
// It is plain text here, not part of this app. HarnessRunner adds the guard that stops endless loops.
// Each case in Run must have as many tests as the problem's TestCount in LifeProblems.
// In the tests a row of cells is shown like [.#.] and a grid like [#../.#./...]: # is alive, . is dead.
static class LifeSource
{
    public const string Header = """
        using System;
        using System.Collections.Generic;

        """;

    // Shared by both harnesses: showing cells, and Check, which runs one test.
    const string Common = """
            static List<string[]> rows;

            static bool[] R(string text)
            {
                bool[] cells = new bool[text.Length];
                for (int i = 0; i < text.Length; i++) cells[i] = text[i] == '#';
                return cells;
            }

            static bool[,] G(params string[] lines)
            {
                bool[,] grid = new bool[lines.Length, lines[0].Length];
                for (int r = 0; r < lines.Length; r++)
                    for (int c = 0; c < lines[0].Length; c++)
                        grid[r, c] = lines[r][c] == '#';
                return grid;
            }

            static string Text(object x)
            {
                if (x == null) return "null";
                if (x is bool) return (bool)x ? "true" : "false";
                if (x is bool[])
                {
                    string s = "";
                    foreach (bool b in (bool[])x) s += b ? "#" : ".";
                    return "[" + s + "]";
                }
                if (x is bool[,])
                {
                    bool[,] g = (bool[,])x;
                    List<string> lines = new List<string>();
                    for (int r = 0; r < g.GetLength(0); r++)
                    {
                        string line = "";
                        for (int c = 0; c < g.GetLength(1); c++) line += g[r, c] ? "#" : ".";
                        lines.Add(line);
                    }
                    return "[" + string.Join("/", lines) + "]";
                }
                return x.ToString();
            }

            // Each row: the call, what the student's code gave, the expected answer, "1" if they match.
            // The cells must be the same afterwards: the rule only looks at them.
            static void Check(string call, object cells, Func<object> run, object expected)
            {
                string before = Text(cells);
                string exp = Text(expected);
                string result;
                bool ok;
                __Guard.Reset();
                try
                {
                    result = Text(run());
                    if (Text(cells) != before) result = "changed the cells to " + Text(cells);
                    ok = result == exp;
                }
                catch (__LoopNeverEnds)
                {
                    ok = false;
                    result = "never ends: a loop went around " + __Guard.MaxLoops.ToString("N0") + " times";
                }
                catch (__TooManyCalls)
                {
                    ok = false;
                    result = "never ends: a method called itself " + __Guard.MaxCalls + " times";
                }
                catch (Exception e)
                {
                    ok = false;
                    result = "CRASHED: " + e.GetType().Name;
                }
                rows.Add(new[] { call, result, exp, ok ? "1" : "0" });
            }
        """;

    public const string RowHarness = """

        public static class __Harness
        {
            public static string[][] Run(string id)
            {
                rows = new List<string[]>();
                switch (id)
                {
                    case "1":
                        IsAlive(".#.", 1, true);
                        IsAlive(".#.", 0, false);
                        IsAlive(".#.", -1, false);
                        IsAlive(".#.", 3, false);
                        IsAlive("#..", 0, true);
                        IsAlive("..#", 2, true);
                        IsAlive("..#", 3, false);
                        IsAlive("#", -1, false);
                        break;
                    case "2":
                        Count(".#.", 0, 1);
                        Count("#.#", 1, 2);
                        Count("###", 1, 2);
                        Count(".#.", 1, 0);
                        Count("#..", 1, 1);
                        Count("##", 0, 1);
                        Count("#", 0, 0);
                        Count("..#", 2, 0);
                        break;
                    case "3":
                        NewCell("#.#", 1, false);
                        NewCell(".#.", 0, true);
                        NewCell("...", 1, false);
                        NewCell("#..", 1, true);
                        NewCell(".#.", 1, false);
                        NewCell("###", 0, true);
                        NewCell("#", 0, false);
                        break;
                }
                return rows.ToArray();
            }

            static void IsAlive(string text, int i, bool expected)
            {
                bool[] cells = R(text);
                Check("IsAlive(" + Text(cells) + ", " + i + ")", cells, () => Student.IsAlive(cells, i), expected);
            }

            static void Count(string text, int i, int expected)
            {
                bool[] cells = R(text);
                Check("CountNeighbors(" + Text(cells) + ", " + i + ")", cells, () => Student.CountNeighbors(cells, i), expected);
            }

            static void NewCell(string text, int i, bool expected)
            {
                bool[] cells = R(text);
                Check("NewCell(" + Text(cells) + ", " + i + ")", cells, () => Student.NewCell(cells, i), expected);
            }

        """ + Common + "\n}\n";

    public const string GridHarness = """

        public static class __Harness
        {
            public static string[][] Run(string id)
            {
                rows = new List<string[]>();
                string[] full = { "###", "###", "###" };
                string[] blinker = { ".#.", ".#.", ".#." };
                switch (id)
                {
                    case "1":
                        IsAlive(new[] { "#..", ".#.", "..." }, 0, 0, true);
                        IsAlive(new[] { "#..", ".#.", "..." }, 1, 1, true);
                        IsAlive(new[] { "#..", ".#.", "..." }, 0, 1, false);
                        IsAlive(new[] { "#..", ".#.", "..." }, -1, 0, false);
                        IsAlive(new[] { "#..", ".#.", "..." }, 0, 3, false);
                        IsAlive(new[] { "#..", ".#.", "..." }, 3, 0, false);
                        IsAlive(new[] { "#..", ".#.", "..." }, 2, -1, false);
                        break;
                    case "2":
                        Count(full, 1, 1, 8);
                        Count(full, 0, 0, 3);
                        Count(full, 0, 1, 5);
                        Count(new[] { "...", "...", "..." }, 1, 1, 0);
                        Count(new[] { "...", ".#.", "..." }, 1, 1, 0);
                        Count(blinker, 1, 0, 3);
                        Count(blinker, 1, 1, 2);
                        Count(blinker, 0, 0, 2);
                        break;
                    case "3":
                        NewCell(blinker, 1, 0, true);
                        NewCell(blinker, 1, 1, true);
                        NewCell(blinker, 0, 1, false);
                        NewCell(blinker, 0, 0, false);
                        NewCell(full, 1, 1, false);
                        NewCell(full, 0, 0, true);
                        NewCell(full, 0, 1, false);
                        NewCell(new[] { "##.", "#..", "..." }, 1, 1, true);
                        NewCell(new[] { "##.", "...", "..." }, 1, 0, false);
                        break;
                }
                return rows.ToArray();
            }

            static string Where(bool[,] grid, int row, int col)
            {
                return "(" + Text(grid) + ", " + row + ", " + col + ")";
            }

            static void IsAlive(string[] lines, int row, int col, bool expected)
            {
                bool[,] grid = G(lines);
                Check("IsAlive" + Where(grid, row, col), grid, () => Student.IsAlive(grid, row, col), expected);
            }

            static void Count(string[] lines, int row, int col, int expected)
            {
                bool[,] grid = G(lines);
                Check("CountNeighbors" + Where(grid, row, col), grid, () => Student.CountNeighbors(grid, row, col), expected);
            }

            static void NewCell(string[] lines, int row, int col, bool expected)
            {
                bool[,] grid = G(lines);
                Check("NewCell" + Where(grid, row, col), grid, () => Student.NewCell(grid, row, col), expected);
            }

        """ + Common + "\n}\n";
}
