namespace LearnToProgram.Practice.Arrays;

// C# source that is compiled in the browser together with the students' methods.
// It is plain text here, not part of this app. HarnessRunner adds the guard that stops endless loops.
// Each case in Run must have as many tests as the problem's TestCount in ArraysProblems.
static class ArraysSource
{
    public const string Header = """
        using System;
        using System.Collections.Generic;

        """;

    public const string Harness = """

        public static class __Harness
        {
            static List<string[]> rows;

            public static string[][] Run(string id)
            {
                rows = new List<string[]>();
                int[] empty = new int[0];
                switch (id)
                {
                    case "1":
                        TestInt("GetFirst", new[] { 4, 8, 15 }, 4, Student.GetFirst);
                        TestInt("GetFirst", new[] { 7 }, 7, Student.GetFirst);
                        TestInt("GetFirst", new[] { -3, 2 }, -3, Student.GetFirst);
                        TestInt("GetFirst", new[] { 0, 9, 9 }, 0, Student.GetFirst);
                        break;
                    case "2":
                        TestInt("GetLast", new[] { 4, 8, 15 }, 15, Student.GetLast);
                        TestInt("GetLast", new[] { 7 }, 7, Student.GetLast);
                        TestInt("GetLast", new[] { -3, 2 }, 2, Student.GetLast);
                        TestInt("GetLast", new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 10, Student.GetLast);
                        TestInt("GetLast", new[] { 5, 5, 6, 5 }, 5, Student.GetLast);
                        break;
                    case "3":
                        TestInt("SumOfEnds", new[] { 1, 2, 3 }, 4, Student.SumOfEnds);
                        TestInt("SumOfEnds", new[] { 7 }, 14, Student.SumOfEnds);
                        TestInt("SumOfEnds", new[] { -5, 0, 5 }, 0, Student.SumOfEnds);
                        TestInt("SumOfEnds", new[] { 10, 20 }, 30, Student.SumOfEnds);
                        TestInt("SumOfEnds", new[] { -2, 8, 8, -3 }, -5, Student.SumOfEnds);
                        break;
                    case "4":
                        TestChange("SwapFirstAndLast", new[] { 1, 2, 3 }, new[] { 3, 2, 1 }, Student.SwapFirstAndLast);
                        TestChange("SwapFirstAndLast", new[] { 1, 2, 3, 4 }, new[] { 4, 2, 3, 1 }, Student.SwapFirstAndLast);
                        TestChange("SwapFirstAndLast", new[] { 7 }, new[] { 7 }, Student.SwapFirstAndLast);
                        TestChange("SwapFirstAndLast", new[] { 5, 9 }, new[] { 9, 5 }, Student.SwapFirstAndLast);
                        TestChange("SwapFirstAndLast", new[] { 6, 0, 6 }, new[] { 6, 0, 6 }, Student.SwapFirstAndLast);
                        break;
                    case "5":
                        foreach (int[] t in SortTests())
                            TestInto("Sort3", "sorted", t, Sorted(t), Student.Sort3);
                        break;
                    case "6":
                        foreach (int[] t in SortTests())
                            TestChange("Sort3InPlace", t, Sorted(t), Student.Sort3InPlace);
                        break;
                    case "7":
                        TestGrid(new int[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } }, 5);
                        TestGrid(new int[,] { { 0, 0, 0 }, { 0, -4, 0 }, { 0, 0, 0 } }, -4);
                        TestGrid(new int[,] { { 9, 8, 7 }, { 6, 3, 4 }, { 5, 2, 1 } }, 3);
                        break;
                    case "8":
                        TestBoard("TopRowWins", B("XXX", "O.O", "..."), "X", true, Student.TopRowWins);
                        TestBoard("TopRowWins", B("XXX", "O.O", "..."), "O", false, Student.TopRowWins);
                        TestBoard("TopRowWins", B("OOO", "XX.", "X.."), "O", true, Student.TopRowWins);
                        TestBoard("TopRowWins", B("XXO", "...", "..."), "X", false, Student.TopRowWins);
                        TestBoard("TopRowWins", B("...", "XXX", "OO."), "X", false, Student.TopRowWins);
                        TestBoard("TopRowWins", B("X..", "X..", "X.."), "X", false, Student.TopRowWins);
                        break;
                    case "9":
                        TestBoard("DiagonalWins", B("X..", ".X.", "..X"), "X", true, Student.DiagonalWins);
                        TestBoard("DiagonalWins", B("..O", ".O.", "O.."), "O", true, Student.DiagonalWins);
                        TestBoard("DiagonalWins", B("..O", ".O.", "O.."), "X", false, Student.DiagonalWins);
                        TestBoard("DiagonalWins", B("XXX", "...", "..."), "X", false, Student.DiagonalWins);
                        TestBoard("DiagonalWins", B("X..", ".O.", "..X"), "X", false, Student.DiagonalWins);
                        TestBoard("DiagonalWins", B("O.X", ".X.", "X.O"), "X", true, Student.DiagonalWins);
                        TestBoard("DiagonalWins", B("...", "...", "..."), "X", false, Student.DiagonalWins);
                        break;
                    case "10":
                        TestInt("Sum", new[] { 1, 2, 3 }, 6, Student.Sum);
                        TestInt("Sum", new[] { 7 }, 7, Student.Sum);
                        TestInt("Sum", empty, 0, Student.Sum);
                        TestInt("Sum", new[] { -4, 4, -1 }, -1, Student.Sum);
                        TestInt("Sum", new[] { 10, 20, 30, 40 }, 100, Student.Sum);
                        break;
                    case "11":
                        TestCountAbove(new[] { 1, 5, 8, 3 }, 4, 2);
                        TestCountAbove(new[] { 1, 5, 8, 3 }, 8, 0);
                        TestCountAbove(new[] { 4, 4, 4 }, 3, 3);
                        TestCountAbove(empty, 0, 0);
                        TestCountAbove(new[] { -2, -1, 0 }, -2, 2);
                        TestCountAbove(new[] { 5 }, 5, 0);
                        break;
                    case "12":
                        TestInt("Max", new[] { 3, 9, 2 }, 9, Student.Max);
                        TestInt("Max", new[] { 7 }, 7, Student.Max);
                        TestInt("Max", new[] { -5, -1, -9 }, -1, Student.Max);
                        TestInt("Max", new[] { 1, 2, 3, 4 }, 4, Student.Max);
                        TestInt("Max", new[] { 8, 1, 8 }, 8, Student.Max);
                        TestInt("Max", new[] { 4, 3, 2, 1 }, 4, Student.Max);
                        break;
                    case "13":
                        TestBool("IsIncreasing", new[] { 1, 2, 3 }, true, Student.IsIncreasing);
                        TestBool("IsIncreasing", new[] { -3, 0, 4, 10 }, true, Student.IsIncreasing);
                        TestBool("IsIncreasing", new[] { 1, 3, 2 }, false, Student.IsIncreasing);
                        TestBool("IsIncreasing", new[] { 2, 1 }, false, Student.IsIncreasing);
                        TestBool("IsIncreasing", new[] { 1, 2, 2 }, false, Student.IsIncreasing);
                        TestBool("IsIncreasing", new[] { 5 }, true, Student.IsIncreasing);
                        TestBool("IsIncreasing", empty, true, Student.IsIncreasing);
                        TestBool("IsIncreasing", new[] { 1, 2, 3, 0 }, false, Student.IsIncreasing);
                        break;
                    case "14":
                        foreach (int[] t in ReverseTests())
                            TestInto("ReverseInto", "result", t, Reversed(t), Student.ReverseInto);
                        break;
                    case "15":
                        foreach (int[] t in ReverseTests())
                            TestChange("ReverseInPlace", t, Reversed(t), Student.ReverseInPlace);
                        break;
                    case "16":
                        TestWords(new[] { "up", "down", "up" }, true);
                        TestWords(new[] { "a", "b" }, false);
                        TestWords(new[] { "x" }, true);
                        TestWords(new string[0], true);
                        TestWords(new[] { "red", "blue", "blue", "red" }, true);
                        TestWords(new[] { "red", "blue", "green", "red" }, false);
                        TestWords(new[] { "one", "two", "three" }, false);
                        break;
                    case "17":
                        TestBool("IsFibonacciLike", new[] { 1, 1, 2, 3, 5, 8 }, true, Student.IsFibonacciLike);
                        TestBool("IsFibonacciLike", new[] { 2, 3, 5, 8 }, true, Student.IsFibonacciLike);
                        TestBool("IsFibonacciLike", new[] { 1, 2, 4 }, false, Student.IsFibonacciLike);
                        TestBool("IsFibonacciLike", new[] { 5, 7 }, true, Student.IsFibonacciLike);
                        TestBool("IsFibonacciLike", empty, true, Student.IsFibonacciLike);
                        TestBool("IsFibonacciLike", new[] { 0, 0, 0 }, true, Student.IsFibonacciLike);
                        TestBool("IsFibonacciLike", new[] { 1, 1, 2, 3, 5, 9 }, false, Student.IsFibonacciLike);
                        TestBool("IsFibonacciLike", new[] { 3, -1, 2, 1 }, true, Student.IsFibonacciLike);
                        break;
                    case "18":
                        TestBoard("Wins", B("XXX", "OO.", "..."), "X", true, Student.Wins);
                        TestBoard("Wins", B("X.X", "OOO", "X.."), "O", true, Student.Wins);
                        TestBoard("Wins", B("O.O", ".O.", "XXX"), "X", true, Student.Wins);
                        TestBoard("Wins", B("XO.", "XO.", "X.."), "X", true, Student.Wins);
                        TestBoard("Wins", B("XO.", ".OX", "XO."), "O", true, Student.Wins);
                        TestBoard("Wins", B("O.X", "O.X", "..X"), "X", true, Student.Wins);
                        TestBoard("Wins", B("O.X", ".O.", "X.O"), "O", true, Student.Wins);
                        TestBoard("Wins", B("O.X", ".X.", "X.O"), "X", true, Student.Wins);
                        TestBoard("Wins", B("XOX", "XOO", "OXX"), "X", false, Student.Wins);
                        TestBoard("Wins", B("XOX", "XOO", "OXX"), "O", false, Student.Wins);
                        break;
                }
                return rows.ToArray();
            }

            static int[][] SortTests()
            {
                return new int[][]
                {
                    new[] { 1, 2, 3 }, new[] { 3, 2, 1 }, new[] { 2, 3, 1 }, new[] { 1, 3, 2 },
                    new[] { 3, 1, 2 }, new[] { 2, 1, 3 }, new[] { 5, 5, 1 }, new[] { -1, -7, 0 },
                };
            }

            static int[][] ReverseTests()
            {
                return new int[][] { new[] { 1, 2, 3 }, new[] { 1, 2 }, new[] { 7 }, new int[0], new[] { 4, 1, 4, 2 } };
            }

            static int[] Sorted(int[] a)
            {
                int[] copy = (int[])a.Clone();
                Array.Sort(copy);
                return copy;
            }

            static int[] Reversed(int[] a)
            {
                int[] copy = (int[])a.Clone();
                Array.Reverse(copy);
                return copy;
            }

            // A tic-tac-toe board from three rows, like "XO.", where . is an empty square ("").
            static string[,] B(string row0, string row1, string row2)
            {
                string[] rowText = { row0, row1, row2 };
                string[,] board = new string[3, 3];
                for (int r = 0; r < 3; r++)
                    for (int c = 0; c < 3; c++)
                        board[r, c] = rowText[r][c] == '.' ? "" : rowText[r][c].ToString();
                return board;
            }

            static string Text(object x)
            {
                if (x == null) return "null";
                if (x is bool) return (bool)x ? "true" : "false";
                if (x is int[]) return "{" + string.Join(", ", (int[])x) + "}";
                if (x is string[])
                {
                    List<string> parts = new List<string>();
                    foreach (string s in (string[])x) parts.Add(s == null ? "null" : "\"" + s + "\"");
                    return "{" + string.Join(", ", parts) + "}";
                }
                if (x is string[,])
                {
                    string[,] b = (string[,])x;
                    List<string> lines = new List<string>();
                    for (int r = 0; r < b.GetLength(0); r++)
                    {
                        string line = "";
                        for (int c = 0; c < b.GetLength(1); c++)
                            line += string.IsNullOrEmpty(b[r, c]) ? "." : b[r, c];
                        lines.Add(line);
                    }
                    return string.Join("/", lines);
                }
                if (x is int[,])
                {
                    int[,] g = (int[,])x;
                    List<string> lines = new List<string>();
                    for (int r = 0; r < g.GetLength(0); r++)
                    {
                        List<string> cells = new List<string>();
                        for (int c = 0; c < g.GetLength(1); c++) cells.Add(g[r, c].ToString());
                        lines.Add("{" + string.Join(", ", cells) + "}");
                    }
                    return "{" + string.Join(", ", lines) + "}";
                }
                return x.ToString();
            }

            // Each row: the call, what the student's code gave, the expected answer, "1" if they match.
            static void Check(string call, Func<object> run, object expected)
            {
                string exp = Text(expected);
                string result;
                bool ok;
                __Guard.Reset();
                try
                {
                    result = Text(run());
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

            static void TestInt(string name, int[] a, int expected, Func<int[], int> f)
            {
                Check(name + "(" + Text(a) + ")", () => f(a), expected);
            }

            static void TestBool(string name, int[] a, bool expected, Func<int[], bool> f)
            {
                Check(name + "(" + Text(a) + ")", () => f(a), expected);
            }

            // A void method that changes the array it gets. The result is the array afterwards.
            static void TestChange(string name, int[] a, int[] expected, Action<int[]> f)
            {
                Check(name + "(" + Text(a) + ")", () =>
                {
                    f(a);
                    return a;
                }, expected);
            }

            // A void method that fills a second array of the same length, without changing the first.
            static void TestInto(string name, string second, int[] a, int[] expected, Action<int[], int[]> f)
            {
                Check(name + "(" + Text(a) + ", " + second + ")", () =>
                {
                    int[] original = (int[])a.Clone();
                    int[] result = new int[a.Length];
                    f(a, result);
                    if (Text(a) != Text(original)) return "changed the first array to " + Text(a);
                    return result;
                }, expected);
            }

            static void TestGrid(int[,] grid, int expected)
            {
                Check("GetCenter(" + Text(grid) + ")", () => Student.GetCenter(grid), expected);
            }

            static void TestBoard(string name, string[,] board, string player, bool expected, Func<string[,], string, bool> f)
            {
                Check(name + "(" + Text(board) + ", \"" + player + "\")", () => f(board, player), expected);
            }

            static void TestCountAbove(int[] a, int limit, int expected)
            {
                Check("CountAbove(" + Text(a) + ", " + limit + ")", () => Student.CountAbove(a, limit), expected);
            }

            static void TestWords(string[] words, bool expected)
            {
                Check("IsPalindrome(" + Text(words) + ")", () => Student.IsPalindrome(words), expected);
            }
        }
        """;
}
