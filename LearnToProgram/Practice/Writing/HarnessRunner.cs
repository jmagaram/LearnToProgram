using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LearnToProgram.Practice.Writing;

// Compiles the students' method bodies together with a course's header and test harness
// (see WriteCourse), using the same in-browser compiler setup as CSharpRunner.
public sealed class HarnessRunner(WriteCourse course)
{
    const string LinePrefix = "P_";
    const string Stub = "throw new NotImplementedException();";

    int compileCount;

    public TimeSpan LastElapsed { get; private set; }

    // The student's code from the last Run, ready to call (with stubs for problems that weren't done or
    // didn't compile), or null if nothing compiled. Pages like Game of Life call the methods in it.
    public Assembly? LastAssembly { get; private set; }

    // Calls to these are added to every loop and method in student code, so a loop that
    // never ends stops with a message instead of freezing the browser.
    const string GuardSource = """

        public sealed class __LoopNeverEnds : Exception { }
        public sealed class __TooManyCalls : Exception { }

        public static class __Guard
        {
            public const int MaxLoops = 1000000;
            public const int MaxCalls = 500;
            static int loops;
            static int calls;
            public static void Reset() { loops = 0; calls = 0; }
            public static void Loop() { if (++loops > MaxLoops) throw new __LoopNeverEnds(); }
            public static void Call() { if (++calls > MaxCalls) throw new __TooManyCalls(); }
        }

        """;

    public WriteCourse Course => course;

    public static bool IsNotDone(WriteProblem p, string body) =>
        string.IsNullOrWhiteSpace(body) || (!p.IsExample && Normalize(body) == Normalize(p.StartingCode));

    static string Normalize(string code) => code.Replace("\r\n", "\n").Trim();

    public void WarmUp()
    {
        // Something that isn't the starting code, so it is really compiled and run.
        var first = course.Problems[0];
        Run(new[] { (first, "// warm up\n" + first.StartingCode) });
        Console.WriteLine($"C# compiler ready for {course.Name}.");
    }

    // Compiles all the given bodies, so a method can call the student's other methods,
    // and returns results for the problems in `report` (all of them when it is null).
    public Dictionary<string, WriteResult> Run(
        IReadOnlyList<(WriteProblem Problem, string Body)> items, IReadOnlyCollection<string>? report = null)
    {
        var watch = Stopwatch.StartNew();
        var results = new Dictionary<string, WriteResult>();
        var pending = new Dictionary<string, string>();
        foreach (var (problem, body) in items)
        {
            if (IsNotDone(problem, body)) results[problem.Id] = WriteResult.NotDoneYet;
            else pending[problem.Id] = body;
        }

        // Compile everything together. Problems with errors are reported and swapped for a
        // stub, and the rest are compiled again so one mistake doesn't block the others.
        Assembly? assembly = null;
        for (int attempt = 0; attempt < 6 && pending.Count > 0; attempt++)
        {
            var tree = CSharpSyntaxTree.ParseText(BuildSource(pending), CSharpRunner.ParseOptions);
            var compilation = Compile(tree);
            var errors = compilation.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error).ToList();
            if (errors.Count == 0)
            {
                assembly = Emit(Compile(AddGuards(tree)));
                break;
            }
            foreach (var group in errors.GroupBy(d => FindProblem(d) ?? pending.Keys.Last()))
            {
                var messages = group
                    .Select(ToMessage)
                    .DistinctBy(m => (m.Line, m.Column, m.Id))
                    .Take(5)
                    .ToList();
                results[group.Key] = new WriteResult(Outcome.CompileError, messages, Array.Empty<WriteRow>());
                pending.Remove(group.Key);
            }
        }

        if (report is not null) pending.Keys.Where(id => !report.Contains(id)).ToList().ForEach(id => pending.Remove(id));
        if (pending.Count > 0)
        {
            var run = assembly?.GetType("__Harness")!.GetMethod("Run", BindingFlags.Public | BindingFlags.Static);
            foreach (var id in pending.Keys)
            {
                results[id] = run is null
                    ? new WriteResult(Outcome.CompileError,
                        new[] { new CompileMessage(null, null, "", "This code could not be compiled.", null) },
                        Array.Empty<WriteRow>())
                    : RunTests(run, id);
            }
        }

        LastAssembly = assembly;
        LastElapsed = watch.Elapsed;
        return report is null ? results : results.Where(r => report.Contains(r.Key)).ToDictionary();
    }

    CSharpCompilation Compile(SyntaxTree tree) => CSharpCompilation.Create(
        course.Name.Replace(" ", "") + "Code" + (++compileCount), new[] { tree }, CSharpRunner.References.Value, CSharpRunner.CompileOptions);

    static Assembly? Emit(CSharpCompilation compilation)
    {
        using var stream = new MemoryStream();
        return compilation.Emit(stream).Success ? Assembly.Load(stream.ToArray()) : null;
    }

    static WriteResult RunTests(MethodInfo run, string id)
    {
        var rows = ((string[][])run.Invoke(null, new object[] { id })!)
            .Select(r => new WriteRow(r[0], r[1], r[2], r[3] == "1"))
            .ToList();
        var outcome = rows.All(r => r.Ok) ? Outcome.Pass : Outcome.Fail;
        return new WriteResult(outcome, Array.Empty<CompileMessage>(), rows);
    }

    // Problems that aren't being run get a stub body, so the tests always compile.
    // Each student body sits under a "#line 1" directive named after its problem, so
    // compiler errors report the problem and the line inside the student's code.
    string BuildSource(IReadOnlyDictionary<string, string> bodies)
    {
        var sb = new StringBuilder(course.Header);
        sb.Append(GuardSource);
        sb.Append("public static class Student\n{\n");
        foreach (var p in course.Problems)
        {
            sb.Append("    ").Append(p.Signature).Append("\n    {\n");
            if (bodies.TryGetValue(p.Id, out var body))
            {
                sb.Append("#line 1 \"").Append(LinePrefix).Append(p.Id).Append("\"\n");
                sb.Append(Normalize(body)).Append('\n');
                sb.Append("#line default\n");
            }
            else
            {
                sb.Append("        ").Append(Stub).Append('\n');
            }
            sb.Append("    }\n");
        }
        sb.Append("}\n");
        sb.Append(course.Harness);
        return sb.ToString();
    }

    static string? FindProblem(Diagnostic d)
    {
        if (d.Location == Location.None) return null;
        var mapped = d.Location.GetMappedLineSpan();
        return mapped.HasMappedPath && mapped.Path.StartsWith(LinePrefix, StringComparison.Ordinal)
            ? mapped.Path[LinePrefix.Length..]
            : null;
    }

    CompileMessage ToMessage(Diagnostic d)
    {
        var m = CSharpRunner.ToMessage(d);
        return m with { Hint = course.Hint(m.Id, m.Message) ?? ReturnHint(m.Id) ?? (m.Id == "CS0103" ? null : m.Hint) ?? GenericHint(m.Id) };
    }

    // These come before CSharpRunner's tips, which are written for the Conditions page.
    static string? ReturnHint(string id) => id switch
    {
        "CS0127" => "This method is void, so it doesn't return a value. Write return; on its own, or leave it out.",
        "CS0126" => "This method must return a value, as in return result;",
        "CS0161" => "Every way through your method must reach a return.",
        _ => null,
    };

    static string? GenericHint(string id) => id switch
    {
        "CS0103" => "C# cares about upper and lower case, and only knows names that exist in this method " +
                    "(like its parameters) or that you created with a variable.",
        _ => null,
    };

    // Adds __Guard.Loop() to the top of every loop body and __Guard.Call() to the top of
    // every student method, so a loop that never ends stops instead of freezing the page.
    // This runs only after the code compiled cleanly, so error positions are never affected.
    static SyntaxTree AddGuards(SyntaxTree tree)
    {
        var root = tree.GetRoot();
        var student = root.DescendantNodes().OfType<ClassDeclarationSyntax>().First(c => c.Identifier.Text == "Student");
        var guarded = new GuardRewriter().Visit(student);
        return tree.WithRootAndOptions(root.ReplaceNode(student, guarded), tree.Options);
    }

    sealed class GuardRewriter : CSharpSyntaxRewriter
    {
        static readonly StatementSyntax LoopTick = SyntaxFactory.ParseStatement("__Guard.Loop();");
        static readonly StatementSyntax CallTick = SyntaxFactory.ParseStatement("__Guard.Call();");

        static BlockSyntax Wrap(StatementSyntax body) => SyntaxFactory.Block(LoopTick, body);

        public override SyntaxNode? VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var visited = (MethodDeclarationSyntax)base.VisitMethodDeclaration(node)!;
            return visited.Body is null ? visited : visited.WithBody(visited.Body.WithStatements(visited.Body.Statements.Insert(0, CallTick)));
        }

        public override SyntaxNode? VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
        {
            var visited = (LocalFunctionStatementSyntax)base.VisitLocalFunctionStatement(node)!;
            return visited.Body is null ? visited : visited.WithBody(visited.Body.WithStatements(visited.Body.Statements.Insert(0, CallTick)));
        }

        public override SyntaxNode? VisitWhileStatement(WhileStatementSyntax node)
        {
            var visited = (WhileStatementSyntax)base.VisitWhileStatement(node)!;
            return visited.WithStatement(Wrap(visited.Statement));
        }

        public override SyntaxNode? VisitDoStatement(DoStatementSyntax node)
        {
            var visited = (DoStatementSyntax)base.VisitDoStatement(node)!;
            return visited.WithStatement(Wrap(visited.Statement));
        }

        public override SyntaxNode? VisitForStatement(ForStatementSyntax node)
        {
            var visited = (ForStatementSyntax)base.VisitForStatement(node)!;
            return visited.WithStatement(Wrap(visited.Statement));
        }

        public override SyntaxNode? VisitForEachStatement(ForEachStatementSyntax node)
        {
            var visited = (ForEachStatementSyntax)base.VisitForEachStatement(node)!;
            return visited.WithStatement(Wrap(visited.Statement));
        }
    }
}
