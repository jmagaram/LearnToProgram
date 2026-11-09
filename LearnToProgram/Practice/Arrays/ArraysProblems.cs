using LearnToProgram.Practice.Writing;

namespace LearnToProgram.Practice.Arrays;

// The Arrays 2 problems: first without loops, then with loops. The tests for each problem are in
// ArraysSource.Harness, under the same Id. TestCount must match the number of tests there.
public static class ArraysProblems
{
    const string SumExample = """
        int total = 0;
        for (int i = 0; i < a.Length; i = i + 1)
        {
            total = total + a[i];
        }
        return total;
        """;

    const string Todo0 = "// TODO\nreturn 0;";
    const string TodoFalse = "// TODO\nreturn false;";
    const string Todo = "// TODO";

    public static readonly WriteProblem[] All =
    {
        // ---------- No loops ----------
        new("1", "The first number in a. The array is never empty.",
            "int", "GetFirst", "int[] a", Todo0, 4),
        new("2", "The last number in a. Use a.Length. The array is never empty.",
            "int", "GetLast", "int[] a", Todo0, 5),
        new("3", "The first number plus the last number. The array is never empty.",
            "int", "SumOfEnds", "int[] a", Todo0, 5),
        new("4", "Swap the first and the last number in a. The array is never empty.",
            "void", "SwapFirstAndLast", "int[] a", Todo, 5),
        new("5", "start has 3 numbers. Put the same 3 numbers into sorted, from small to large. " +
                 "Use only if and else (no loops). Don't change start.",
            "void", "Sort3", "int[] start, int[] sorted", Todo, 8),
        new("6", "Challenge: a has 3 numbers. Sort a itself from small to large, by swapping its numbers. " +
                 "Use only if and else.",
            "void", "Sort3InPlace", "int[] a", Todo, 8),
        new("7", "grid is 3 by 3. Return the number in the middle.",
            "int", "GetCenter", "int[,] grid", Todo0, 3),
        new("8", "A tic-tac-toe board is 3 by 3. Each square holds \"X\", \"O\" or \"\" (empty, shown as . in the tests). " +
                 "true if player (\"X\" or \"O\") has all three squares of the top row.",
            "bool", "TopRowWins", "string[,] board, string player", TodoFalse, 6),
        new("9", "true if player has all three squares of either diagonal: " +
                 "top-left to bottom-right, or top-right to bottom-left.",
            "bool", "DiagonalWins", "string[,] board, string player", TodoFalse, 7),

        // ---------- With loops ----------
        new("10", "The sum of all the numbers in a. This one is already done, as an example.",
            "int", "Sum", "int[] a", SumExample, 5, IsExample: true),
        new("11", "How many numbers in a are bigger than limit?",
            "int", "CountAbove", "int[] a, int limit", Todo0, 6),
        new("12", "The largest number in a. The array is never empty.",
            "int", "Max", "int[] a", Todo0, 6),
        new("13", "true if every number is bigger than the one before it. " +
                  "An empty array and an array with one number count as increasing.",
            "bool", "IsIncreasing", "int[] a", TodoFalse, 8),
        new("14", "result has the same length as a. Fill result with a's numbers in reverse order. Don't change a.",
            "void", "ReverseInto", "int[] a, int[] result", Todo, 5),
        new("15", "Challenge: reverse a itself, by swapping its numbers. Don't create a new array.",
            "void", "ReverseInPlace", "int[] a", Todo, 5),
        new("16", "true if words reads the same from both ends, like {\"up\", \"down\", \"up\"}.",
            "bool", "IsPalindrome", "string[] words", TodoFalse, 7),
        new("17", "true if every number, from the third one on, is the sum of the two before it, " +
                  "like {1, 1, 2, 3, 5, 8}. An array with fewer than 3 numbers counts as true.",
            "bool", "IsFibonacciLike", "int[] a", TodoFalse, 8),
        new("18", "true if player has a whole row, a whole column or a whole diagonal of the tic-tac-toe board. " +
                  "You can use loops, or call your other methods.",
            "bool", "Wins", "string[,] board, string player", TodoFalse, 10),
    };

    public static readonly WriteCourse Course = new("Arrays", All, ArraysSource.Header, ArraysSource.Harness, HintFor, TipsFor);

    // Tips for array compiler errors. HarnessRunner's tips are used for everything else.
    static string? HintFor(string id, string message) => id switch
    {
        "CS1955" when message.Contains("Length") =>
            "Length has no ( ). Write a.Length, not a.Length().",
        "CS1061" when message.Contains("'length'") =>
            "Length starts with a capital L: a.Length",
        "CS1061" or "CS0117" when message.Contains("'Count'") || message.Contains("'Size'") =>
            "The number of items in an array is a.Length",
        "CS0022" =>
            "A 2D array needs two indexes: grid[row, col]. A normal array needs one: a[i].",
        "CS0029" or "CS0266" when message.Contains("'int[]' to 'int'") =>
            "This is the whole array. Use a[i] to get one number from it.",
        "CS0019" when message.Contains("'int[]'") =>
            "One side is the whole array. Use a[i] to get one number from it.",
        "CS1012" or "CS0019" when message.Contains("character literal") || message.Contains("'char'") =>
            "Strings use double quotes, like \"X\". Single quotes ('X') are for a single char.",
        "CS0029" when message.Contains("'char' to 'string'") =>
            "Strings use double quotes, like \"X\". Single quotes ('X') are for a single char.",
        "CS0103" => "C# cares about upper and lower case, and only knows names that exist in this method " +
                    "(like its parameters) or that you created with a variable.",
        _ => null,
    };

    // Plain-language tips for the ways array code usually goes wrong.
    static IEnumerable<string> TipsFor(WriteResult r)
    {
        var got = r.Rows.Where(x => !x.Ok).Select(x => x.Got).ToList();
        if (got.Any(g => g.Contains("IndexOutOfRangeException")))
            yield return "Your code used an index outside the array. The indexes go from 0 to a.Length - 1.";
        if (got.Any(g => g.StartsWith("never ends: a loop")))
            yield return "A loop never stopped. Check that the loop variable changes each time, like i = i + 1.";
        if (got.Any(g => g.StartsWith("never ends: a method")))
            yield return "Your method keeps calling itself. Make sure it stops.";
        if (got.Any(g => g.StartsWith("changed ")))
            yield return "Your code changed an array it was told not to change. Only write into the other array.";
        if (got.Any(g => g.Contains("NotImplementedException")))
            yield return "This calls one of your other methods that isn't done yet or doesn't compile.";
    }
}
