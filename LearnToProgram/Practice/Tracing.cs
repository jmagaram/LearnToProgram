namespace LearnToProgram.Practice;

// Questions for the Conditions 1 page: students read a short piece of code and choose what it prints.
// No typing of code is needed.
//
// Each question has the code students see (Code) and the same code as a C# lambda (Run) that the page
// uses to check answers. Keep the two in sync: Code is what students read, Run decides what is correct.
// In Run, p("...") stands for Console.WriteLine("...").

public enum TraceKind
{
    // For each input row, choose what the code prints.
    Trace,

    // For each output, type a number x that makes the code print exactly that.
    Find,
}

public sealed record TraceLevel(int Number, string Title, string NotesHtml, string? NotesHebrew = null);

public sealed record TraceQuestion(
    int Number,
    int Level,
    TraceKind Kind,
    string Instruction,
    string? InstructionHebrew,
    string[] Params,
    string Code,
    int[][] Inputs,          // Trace: one row per input
    string[][] Choices,      // Trace: the possible outputs. Find: the outputs to aim for.
    Func<int[], string[]> Run)
{
    public string[] Expected(int[] args) => Run(args);

    public bool Matches(int[] args, string[] output) => Run(args).SequenceEqual(output);
}

public static class Tracing
{
    public static readonly TraceLevel[] Levels =
    {
        new(1, "if",
            "The code inside <code>{ }</code> runs only when the condition is <b>true</b>. " +
            "Lines after the <code>if</code> always run."),
        new(2, "if and else",
            "<code>else</code> runs when the condition is <b>false</b>. Exactly one of the two parts runs, never both."),
        new(3, "else if",
            "C# checks the conditions from top to bottom and runs only the <b>first</b> one that is true. " +
            "Separate <code>if</code>s are different: each one is checked, so more than one can run.",
            "ב־else if רק החלק הראשון שהתנאי שלו נכון רץ. ב־if נפרדים, כל if נבדק בנפרד ויכול להיות שכמה ירוצו."),
        new(4, "AND && and OR ||",
            "<ul><li><code>a &amp;&amp; b</code> is true only when <b>both</b> are true</li>" +
            "<li><code>a || b</code> is true when <b>at least one</b> is true</li></ul>",
            "&& (וגם) נכון רק כששני התנאים נכונים. || (או) נכון כשלפחות אחד מהם נכון."),
        new(5, "Putting it together",
            "An <code>if</code> can be inside another <code>if</code>. A variable can change in the middle of the code, " +
            "so read the lines in order."),
        new(6, "Work backwards",
            "Now you get the output. Type a number <code>x</code> that makes the code print exactly that. " +
            "There is more than one right answer.",
            "עכשיו נתון הפלט. כתבו מספר x שיגרום לקוד להדפיס בדיוק את זה. יש יותר מתשובה נכונה אחת."),
    };

    static readonly string[] Nothing = Array.Empty<string>();

    public static readonly TraceQuestion[] All =
    {
        // ---------- Level 1: if ----------
        Trace(1, 1, null, null,
            """
            if (x > 10)
            {
                Console.WriteLine("Big");
            }
            Console.WriteLine("Done");
            """,
            new[] { 5, 10, 11, 50 },
            new[] { L("Big", "Done"), L("Big"), L("Done"), Nothing },
            (x, p) =>
            {
                if (x > 10) p("Big");
                p("Done");
            }),

        Trace(2, 1, null, null,
            """
            if (x < 100)
            {
                Console.WriteLine("Apple");
            }
            if (x > 50)
            {
                Console.WriteLine("Banana");
            }
            """,
            new[] { 23, 67, 142, 50, 100 },
            new[] { L("Apple"), L("Banana"), L("Apple", "Banana"), Nothing },
            (x, p) =>
            {
                if (x < 100) p("Apple");
                if (x > 50) p("Banana");
            }),

        // ---------- Level 2: if and else ----------
        Trace(3, 2, null, null,
            """
            if (age >= 18)
            {
                Console.WriteLine("Adult");
            }
            else
            {
                Console.WriteLine("Kid");
            }
            """,
            new[] { 12, 17, 18, 30 },
            new[] { L("Adult"), L("Kid"), L("Adult", "Kid"), Nothing },
            (age, p) =>
            {
                if (age >= 18) p("Adult");
                else p("Kid");
            },
            "age"),

        Trace(4, 2, null, null,
            """
            if (temp <= 20)
            {
                Console.WriteLine("Coat");
            }
            else
            {
                Console.WriteLine("T-shirt");
            }
            Console.WriteLine("Go outside");
            """,
            new[] { 15, 20, 21, 35 },
            new[] { L("Coat", "Go outside"), L("T-shirt", "Go outside"), L("Coat"), L("T-shirt") },
            (temp, p) =>
            {
                if (temp <= 20) p("Coat");
                else p("T-shirt");
                p("Go outside");
            },
            "temp"),

        Trace(5, 2, null, null,
            """
            if (x == 0)
            {
                Console.WriteLine("Zero");
            }
            else
            {
                Console.WriteLine("Not zero");
            }
            if (x != 5)
            {
                Console.WriteLine("Not five");
            }
            """,
            new[] { 0, 5, 7, -5 },
            new[] { L("Zero"), L("Not zero"), L("Zero", "Not five"), L("Not zero", "Not five") },
            (x, p) =>
            {
                if (x == 0) p("Zero");
                else p("Not zero");
                if (x != 5) p("Not five");
            }),

        // ---------- Level 3: else if ----------
        Trace(6, 3, null, null,
            """
            if (grade >= 90)
            {
                Console.WriteLine("Excellent");
            }
            else if (grade >= 70)
            {
                Console.WriteLine("Good");
            }
            else
            {
                Console.WriteLine("Keep practicing");
            }
            """,
            new[] { 95, 90, 75, 70, 40 },
            new[] { L("Excellent"), L("Good"), L("Keep practicing"), L("Excellent", "Good") },
            (grade, p) =>
            {
                if (grade >= 90) p("Excellent");
                else if (grade >= 70) p("Good");
                else p("Keep practicing");
            },
            "grade"),

        Trace(7, 3,
            "This is question 2 again, but with else if. Compare your answers!",
            "זו שאלה 2 שוב, אבל עם else if\u200F. השוו את התשובות!",
            """
            if (x < 100)
            {
                Console.WriteLine("Apple");
            }
            else if (x > 50)
            {
                Console.WriteLine("Banana");
            }
            """,
            new[] { 23, 67, 142, 50, 100 },
            new[] { L("Apple"), L("Banana"), L("Apple", "Banana"), Nothing },
            (x, p) =>
            {
                if (x < 100) p("Apple");
                else if (x > 50) p("Banana");
            }),

        Trace(8, 3, "These are three separate ifs, not else if.", "אלה שלושה if נפרדים, לא else if\u200F.",
            """
            if (x > 0)
            {
                Console.WriteLine("Positive");
            }
            if (x > 10)
            {
                Console.WriteLine("Big");
            }
            if (x > 100)
            {
                Console.WriteLine("Huge");
            }
            """,
            new[] { 5, 50, 500, 0, -3 },
            new[] { L("Positive"), L("Positive", "Big"), L("Positive", "Big", "Huge"), L("Huge"), Nothing },
            (x, p) =>
            {
                if (x > 0) p("Positive");
                if (x > 10) p("Big");
                if (x > 100) p("Huge");
            }),

        // ---------- Level 4: && and || ----------
        Trace(9, 4, null, null,
            """
            if (age >= 13 && age <= 19)
            {
                Console.WriteLine("Teen");
            }
            else
            {
                Console.WriteLine("Not a teen");
            }
            """,
            new[] { 12, 13, 16, 19, 20 },
            new[] { L("Teen"), L("Not a teen"), L("Teen", "Not a teen") },
            (age, p) =>
            {
                if (age >= 13 && age <= 19) p("Teen");
                else p("Not a teen");
            },
            "age"),

        Trace(10, 4, null, null,
            """
            if (x < 0 || x > 100)
            {
                Console.WriteLine("Error");
            }
            else
            {
                Console.WriteLine("OK");
            }
            """,
            new[] { -5, 0, 50, 100, 101 },
            new[] { L("Error"), L("OK"), L("Error", "OK") },
            (x, p) =>
            {
                if (x < 0 || x > 100) p("Error");
                else p("OK");
            }),

        Trace(11, 4, null, null,
            """
            if (x > 5 && x < 10)
            {
                Console.WriteLine("A");
            }
            if (x < 3 || x > 8)
            {
                Console.WriteLine("B");
            }
            """,
            new[] { 7, 9, 20, 2, 3 },
            new[] { L("A"), L("B"), L("A", "B"), Nothing },
            (x, p) =>
            {
                if (x > 5 && x < 10) p("A");
                if (x < 3 || x > 8) p("B");
            }),

        Trace(12, 4, "Now there are two numbers, x and y.", "עכשיו יש שני מספרים, x ו־y\u200F.",
            """
            if (x > 0 && y > 0)
            {
                Console.WriteLine("Both positive");
            }
            else if (x > 0 || y > 0)
            {
                Console.WriteLine("One positive");
            }
            else
            {
                Console.WriteLine("None positive");
            }
            """,
            new[] { new[] { 3, 5 }, new[] { 3, -5 }, new[] { -3, 5 }, new[] { 0, 0 }, new[] { -1, 8 } },
            new[] { L("Both positive"), L("One positive"), L("None positive"), L("Both positive", "One positive") },
            (a, p) =>
            {
                int x = a[0], y = a[1];
                if (x > 0 && y > 0) p("Both positive");
                else if (x > 0 || y > 0) p("One positive");
                else p("None positive");
            },
            new[] { "x", "y" }),

        // ---------- Level 5: putting it together ----------
        Trace(13, 5, "There is an if inside an if. x is a positive number.", "יש if בתוך if. x הוא מספר חיובי.",
            """
            if (x >= 10)
            {
                if (x >= 100)
                {
                    Console.WriteLine("Three digits");
                }
                else
                {
                    Console.WriteLine("Two digits");
                }
            }
            else
            {
                Console.WriteLine("One digit");
            }
            """,
            new[] { 7, 10, 99, 100, 250 },
            new[] { L("One digit"), L("Two digits"), L("Three digits"), L("Two digits", "Three digits") },
            (x, p) =>
            {
                if (x >= 10)
                {
                    if (x >= 100) p("Three digits");
                    else p("Two digits");
                }
                else p("One digit");
            }),

        Trace(14, 5, "Careful: x changes in the middle!", "זהירות: x משתנה באמצע!",
            """
            if (x > 5)
            {
                x = x - 5;
                Console.WriteLine("Minus");
            }
            if (x > 5)
            {
                Console.WriteLine("Still big");
            }
            """,
            new[] { 3, 8, 12, 10 },
            new[] { L("Minus"), L("Still big"), L("Minus", "Still big"), Nothing },
            (x, p) =>
            {
                if (x > 5)
                {
                    x = x - 5;
                    p("Minus");
                }
                if (x > 5) p("Still big");
            }),

        // ---------- Level 6: work backwards ----------
        Find(15, 6,
            """
            if (x < 100)
            {
                Console.WriteLine("Apple");
            }
            if (x > 50)
            {
                Console.WriteLine("Banana");
            }
            """,
            new[] { L("Apple"), L("Banana"), L("Apple", "Banana") },
            (x, p) =>
            {
                if (x < 100) p("Apple");
                if (x > 50) p("Banana");
            }),

        Find(16, 6,
            """
            if (x > 5 && x < 10)
            {
                Console.WriteLine("A");
            }
            if (x < 3 || x > 8)
            {
                Console.WriteLine("B");
            }
            """,
            new[] { L("A"), L("A", "B"), Nothing },
            (x, p) =>
            {
                if (x > 5 && x < 10) p("A");
                if (x < 3 || x > 8) p("B");
            }),
    };

    static string[] L(params string[] lines) => lines;

    static TraceQuestion Trace(int number, int level, string? instruction, string? hebrew, string code,
        int[] xs, string[][] choices, Action<int, Action<string>> run, string param = "x") =>
        Trace(number, level, instruction, hebrew, code, xs.Select(x => new[] { x }).ToArray(), choices,
            (a, p) => run(a[0], p), new[] { param });

    static TraceQuestion Trace(int number, int level, string? instruction, string? hebrew, string code,
        int[][] inputs, string[][] choices, Action<int[], Action<string>> run, string[] parameters) =>
        new(number, level, TraceKind.Trace, instruction ?? "What does the code print?", hebrew,
            parameters, code, inputs, choices, Capture(run));

    static TraceQuestion Find(int number, int level, string code, string[][] targets, Action<int, Action<string>> run) =>
        new(number, level, TraceKind.Find, "Type a number x that makes the code print exactly this.", null,
            new[] { "x" }, code, Array.Empty<int[]>(), targets, Capture((a, p) => run(a[0], p)));

    static Func<int[], string[]> Capture(Action<int[], Action<string>> run) => args =>
    {
        var lines = new List<string>();
        run(args, lines.Add);
        return lines.ToArray();
    };
}
