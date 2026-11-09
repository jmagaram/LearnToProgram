using System.Text.RegularExpressions;

namespace LearnToProgram.Practice;

// The question types for the "Read the code" pages (Linked List 1, Arrays 1): students read a short
// piece of code and choose or type what it prints or what the data looks like afterwards.
//
// Each question has the code students see (Code) and the same code as a C# lambda (Run) that the page
// uses to decide the right answer. Keep the two in sync: Code is what students read, Run decides what
// is correct.

public enum ReadKind
{
    // Pick one of the choices.
    Choose,

    // Type a whole number.
    TypeNumber,

    // Type a list or array of numbers, like 1 -> 2 -> 3 or {1, 2, 3}.
    TypeList,
}

public sealed record ReadLevel(int Number, string Title, string NotesHtml, string? NotesHebrew = null);

public sealed record ReadQuestion(
    int Number,
    int Level,
    ReadKind Kind,
    string Instruction,
    string? InstructionHebrew,
    string Code,
    string[][] Choices,      // Choose only: the possible outputs, each one or more printed lines.
    Func<string[]> Run)
{
    public string[] Expected() => Run();

    public bool Matches(string[] output) => Run().SequenceEqual(output);

    // Typed answers: spaces, brackets and the kind of arrow or comma don't matter, only the numbers.
    public bool MatchesTyped(string typed)
    {
        var want = string.Join(" ", Run());
        if (Kind == ReadKind.TypeNumber)
            return int.TryParse(typed.Trim(), out var n) && n.ToString() == want.Trim();
        var got = Numbers(typed);
        return got.Length > 0 && got.SequenceEqual(Numbers(want));
    }

    static string[] Numbers(string s) => Regex.Matches(s, @"-?\d+").Select(m => int.Parse(m.Value).ToString()).ToArray();
}

public static class Reading
{
    // Problems with the questions themselves, for the developer. Empty when everything is fine.
    public static IEnumerable<string> Validate(IEnumerable<ReadQuestion> questions)
    {
        foreach (var q in questions)
        {
            if (q.Kind == ReadKind.Choose)
            {
                var right = q.Choices.Count(q.Matches);
                if (right != 1) yield return $"Question {q.Number}: {right} choices are correct, expected exactly 1. Run prints: {string.Join(" / ", q.Run())}";
            }
            else if (q.Choices.Length != 0)
            {
                yield return $"Question {q.Number}: typed questions should not have choices.";
            }
        }
    }
}
