namespace LearnToProgram.Practice.LinkedList;

// Id matches the worksheet numbering (1-12, with 11a and 11b). The tests for each
// problem are in LinkedListSource.Harness.
public sealed record LinkedListProblem(
    string Id, string Question, string ReturnType, string Name, string Params, string StartingCode, int TestCount,
    bool IsExample = false)
{
    public string Anchor => "ll" + Id;

    public string Signature => $"public static {ReturnType} {Name}({Params})";
}

public static class LinkedListProblems
{
    const string SumEvenExample = """
        if (lst == null)
        {
            return 0;
        }
        int result = 0;
        while (lst != null)
        {
            if (lst.GetValue() % 2 == 0)
            {
                result += lst.GetValue();
            }
            lst = lst.GetNext();
        }
        return result;
        """;

    public static readonly LinkedListProblem[] All =
    {
        new("1", "Count the nodes in the list.",
            "int", "CountList", "IntNode lst", "// TODO\nreturn 0;", 4),
        new("2", "The sum of the even numbers in the list. This one is already done, as an example.",
            "int", "SumEvenElements", "IntNode lst", SumEvenExample, 6, IsExample: true),
        new("3", "true if x is somewhere in the list, otherwise false.",
            "bool", "HasValue", "IntNode lst, int x", "// TODO\nreturn false;", 7),
        new("4", "The largest number in the list. The list is never empty.",
            "int", "FindMaximum", "IntNode lst", "// TODO\nreturn 0;", 5),
        new("5", "Add 1 to the value in every node.",
            "void", "IncrementAll", "IntNode lst", "// TODO", 3),
        new("6", "Add a new node with val at the end of the list. The list is never empty.",
            "void", "AddLast", "IntNode lst, int val", "// TODO", 3),
        new("7", "Remove the first node that holds x. If x is not in the list, change nothing. " +
                 "Return the head of the list (it changes if x was in the first node).",
            "IntNode", "RemoveFirst", "IntNode lst, int x", "// TODO\nreturn null;", 7),
        new("8", "Remove every node that holds an even number. Return the head of the list.",
            "IntNode", "RemoveEvens", "IntNode lst", "// TODO\nreturn null;", 6),
        new("9", "true if every number is smaller than the one after it, otherwise false. " +
                 "An empty list and a list with one node are sorted.",
            "bool", "IsSorted", "IntNode lst", "// TODO\nreturn false;", 7),
        new("10", "chain1 and chain2 are sorted from small to large. Return a NEW sorted list with all " +
                  "their numbers. Don't change chain1 or chain2.",
            "IntNode", "Merge", "IntNode chain1, IntNode chain2", "// TODO\nreturn null;", 6),
        new("11a", "Return a NEW list with the same numbers in reverse order. Don't change the original list.",
            "IntNode", "ReverseCopy", "IntNode lst", "// TODO\nreturn null;", 4),
        new("11b", "Challenge: reverse the list without creating any new nodes. Change the links so each " +
                   "node points to the one that was before it. Return the new head.",
            "IntNode", "Reverse", "IntNode lst", "// TODO\nreturn null;", 4),
        new("12", "The length of the longest run of neighbouring nodes with the same value. " +
                  "For [1 -> 2 -> 2 -> 2 -> 3 -> 3] the answer is 3.",
            "int", "LongestRun", "IntNode lst", "// TODO\nreturn 0;", 8),
    };
}
