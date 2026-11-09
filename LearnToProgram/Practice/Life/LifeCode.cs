using System.Reflection;

namespace LearnToProgram.Practice.Life;

// The student's NewCell from compiled code (HarnessRunner.LastAssembly), ready for the board to call once
// per cell. Calls are direct (a delegate, not reflection), so a whole grid takes milliseconds.
public sealed class LifeCode
{
    readonly Action resetGuard;
    readonly Func<bool[], int, bool>? row;
    readonly Func<bool[,], int, int, bool>? grid;

    public Assembly Assembly { get; }

    LifeCode(Assembly assembly, Action resetGuard, Func<bool[], int, bool>? row, Func<bool[,], int, int, bool>? grid)
    {
        Assembly = assembly;
        this.resetGuard = resetGuard;
        this.row = row;
        this.grid = grid;
    }

    // Null if the code has no NewCell with the expected parameters (it always does after HarnessRunner).
    public static LifeCode? From(Assembly? assembly)
    {
        var student = assembly?.GetType("Student");
        var reset = assembly?.GetType("__Guard")?.GetMethod("Reset", BindingFlags.Public | BindingFlags.Static);
        if (assembly is null || student is null || reset is null) return null;
        var flags = BindingFlags.Public | BindingFlags.Static;
        var rowMethod = student.GetMethod("NewCell", flags, new[] { typeof(bool[]), typeof(int) });
        var gridMethod = student.GetMethod("NewCell", flags, new[] { typeof(bool[,]), typeof(int), typeof(int) });
        if (rowMethod is null && gridMethod is null) return null;
        return new LifeCode(
            assembly,
            reset.CreateDelegate<Action>(),
            rowMethod?.CreateDelegate<Func<bool[], int, bool>>(),
            gridMethod?.CreateDelegate<Func<bool[,], int, int, bool>>());
    }

    public bool NewCell(bool[] cells, int i)
    {
        resetGuard();
        return row!(cells, i);
    }

    public bool NewCell(bool[,] cells, int r, int c)
    {
        resetGuard();
        return grid!(cells, r, c);
    }

    // What went wrong inside the student's code, in plain words.
    public static string Explain(Exception e) => e.GetType().Name switch
    {
        "__LoopNeverEnds" => "a loop never ends.",
        "__TooManyCalls" => "a method keeps calling itself.",
        "NotImplementedException" => "one of your methods isn't done yet or doesn't compile. Press Run all to see which.",
        "IndexOutOfRangeException" => "it looked outside the cells (IndexOutOfRangeException). Use IsAlive, which checks the edges.",
        var name => "it crashed with " + name + ".",
    };
}
