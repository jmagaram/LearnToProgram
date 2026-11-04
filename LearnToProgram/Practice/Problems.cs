namespace LearnToProgram.Practice;

public sealed record TestCase(int[] Args, bool Expected);

public sealed record Level(int Number, string Title, string NotesHtml);

public sealed record Problem(int Number, int Level, string Question, string[] Params, TestCase[] Tests)
{
    public string MethodName => $"P{Number:00}";

    public string Signature => $"static bool {MethodName}({string.Join(", ", Params.Select(p => "int " + p))})";
}

// The same 24 problems and test values as ConditionsPractice.cs.
// Expected answers: T = true, F = false, one letter per test value.
public static class Problems
{
    public static readonly Level[] Levels =
    {
        new(1, "Comparisons",
            "Use <code>&lt;</code> <code>&gt;</code> <code>&lt;=</code> <code>&gt;=</code> <code>==</code> <code>!=</code>"),
        new(2, "Modulo % (the remainder after dividing)",
            "<ul><li><code>10 % 3 == 1</code> because 10 = 3*3 + 1</li>" +
            "<li><code>12 % 4 == 0</code> because 4 divides 12 exactly</li></ul>"),
        new(3, "AND && and OR ||",
            "<ul><li><code>a &amp;&amp; b</code> is true only when BOTH are true</li>" +
            "<li><code>a || b</code> is true when AT LEAST ONE is true</li></ul>"),
        new(4, "Digits (all numbers here are positive)",
            "<ul><li><code>x % 10</code> gives the LAST digit (<code>347 % 10 == 7</code>)</li>" +
            "<li><code>x / 10</code> removes the last digit (<code>347 / 10 == 34</code>)</li>" +
            "<li><code>x / 10 % 10</code> gives the TENS digit (<code>347 / 10 % 10 == 4</code>)</li>" +
            "<li><code>x / 100</code> gives the HUNDREDS digit (<code>347 / 100 == 3</code>, for x &lt; 1000)</li></ul>"),
        new(5, "Two numbers x and y",
            "Now the method gets TWO numbers. Use both of them in your condition."),
    };

    public static readonly Problem[] All =
    {
        One(1, 1, "Is x less than 10?", new[] { 3, 9, 10, 11, -5, 0, 100 }, "TTFFTTF"),
        One(2, 1, "Is x greater than 100?", new[] { 50, 99, 100, 101, 500, -200 }, "FFFTTF"),
        One(3, 1, "Is x at least 18? (old enough to vote)", new[] { 12, 17, 18, 19, 40, 0 }, "FFTTTF"),
        One(4, 1, "Is x zero or negative?", new[] { 5, 1, 0, -1, -100 }, "FFTTT"),
        One(5, 1, "Is x exactly 7?", new[] { 7, 6, 8, 70, 17, -7 }, "TFFFFF"),
        One(6, 1, "Is x NOT zero?", new[] { 0, 1, -1, 99, 10 }, "FTTTT"),

        One(7, 2, "Is x even?", new[] { 4, 7, 0, 100, 13, -6 }, "TFTTFT"),
        One(8, 2, "Is x odd?  (careful: try negative numbers!)", new[] { 3, 8, 0, 99, -3, -4, -11 }, "TFFTTFT"),
        One(9, 2, "Is x divisible by 5?", new[] { 10, 12, 0, 35, 51, 5, 4 }, "TFTTFTF"),

        One(10, 3, "Is x between 10 and 20? (10 and 20 count)", new[] { 9, 10, 15, 20, 21, 0, 100 }, "FTTTFFF"),
        One(11, 3, "Is x OUTSIDE the range 1 to 100?", new[] { 0, 1, 50, 100, 101, -5, 500 }, "TFFFTTT"),
        One(12, 3, "Is x even AND positive?", new[] { 4, 3, 0, -4, 10, 7, -3 }, "TFFFTFF"),
        One(13, 3, "Is x divisible by 3 OR by 5?", new[] { 9, 10, 15, 7, 22, 25, 1 }, "TTTFFTF"),
        One(14, 3, "Is x divisible by BOTH 3 and 5?", new[] { 15, 9, 10, 30, 7, 45, 50 }, "TFFTFTF"),
        One(15, 3, "Is x a positive two-digit number? (10 to 99)", new[] { 9, 10, 55, 99, 100, -50, 5 }, "FTTTFFF"),

        One(16, 4, "Does x end with the digit 7?", new[] { 7, 17, 70, 127, 71, 1007, 8 }, "TTFTFTF"),
        One(17, 4, "Is x a two-digit number in the 70s? (70 to 79)", new[] { 69, 70, 75, 79, 80, 7, 77 }, "FTTTFFT"),
        One(18, 4, "Is x a two-digit number with both digits the same? (11, 22, ...)",
            new[] { 33, 34, 99, 10, 77, 11, 12, 90 }, "TFTFTTFF"),
        One(19, 4, "7 BOOM! (x from 1 to 99) Is x divisible by 7, OR ends with 7, OR in the 70s?",
            new[] { 1, 7, 14, 17, 22, 49, 50, 70, 71, 77, 80, 91, 97 }, "FTTTFTFTTTFTT"),
        One(20, 4, "Does x contain the digit 3 anywhere? (x from 0 to 999)",
            new[] { 3, 13, 31, 130, 305, 300, 333, 123, 444, 100, 0, 999, 38 }, "TTTTTTTTFFFFT"),

        Two(21, 5, "Is x greater than y?",
            new[] { 5, 3, 4, -1, 0, 100, -8 },
            new[] { 3, 5, 4, -2, 10, 99, -3 }, "TFFTFTF"),
        Two(22, 5, "Are x and y BOTH even?",
            new[] { 2, 2, 3, 3, 0, 10, -4 },
            new[] { 4, 3, 2, 5, -6, 7, 8 }, "TFFFTFT"),
        Two(23, 5, "Are x and y BOTH odd?  (careful: try negative numbers!)",
            new[] { 3, 3, 4, 2, -3, -1, 0 },
            new[] { 5, 4, 3, 6, 7, -1, 1 }, "TFFFTTF"),
        Two(24, 5, "Are x and y both odd OR both even?",
            new[] { 2, 3, 2, 3, 0, -3, -4, 10, 7 },
            new[] { 4, 5, 3, 2, 0, 5, 7, -1, -9 }, "TTFFTTFFT"),
    };

    static Problem One(int number, int level, string question, int[] xs, string expected) =>
        new(number, level, question, new[] { "x" },
            Tests(expected, xs.Select(x => new[] { x }).ToArray()));

    static Problem Two(int number, int level, string question, int[] xs, int[] ys, string expected) =>
        new(number, level, question, new[] { "x", "y" },
            Tests(expected, xs.Zip(ys, (x, y) => new[] { x, y }).ToArray()));

    static TestCase[] Tests(string expected, int[][] args)
    {
        if (expected.Length != args.Length) throw new InvalidOperationException("Test data length mismatch");
        return args.Select((a, i) => new TestCase(a, expected[i] == 'T')).ToArray();
    }
}
