using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace LearnToProgram.Practice;

public enum Outcome { NotDone, CompileError, Pass, Fail }

public sealed record CompileMessage(int? Line, int? Column, string Id, string Message, string? Hint);

public sealed record TestRow(int[] Args, bool Expected, bool? Got, string? Crash)
{
    public bool Ok => Crash is null && Got == Expected;
}

public sealed record ProblemResult(Outcome Outcome, IReadOnlyList<CompileMessage> Errors, IReadOnlyList<TestRow> Rows)
{
    public int Correct => Rows.Count(r => r.Ok);

    public static readonly ProblemResult NotDoneYet = new(Outcome.NotDone, Array.Empty<CompileMessage>(), Array.Empty<TestRow>());
}

// Compiles students' method bodies with the real C# compiler (Roslyn), which runs
// inside the browser on WebAssembly, then runs the test values against them.
public sealed class CSharpRunner
{
    // Reference assemblies (System.Runtime, System.Console) are embedded in this app
    // by the project file, because WebAssembly apps have no DLL files on disk to point at.
    internal static readonly Lazy<MetadataReference[]> References = new(() =>
    {
        var assembly = typeof(CSharpRunner).Assembly;
        return assembly.GetManifestResourceNames()
            .Where(n => n.StartsWith("ref/", StringComparison.Ordinal))
            .Select(n =>
            {
                using var stream = assembly.GetManifestResourceStream(n)!;
                return (MetadataReference)MetadataReference.CreateFromStream(stream);
            })
            .ToArray();
    });

    internal static readonly CSharpParseOptions ParseOptions = new(LanguageVersion.CSharp12);

    // The browser has a single thread, so build without parallelism.
    internal static readonly CSharpCompilationOptions CompileOptions = new(
        OutputKind.DynamicallyLinkedLibrary,
        optimizationLevel: OptimizationLevel.Release,
        concurrentBuild: false);

    int compileCount;

    public TimeSpan LastElapsed { get; private set; }

    // Compile something small so the first real Run doesn't pay the start-up cost.
    public void WarmUp()
    {
        var p = Problems.All[0];
        Run(new[] { (p, "return x < 10;") });
        Console.WriteLine("C# compiler ready.");
    }

    public Dictionary<int, ProblemResult> Run(IReadOnlyList<(Problem Problem, string Body)> items)
    {
        var watch = Stopwatch.StartNew();
        var results = new Dictionary<int, ProblemResult>();
        var pending = new List<(Problem Problem, string Body)>();
        foreach (var item in items)
        {
            if (string.IsNullOrWhiteSpace(item.Body)) results[item.Problem.Number] = ProblemResult.NotDoneYet;
            else pending.Add(item);
        }

        // Compile everything together. Problems with errors are reported and left out,
        // and the rest are compiled again so one mistake doesn't block the others.
        Assembly? assembly = null;
        for (int attempt = 0; attempt < 5 && pending.Count > 0; attempt++)
        {
            var (source, ranges) = BuildSource(pending);
            var compilation = CSharpCompilation.Create(
                "StudentCode" + (++compileCount),
                new[] { CSharpSyntaxTree.ParseText(source, ParseOptions) },
                References.Value,
                CompileOptions);

            using var stream = new MemoryStream();
            var emit = compilation.Emit(stream);
            if (emit.Success)
            {
                assembly = Assembly.Load(stream.ToArray());
                break;
            }

            var errors = emit.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).ToList();
            var byProblem = errors
                .GroupBy(d => FindProblem(d, ranges) ?? pending[^1].Problem.Number)
                .ToList();
            foreach (var group in byProblem)
            {
                var messages = group
                    .Select(ToMessage)
                    .DistinctBy(m => (m.Line, m.Column, m.Id))
                    .Take(5)
                    .ToList();
                results[group.Key] = new ProblemResult(Outcome.CompileError, messages, Array.Empty<TestRow>());
            }
            pending.RemoveAll(p => results.ContainsKey(p.Problem.Number));
        }

        if (assembly is not null)
        {
            var type = assembly.GetType("Student")!;
            foreach (var (problem, _) in pending)
            {
                var method = type.GetMethod(problem.MethodName, BindingFlags.Public | BindingFlags.Static)!;
                results[problem.Number] = RunTests(problem, method);
            }
        }
        else
        {
            foreach (var (problem, _) in pending)
            {
                results[problem.Number] = new ProblemResult(Outcome.CompileError,
                    new[] { new CompileMessage(null, null, "", "This code could not be compiled.", null) },
                    Array.Empty<TestRow>());
            }
        }

        LastElapsed = watch.Elapsed;
        return results;
    }

    static ProblemResult RunTests(Problem problem, MethodInfo method)
    {
        var rows = new List<TestRow>();
        foreach (var test in problem.Tests)
        {
            try
            {
                var got = (bool)method.Invoke(null, test.Args.Cast<object>().ToArray())!;
                rows.Add(new TestRow(test.Args, test.Expected, got, null));
            }
            catch (TargetInvocationException ex) when (ex.InnerException is NotImplementedException)
            {
                return ProblemResult.NotDoneYet;
            }
            catch (TargetInvocationException ex)
            {
                rows.Add(new TestRow(test.Args, test.Expected, null, ex.InnerException?.GetType().Name ?? "Exception"));
            }
        }
        var outcome = rows.All(r => r.Ok) ? Outcome.Pass : Outcome.Fail;
        return new ProblemResult(outcome, Array.Empty<CompileMessage>(), rows);
    }

    // Each student's body sits under a "#line 1" directive named after its problem,
    // so compiler errors report the problem and line number inside the student's code.
    static (string Source, List<(int Number, int StartLine, int EndLine)> Ranges) BuildSource(
        IEnumerable<(Problem Problem, string Body)> items)
    {
        var sb = new StringBuilder();
        var ranges = new List<(int, int, int)>();
        int line = 0;
        void Add(string text)
        {
            sb.Append(text).Append('\n');
            line += text.Count(c => c == '\n') + 1;
        }

        Add("using System;");
        Add("public static class Student");
        Add("{");
        Add("    static bool TODO { get { throw new NotImplementedException(); } }");
        foreach (var (problem, body) in items)
        {
            int start = line;
            Add("    public " + problem.Signature);
            Add("    {");
            Add("#line 1 \"" + problem.MethodName + "\"");
            Add(body.Replace("\r\n", "\n").TrimEnd('\n'));
            Add("#line default");
            Add("    }");
            ranges.Add((problem.Number, start, line - 1));
        }
        Add("}");
        return (sb.ToString(), ranges);
    }

    static int? FindProblem(Diagnostic d, List<(int Number, int StartLine, int EndLine)> ranges)
    {
        if (d.Location == Location.None) return null;
        var mapped = d.Location.GetMappedLineSpan();
        if (mapped.HasMappedPath && mapped.Path.StartsWith('P') && int.TryParse(mapped.Path[1..], out var n)) return n;
        var line = d.Location.GetLineSpan().StartLinePosition.Line;
        foreach (var r in ranges)
        {
            if (line >= r.StartLine && line <= r.EndLine) return r.Number;
        }
        return null;
    }

    internal static CompileMessage ToMessage(Diagnostic d)
    {
        // Hide the generated class name: "'Student.P12(int)'" reads better as "'P12(int)'".
        var message = d.GetMessage(CultureInfo.InvariantCulture).Replace("'Student.", "'");
        int? lineNo = null, col = null;
        if (d.Location != Location.None)
        {
            var mapped = d.Location.GetMappedLineSpan();
            if (mapped.HasMappedPath)
            {
                lineNo = mapped.StartLinePosition.Line + 1;
                col = mapped.StartLinePosition.Character + 1;
            }
        }
        return new CompileMessage(lineNo, col, d.Id, message, HintFor(d.Id, message));
    }

    // Plain-language tips for the compiler errors beginners hit most. The real
    // compiler message is always shown too.
    static string? HintFor(string id, string message) => id switch
    {
        "CS0029" when message.Contains("'int' to 'bool'") =>
            "Your code gives a number, not true or false. To compare, use == (two equals signs), <, >, <=, >= or !=. A single = means \"store a value\".",
        "CS0019" when message.Contains("'bool' and 'int'") || message.Contains("'int' and 'bool'") =>
            "One side is a number and the other is true/false. C# can't chain comparisons like 10 <= x <= 20: write 10 <= x && x <= 20. Each side of && and || must be a full comparison.",
        "CS0023" when message.Contains("'!'") =>
            "! only works on true/false. Use parentheses, as in !(x == 0), or write x != 0.",
        "CS0103" =>
            "C# cares about upper and lower case, and only knows the names in this method (like x and y).",
        "CS1002" => "Each statement must end with a semicolon ;",
        "CS0161" =>
            "Every way through your method must reach a return. If you use if, add an else, or put a return at the end.",
        "CS1525" or "CS1026" or "CS1513" or "CS1514" or "CS1003" =>
            "Check your brackets and operators: every ( needs a ), and every { needs a }.",
        "CS0165" => "Give the variable a value before you use it.",
        "CS0020" => "You can't divide by zero.",
        _ => null,
    };
}
