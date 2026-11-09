namespace LearnToProgram.Practice.Writing;

// The "Write the code" pages (Linked List 2, Arrays 2): students write method bodies, which are compiled
// in the browser together with a test harness (plain C# source text) and run against its tests.

// One method to write. The harness runs the tests for a problem when asked for its Id.
public sealed record WriteProblem(
    string Id, string Question, string ReturnType, string Name, string Params, string StartingCode, int TestCount,
    bool IsExample = false)
{
    public string Signature => $"public static {ReturnType} {Name}({Params})";
}

public sealed record WriteRow(string Call, string Got, string Expected, bool Ok);

public sealed record WriteResult(Outcome Outcome, IReadOnlyList<CompileMessage> Errors, IReadOnlyList<WriteRow> Rows)
{
    public int Correct => Rows.Count(r => r.Ok);

    public static readonly WriteResult NotDoneYet =
        new(Outcome.NotDone, Array.Empty<CompileMessage>(), Array.Empty<WriteRow>());
}

// Everything that differs between the "Write the code" pages.
//   Header:  usings and types the student code can use (such as IntNode). HarnessRunner adds the loop guard.
//   Harness: a class named __Harness with "public static string[][] Run(string id)" that calls the
//            student's methods (Student.Name(...)) and returns rows of { call, got, expected, "1" or "0" }.
//   Hint:    a tip for a compiler error (id, message), or null to use the general tips.
//   Tips:    plain-language tips for a result whose tests failed.
public sealed record WriteCourse(
    string Name,
    WriteProblem[] Problems,
    string Header,
    string Harness,
    Func<string, string, string?> Hint,
    Func<WriteResult, IEnumerable<string>> Tips);
