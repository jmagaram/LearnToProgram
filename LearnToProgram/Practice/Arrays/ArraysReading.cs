namespace LearnToProgram.Practice.Arrays;

// Questions for the Arrays 1 page: students read a short piece of code and choose or type what it
// prints or what the array looks like afterwards. No code is written. See Practice/Reading.cs.
//
// In Run, p(...) stands for Console.WriteLine(...). Show(a) prints an array like {4, 8, 15} or
// {"red", "blue"}, and Rows(grid) prints a 2D array one row per line, like "1 2 3".
// Steps are written as i = i + 1, not i++ or i += 1, since students may not know the short forms.

public static class ArraysReading
{
    public const string Crash = "Error: the program crashes";

    public static readonly ReadLevel[] Levels =
    {
        new(1, "Index and Length",
            "<code>a[0]</code> is the first item, <code>a[1]</code> the second, and so on. " +
            "<code>a.Length</code> is how many items there are, so the last item is <code>a[a.Length - 1]</code>. " +
            "Using an index outside the array crashes the program.",
            "המקום הראשון במערך הוא 0, והאחרון הוא a.Length - 1. אינדקס מחוץ למערך גורם לקריסה."),
        new(2, "Loops that read",
            "<code>for (int i = 0; i &lt; a.Length; i = i + 1)</code> visits every index from first to last. " +
            "Check the start, the condition and the step of every loop.",
            "בדקו את ההתחלה, התנאי והקפיצה של כל לולאה."),
        new(3, "Loops that change the array",
            "When a loop changes <code>a[i]</code>, the next steps see the <b>new</b> value, not the old one. " +
            "Type arrays like <code>{1, 2, 3}</code>.",
            "כשהלולאה משנה את a[i], הצעדים הבאים רואים את הערך החדש."),
        new(4, "Arrays of words",
            "An array can hold strings, like <code>{\"red\", \"blue\"}</code>. " +
            "Compare strings with <code>==</code>. Remember that earlier steps may have already changed an item.",
            "מערך יכול להכיל מחרוזות. משווים מחרוזות עם ==."),
        new(5, "Follow the pointer",
            "Here each number is an <b>index</b>: it tells you where to go next. " +
            "Write down where you are after each step.",
            "כל מספר במערך אומר לאיזה מקום ללכת. רשמו איפה אתם אחרי כל צעד."),
        new(6, "2D arrays (grids)",
            "<code>grid[row, col]</code>: the row first, then the column, both starting at 0. " +
            "<code>grid.GetLength(0)</code> is the number of rows, <code>grid.GetLength(1)</code> the number of columns, " +
            "and <code>grid.Length</code> the number of cells in the whole grid.",
            "קודם השורה ואחר כך העמודה. GetLength(0) הוא מספר השורות, ו־GetLength(1) מספר העמודות."),
    };

    static string[] L(params string[] lines) => lines;
    static string[] L(params int[] lines) => lines.Select(n => n.ToString()).ToArray();

    public static string Show(int[] a) => "{" + string.Join(", ", a) + "}";
    public static string Show(string[] a) => "{" + string.Join(", ", a.Select(s => "\"" + s + "\"")) + "}";

    static string[] A(params int[] values) => L(Show(values));
    static string[] W(params string[] words) => L(Show(words));

    public static string[] Rows(int[,] grid) =>
        Enumerable.Range(0, grid.GetLength(0))
            .Select(r => string.Join(" ", Enumerable.Range(0, grid.GetLength(1)).Select(c => grid[r, c])))
            .ToArray();

    static readonly int[,] Grid123 = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
    static readonly int[,] Grid147 = { { 1, 4, 7 }, { 2, 5, 8 }, { 3, 6, 9 } };
    static readonly int[,] Grid987 = { { 9, 8, 7 }, { 6, 5, 4 }, { 3, 2, 1 } };
    static readonly int[,] Grid111 = { { 1, 2, 3 }, { 2, 5, 6 }, { 3, 6, 9 } };

    // Runs the code and collects what it prints. An index outside the array counts as a crash.
    static Func<string[]> Prints(Action<Action<object>> body) => () =>
    {
        var lines = new List<string>();
        try { body(x => lines.Add(x.ToString()!)); }
        catch (IndexOutOfRangeException) { lines.Add(Crash); }
        return lines.ToArray();
    };

    const string WhatPrints = "What does this code print?";
    const string WhatPrintsHe = "מה הקוד ידפיס?";
    const string WhatArray = "What does a look like after this code?";
    const string WhatArrayHe = "איך ייראה המערך a אחרי הקוד?";
    const string TypeHint = "Type the array, like {1, 2, 3}.";
    const string WhatGrid = "What does grid look like after this code?";
    const string WhatGridHe = "איך ייראה grid אחרי הקוד?";

    static readonly string[][] None = Array.Empty<string[]>();

    public static readonly ReadQuestion[] All =
    {
        // ---------- Level 1: index and Length ----------
        new(1, 1, ReadKind.Choose, WhatPrints, WhatPrintsHe,
            """
            int[] nums = {4, 8, 15, 16, 23};
            Console.WriteLine(nums[2]);
            """,
            new[] { L(8), L(15), L(16), L(2) },
            Prints(p =>
            {
                int[] nums = { 4, 8, 15, 16, 23 };
                p(nums[2]);
            })),

        new(2, 1, ReadKind.TypeNumber, WhatPrints, WhatPrintsHe,
            """
            int[] nums = {4, 8, 15, 16, 23};
            Console.WriteLine(nums.Length);
            """,
            None,
            Prints(p =>
            {
                int[] nums = { 4, 8, 15, 16, 23 };
                p(nums.Length);
            })),

        new(3, 1, ReadKind.Choose, WhatPrints, WhatPrintsHe,
            """
            int[] nums = {4, 8, 15, 16, 23};
            Console.WriteLine(nums[5]);
            """,
            new[] { L(23), L(0), L(Crash) },
            Prints(p =>
            {
                int[] nums = { 4, 8, 15, 16, 23 };
                p(nums[Five()]);
            })),

        new(4, 1, ReadKind.Choose, WhatPrints, WhatPrintsHe,
            """
            int[] nums = {4, 8, 15, 16, 23};
            Console.WriteLine(nums[nums.Length - 1]);
            """,
            new[] { L(23), L(16), L(4), L(Crash) },
            Prints(p =>
            {
                int[] nums = { 4, 8, 15, 16, 23 };
                p(nums[nums.Length - 1]);
            })),

        new(5, 1, ReadKind.Choose, WhatPrints, WhatPrintsHe,
            """
            string[] colors = {"red", "blue", "green"};
            colors[1] = "yellow";
            Console.WriteLine(colors[0] + " " + colors[1]);
            """,
            new[] { L("red yellow"), L("yellow blue"), L("red blue"), L("blue yellow") },
            Prints(p =>
            {
                string[] colors = { "red", "blue", "green" };
                colors[1] = "yellow";
                p(colors[0] + " " + colors[1]);
            })),

        new(6, 1, ReadKind.TypeNumber, WhatPrints, WhatPrintsHe,
            """
            int[] a = {3, 7, 2};
            a[0] = a[1] + a[2];
            a[2] = a[0] - 1;
            Console.WriteLine(a[2]);
            """,
            None,
            Prints(p =>
            {
                int[] a = { 3, 7, 2 };
                a[0] = a[1] + a[2];
                a[2] = a[0] - 1;
                p(a[2]);
            })),

        // ---------- Level 2: loops that read ----------
        new(7, 2, ReadKind.TypeNumber, WhatPrints, WhatPrintsHe,
            """
            int[] a = {6, 2, 9, 4, 5};
            int count = 0;
            for (int i = 0; i < a.Length; i = i + 1)
            {
                if (a[i] > 4)
                {
                    count = count + 1;
                }
            }
            Console.WriteLine(count);
            """,
            None,
            Prints(p =>
            {
                int[] a = { 6, 2, 9, 4, 5 };
                int count = 0;
                for (int i = 0; i < a.Length; i = i + 1)
                {
                    if (a[i] > 4) count = count + 1;
                }
                p(count);
            })),

        new(8, 2, ReadKind.Choose, WhatPrints, WhatPrintsHe,
            """
            int[] a = {10, 20, 30, 40, 50};
            for (int i = 0; i < a.Length; i = i + 2)
            {
                Console.WriteLine(a[i]);
            }
            """,
            new[] { L(10, 30, 50), L(20, 40), L(10, 20, 30, 40, 50), L(10, 30) },
            Prints(p =>
            {
                int[] a = { 10, 20, 30, 40, 50 };
                for (int i = 0; i < a.Length; i = i + 2) p(a[i]);
            })),

        new(9, 2, ReadKind.Choose, WhatPrints, WhatPrintsHe,
            """
            int[] a = {1, 2, 3};
            for (int i = 0; i <= a.Length; i = i + 1)
            {
                Console.WriteLine(a[i]);
            }
            """,
            new[] { L("1", "2", "3"), L("1", "2", "3", Crash), L("1", "2"), L(Crash) },
            Prints(p =>
            {
                int[] a = { 1, 2, 3 };
                for (int i = 0; i <= a.Length; i = i + 1) p(a[i]);
            })),

        new(10, 2, ReadKind.Choose, WhatPrints, WhatPrintsHe,
            """
            int[] a = {5, 9, 2, 7};
            for (int i = a.Length - 1; i >= 0; i = i - 1)
            {
                Console.WriteLine(a[i]);
            }
            """,
            new[] { L(7, 2, 9, 5), L(5, 9, 2, 7), L(7, 2, 9), L(2, 9, 5) },
            Prints(p =>
            {
                int[] a = { 5, 9, 2, 7 };
                for (int i = a.Length - 1; i >= 0; i = i - 1) p(a[i]);
            })),

        new(11, 2, ReadKind.TypeNumber, WhatPrints, WhatPrintsHe,
            """
            int[] a = {6, -2, 11, 4, 11};
            int best = 0;
            for (int i = 1; i < a.Length; i = i + 1)
            {
                if (a[i] > a[best])
                {
                    best = i;
                }
            }
            Console.WriteLine(best);
            """,
            None,
            Prints(p =>
            {
                int[] a = { 6, -2, 11, 4, 11 };
                int best = 0;
                for (int i = 1; i < a.Length; i = i + 1)
                {
                    if (a[i] > a[best]) best = i;
                }
                p(best);
            })),

        // ---------- Level 3: loops that change the array ----------
        new(12, 3, ReadKind.TypeList, WhatArray + " " + TypeHint, WhatArrayHe,
            """
            int[] a = {1, 2, 3, 4};
            for (int i = 0; i < a.Length - 1; i = i + 1)
            {
                a[i] = a[i] + a[i + 1];
            }
            """,
            None,
            Prints(p =>
            {
                int[] a = { 1, 2, 3, 4 };
                for (int i = 0; i < a.Length - 1; i = i + 1) a[i] = a[i] + a[i + 1];
                p(Show(a));
            })),

        new(13, 3, ReadKind.Choose, WhatArray, WhatArrayHe,
            """
            int[] a = {1, 2, 3, 4};
            for (int i = 1; i < a.Length; i = i + 1)
            {
                a[i] = a[i] + a[i - 1];
            }
            """,
            new[] { A(1, 3, 5, 7), A(1, 3, 6, 10), A(3, 5, 7, 4), A(2, 3, 4, 5) },
            Prints(p =>
            {
                int[] a = { 1, 2, 3, 4 };
                for (int i = 1; i < a.Length; i = i + 1) a[i] = a[i] + a[i - 1];
                p(Show(a));
            })),

        new(14, 3, ReadKind.Choose, WhatArray, WhatArrayHe,
            """
            int[] a = {1, 2, 3, 4, 5};
            for (int i = 0; i < a.Length - 1; i = i + 1)
            {
                int temp = a[i];
                a[i] = a[i + 1];
                a[i + 1] = temp;
            }
            """,
            new[] { A(2, 1, 4, 3, 5), A(2, 3, 4, 5, 1), A(5, 4, 3, 2, 1), A(1, 2, 3, 4, 5) },
            Prints(p =>
            {
                int[] a = { 1, 2, 3, 4, 5 };
                for (int i = 0; i < a.Length - 1; i = i + 1)
                {
                    int temp = a[i];
                    a[i] = a[i + 1];
                    a[i + 1] = temp;
                }
                p(Show(a));
            })),

        new(15, 3, ReadKind.Choose, "The same code, but now the step is i = i + 2. " + WhatArray, WhatArrayHe,
            """
            int[] a = {1, 2, 3, 4, 5};
            for (int i = 0; i < a.Length - 1; i = i + 2)
            {
                int temp = a[i];
                a[i] = a[i + 1];
                a[i + 1] = temp;
            }
            """,
            new[] { A(2, 1, 4, 3, 5), A(2, 3, 4, 5, 1), A(5, 4, 3, 2, 1), A(2, 1, 4, 3, 1) },
            Prints(p =>
            {
                int[] a = { 1, 2, 3, 4, 5 };
                for (int i = 0; i < a.Length - 1; i = i + 2)
                {
                    int temp = a[i];
                    a[i] = a[i + 1];
                    a[i + 1] = temp;
                }
                p(Show(a));
            })),

        new(16, 3, ReadKind.Choose, "This loop tries to move every item one place to the right. " + WhatArray, WhatArrayHe,
            """
            int[] a = {7, 8, 9, 10};
            for (int i = 1; i < a.Length; i = i + 1)
            {
                a[i] = a[i - 1];
            }
            """,
            new[] { A(7, 7, 8, 9), A(7, 7, 7, 7), A(10, 7, 8, 9), A(8, 9, 10, 10) },
            Prints(p =>
            {
                int[] a = { 7, 8, 9, 10 };
                for (int i = 1; i < a.Length; i = i + 1) a[i] = a[i - 1];
                p(Show(a));
            })),

        new(17, 3, ReadKind.Choose, "Now the loop goes from right to left. " + WhatArray, WhatArrayHe,
            """
            int[] a = {7, 8, 9, 10};
            for (int i = a.Length - 1; i > 0; i = i - 1)
            {
                a[i] = a[i - 1];
            }
            """,
            new[] { A(7, 7, 8, 9), A(7, 7, 7, 7), A(10, 7, 8, 9), A(8, 9, 10, 10) },
            Prints(p =>
            {
                int[] a = { 7, 8, 9, 10 };
                for (int i = a.Length - 1; i > 0; i = i - 1) a[i] = a[i - 1];
                p(Show(a));
            })),

        new(18, 3, ReadKind.TypeList, WhatArray + " " + TypeHint, WhatArrayHe,
            """
            int[] a = {1, 2, 4, 5, 3};
            for (int i = 1; i < a.Length - 1; i = i + 1)
            {
                if ((a[i - 1] + a[i + 1]) % 2 == 1)
                {
                    a[i] = a[i] + 4;
                }
            }
            """,
            None,
            Prints(p =>
            {
                int[] a = { 1, 2, 4, 5, 3 };
                for (int i = 1; i < a.Length - 1; i = i + 1)
                {
                    if ((a[i - 1] + a[i + 1]) % 2 == 1) a[i] = a[i] + 4;
                }
                p(Show(a));
            })),

        // ---------- Level 4: arrays of words ----------
        new(19, 4, ReadKind.Choose, "What does colors look like after this code?", "איך ייראה המערך colors אחרי הקוד?",
            """
            string[] colors = {"red", "blue", "red", "green"};
            for (int i = 1; i < colors.Length; i = i + 1)
            {
                if (colors[i - 1] == "red")
                {
                    colors[i] = "purple";
                }
            }
            """,
            new[]
            {
                W("red", "purple", "red", "purple"),
                W("red", "purple", "purple", "purple"),
                W("purple", "blue", "purple", "green"),
                W("red", "purple", "red", "green"),
            },
            Prints(p =>
            {
                string[] colors = { "red", "blue", "red", "green" };
                for (int i = 1; i < colors.Length; i = i + 1)
                {
                    if (colors[i - 1] == "red") colors[i] = "purple";
                }
                p(Show(colors));
            })),

        new(20, 4, ReadKind.Choose, "What does zoo look like after this code?", "איך ייראה המערך zoo אחרי הקוד?",
            """
            string[] zoo = {"dog", "cat", "mouse", "cat", "mouse"};
            for (int i = 1; i < zoo.Length; i = i + 1)
            {
                if (zoo[i] == "cat" && zoo[i - 1] == "dog")
                {
                    zoo[i] = "running cat";
                }
                if (zoo[i] == "mouse" && zoo[i - 1] == "cat")
                {
                    zoo[i] = "ghost";
                }
            }
            """,
            new[]
            {
                W("dog", "running cat", "mouse", "cat", "ghost"),
                W("dog", "running cat", "ghost", "cat", "ghost"),
                W("dog", "cat", "ghost", "cat", "ghost"),
                W("dog", "running cat", "mouse", "running cat", "ghost"),
            },
            Prints(p =>
            {
                string[] zoo = { "dog", "cat", "mouse", "cat", "mouse" };
                for (int i = 1; i < zoo.Length; i = i + 1)
                {
                    if (zoo[i] == "cat" && zoo[i - 1] == "dog") zoo[i] = "running cat";
                    if (zoo[i] == "mouse" && zoo[i - 1] == "cat") zoo[i] = "ghost";
                }
                p(Show(zoo));
            })),

        new(21, 4, ReadKind.TypeNumber, WhatPrints, WhatPrintsHe,
            """
            string[] zoo = {"dog", "mouse", "live cat", "dog", "mouse", "live cat"};
            int count = 0;
            for (int i = 1; i < zoo.Length - 1; i = i + 1)
            {
                if (zoo[i - 1] == "dog" && zoo[i + 1] == "live cat")
                {
                    zoo[i] = "dead cat";
                    count = count + 1;
                }
            }
            Console.WriteLine(count);
            """,
            None,
            Prints(p =>
            {
                string[] zoo = { "dog", "mouse", "live cat", "dog", "mouse", "live cat" };
                int count = 0;
                for (int i = 1; i < zoo.Length - 1; i = i + 1)
                {
                    if (zoo[i - 1] == "dog" && zoo[i + 1] == "live cat")
                    {
                        zoo[i] = "dead cat";
                        count = count + 1;
                    }
                }
                p(count);
            })),

        new(22, 4, ReadKind.Choose, WhatPrints, WhatPrintsHe,
            """
            string[] words = {"sun", "moon", "star"};
            string s = "";
            for (int i = words.Length - 1; i >= 0; i = i - 1)
            {
                s = s + words[i];
            }
            Console.WriteLine(s);
            """,
            new[] { L("starmoonsun"), L("sunmoonstar"), L("star moon sun"), L("starmoon") },
            Prints(p =>
            {
                string[] words = { "sun", "moon", "star" };
                string s = "";
                for (int i = words.Length - 1; i >= 0; i = i - 1) s = s + words[i];
                p(s);
            })),

        // ---------- Level 5: follow the pointer ----------
        new(23, 5, ReadKind.TypeNumber, WhatPrints, WhatPrintsHe,
            """
            int[] next = {3, 0, 4, 1, 2};
            int i = 0;
            for (int step = 0; step < 4; step = step + 1)
            {
                i = next[i];
            }
            Console.WriteLine(i);
            """,
            None,
            Prints(p =>
            {
                int[] next = { 3, 0, 4, 1, 2 };
                int i = 0;
                for (int step = 0; step < 4; step = step + 1) i = next[i];
                p(i);
            })),

        new(24, 5, ReadKind.Choose, WhatPrints, WhatPrintsHe,
            """
            int[] next = {2, 4, 1, 0, 3};
            int i = 0;
            for (int step = 0; step < 5; step = step + 1)
            {
                i = next[i];
                Console.WriteLine(i);
            }
            """,
            new[] { L(2, 1, 4, 3, 0), L(2, 4, 1, 0, 3), L(0, 2, 1, 4, 3), L(2, 1, 2, 1, 2) },
            Prints(p =>
            {
                int[] next = { 2, 4, 1, 0, 3 };
                int i = 0;
                for (int step = 0; step < 5; step = step + 1)
                {
                    i = next[i];
                    p(i);
                }
            })),

        new(25, 5, ReadKind.TypeNumber, WhatPrints, WhatPrintsHe,
            """
            int[] next = {1, 3, 0, 2};
            int i = next[0];
            int count = 1;
            while (i != 0)
            {
                i = next[i];
                count = count + 1;
            }
            Console.WriteLine(count);
            """,
            None,
            Prints(p =>
            {
                int[] next = { 1, 3, 0, 2 };
                int i = next[0];
                int count = 1;
                while (i != 0)
                {
                    i = next[i];
                    count = count + 1;
                }
                p(count);
            })),

        new(26, 5, ReadKind.Choose, WhatPrints, WhatPrintsHe,
            """
            int[] next = {2, 0, 5, 1};
            int i = 0;
            for (int step = 0; step < 3; step = step + 1)
            {
                i = next[i];
                Console.WriteLine(i);
            }
            """,
            new[] { L("2", "5", Crash), L(2, 5, 0), L(2, 1, 0), L("2", Crash) },
            Prints(p =>
            {
                int[] next = { 2, 0, 5, 1 };
                int i = 0;
                for (int step = 0; step < 3; step = step + 1)
                {
                    i = next[i];
                    p(i);
                }
            })),

        // ---------- Level 6: 2D arrays ----------
        new(27, 6, ReadKind.TypeNumber, WhatPrints, WhatPrintsHe,
            """
            int[,] grid = {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            };
            Console.WriteLine(grid[1, 2]);
            """,
            None,
            Prints(p => p(Grid123[1, 2]))),

        new(28, 6, ReadKind.Choose, WhatPrints, WhatPrintsHe,
            """
            int[,] grid = new int[2, 4];
            Console.WriteLine(grid.GetLength(0));
            Console.WriteLine(grid.GetLength(1));
            Console.WriteLine(grid.Length);
            """,
            new[] { L(2, 4, 8), L(4, 2, 8), L(2, 4, 2), L(2, 4, 6) },
            Prints(p =>
            {
                int[,] grid = new int[2, 4];
                p(grid.GetLength(0));
                p(grid.GetLength(1));
                p(grid.Length);
            })),

        new(29, 6, ReadKind.TypeNumber, WhatPrints, WhatPrintsHe,
            """
            int[,] grid = {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            };
            int sum = 0;
            for (int i = 0; i < 3; i = i + 1)
            {
                sum = sum + grid[i, 2 - i];
            }
            Console.WriteLine(sum);
            """,
            None,
            Prints(p =>
            {
                int sum = 0;
                for (int i = 0; i < 3; i = i + 1) sum = sum + Grid123[i, 2 - i];
                p(sum);
            })),

        new(30, 6, ReadKind.Choose, "This code swaps grid[row, col] with grid[col, row]. " + WhatGrid, WhatGridHe,
            """
            int[,] grid = {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            };
            for (int row = 0; row < 3; row = row + 1)
            {
                for (int col = 0; col < 3; col = col + 1)
                {
                    int temp = grid[row, col];
                    grid[row, col] = grid[col, row];
                    grid[col, row] = temp;
                }
            }
            """,
            new[] { Rows(Grid123), Rows(Grid147), Rows(Grid987), Rows(Grid111) },
            Prints(p =>
            {
                var grid = (int[,])Grid123.Clone();
                for (int row = 0; row < 3; row = row + 1)
                {
                    for (int col = 0; col < 3; col = col + 1)
                    {
                        int temp = grid[row, col];
                        grid[row, col] = grid[col, row];
                        grid[col, row] = temp;
                    }
                }
                foreach (var line in Rows(grid)) p(line);
            })),

        new(31, 6, ReadKind.Choose, "Now col starts at row + 1. " + WhatGrid, WhatGridHe,
            """
            int[,] grid = {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            };
            for (int row = 0; row < 3; row = row + 1)
            {
                for (int col = row + 1; col < 3; col = col + 1)
                {
                    int temp = grid[row, col];
                    grid[row, col] = grid[col, row];
                    grid[col, row] = temp;
                }
            }
            """,
            new[] { Rows(Grid123), Rows(Grid147), Rows(Grid987), Rows(Grid111) },
            Prints(p =>
            {
                var grid = (int[,])Grid123.Clone();
                for (int row = 0; row < 3; row = row + 1)
                {
                    for (int col = row + 1; col < 3; col = col + 1)
                    {
                        int temp = grid[row, col];
                        grid[row, col] = grid[col, row];
                        grid[col, row] = temp;
                    }
                }
                foreach (var line in Rows(grid)) p(line);
            })),
    };

    // 5, but hidden from the compiler so it doesn't warn about an index it can see is too big.
    static int Five() => 5;

    public static IEnumerable<string> Validate() => Reading.Validate(All);
}
