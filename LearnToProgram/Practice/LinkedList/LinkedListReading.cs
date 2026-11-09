namespace LearnToProgram.Practice.LinkedList;

// Questions for the Linked List 1 page: students read a short piece of code and choose or type
// what it prints or what the list looks like afterwards. No code is written. See Practice/Reading.cs.
//
// In Run, p(...) stands for Console.WriteLine(...), and Show(list) prints a list like [2 -> 1 -> 5].

public static class LinkedListReading
{
    // The same IntNode students use on the Linked List 2 page, so Run can be written like the code they read.
    public sealed class IntNode
    {
        private int value;
        private IntNode? next;

        public IntNode(int value) { this.value = value; }
        public IntNode(int value, IntNode? next) { this.value = value; this.next = next; }

        public int GetValue() => value;
        public IntNode? GetNext() => next;
        public void SetValue(int value) => this.value = value;
        public void SetNext(IntNode? next) => this.next = next;
        public bool HasNext() => next != null;
    }

    public const string Crash = "Error: the program crashes";

    public static readonly ReadLevel[] Levels =
    {
        new(1, "Building a list",
            "<code>new IntNode(5, other)</code> makes a node holding 5 that points to <code>other</code>. " +
            "Lists are shown like <code>[2 -&gt; 1 -&gt; 5]</code>: the first node is on the left.",
            "הרשימה מוצגת משמאל לימין: החוליה הראשונה בצד שמאל."),
        new(2, "Walking through a list",
            "A loop moves <code>p</code> from node to node with <code>p = p.GetNext()</code>. " +
            "Read the loop condition carefully: <code>p != null</code> and <code>p.HasNext()</code> are not the same.",
            "שימו לב לתנאי הלולאה: p != null ו־p.HasNext() הם לא אותו דבר."),
        new(3, "Changing a list",
            "<code>SetValue</code> changes the number in a node. <code>SetNext</code> changes which node comes next, " +
            "so it can move nodes around."),
        new(4, "Removing and inserting",
            "To remove a node, point the node before it to the node after it. " +
            "To insert a node, point the new node to the rest of the list first, then point the node before it to the new node.",
            "כדי להוציא חוליה, מחברים את החוליה שלפניה לחוליה שאחריה."),
    };

    static string[] L(params string[] lines) => lines;
    static string[] L(params int[] lines) => lines.Select(n => n.ToString()).ToArray();

    static IntNode? Make(params int[] values)
    {
        IntNode? list = null;
        for (int i = values.Length - 1; i >= 0; i--) list = new IntNode(values[i], list);
        return list;
    }

    public static string Show(IntNode? list)
    {
        var parts = new List<string>();
        for (var p = list; p != null && parts.Count < 100; p = p.GetNext()) parts.Add(p.GetValue().ToString());
        return "[" + string.Join(" -> ", parts) + "]";
    }

    static string[] LS(params int[] values) => L(Show(Make(values)));

    // Runs the code and collects what it prints. A null node error counts as a crash.
    static Func<string[]> Prints(Action<Action<object>> body) => () =>
    {
        var lines = new List<string>();
        try { body(x => lines.Add(x.ToString()!)); }
        catch (NullReferenceException) { lines.Add(Crash); }
        return lines.ToArray();
    };

    const string WhatList = "What does list look like after this code?";
    const string WhatListHe = "איך תיראה הרשימה אחרי הקוד?";
    const string WhatPrints = "What does this code print?";
    const string WhatPrintsHe = "מה הקוד ידפיס?";
    const string TypeListHint = "Type the list, like 1 -> 2 -> 3.";

    public static readonly ReadQuestion[] All =
    {
        // ---------- Level 1: building a list ----------
        new(1, 1, ReadKind.Choose, WhatList, WhatListHe,
            """
            IntNode list = new IntNode(2, new IntNode(1, new IntNode(5)));
            """,
            new[] { LS(2, 1, 5), LS(5, 1, 2), LS(2, 1), LS(1, 5) },
            Prints(p =>
            {
                IntNode list = new IntNode(2, new IntNode(1, new IntNode(5)));
                p(Show(list));
            })),

        new(2, 1, ReadKind.Choose, WhatList, WhatListHe,
            """
            IntNode a = new IntNode(4);
            IntNode b = new IntNode(9);
            IntNode c = new IntNode(6);
            b.SetNext(a);
            a.SetNext(c);
            IntNode list = b;
            """,
            new[] { LS(4, 9, 6), LS(9, 4, 6), LS(4, 6, 9), LS(9, 6, 4) },
            Prints(p =>
            {
                IntNode a = new IntNode(4);
                IntNode b = new IntNode(9);
                IntNode c = new IntNode(6);
                b.SetNext(a);
                a.SetNext(c);
                IntNode list = b;
                p(Show(list));
            })),

        new(3, 1, ReadKind.Choose, WhatList, WhatListHe,
            """
            IntNode list = null;
            for (int i = 1; i <= 4; i++)
            {
                list = new IntNode(i, list);
            }
            """,
            new[] { LS(1, 2, 3, 4), LS(4, 3, 2, 1), LS(4), LS(1) },
            Prints(p =>
            {
                IntNode? list = null;
                for (int i = 1; i <= 4; i++)
                {
                    list = new IntNode(i, list);
                }
                p(Show(list));
            })),

        new(4, 1, ReadKind.TypeNumber, WhatPrints, WhatPrintsHe,
            """
            IntNode list = new IntNode(8, new IntNode(3, new IntNode(7, new IntNode(1))));
            Console.WriteLine(list.GetNext().GetNext().GetValue());
            """,
            Array.Empty<string[]>(),
            Prints(p =>
            {
                IntNode list = new IntNode(8, new IntNode(3, new IntNode(7, new IntNode(1))));
                p(list.GetNext()!.GetNext()!.GetValue());
            })),

        // ---------- Level 2: walking through a list ----------
        new(5, 2, ReadKind.TypeNumber, WhatPrints, WhatPrintsHe,
            """
            // list is [20 -> 5 -> 13 -> 14 -> 30]
            int count = 0;
            IntNode p = list;
            while (p != null)
            {
                if (p.GetValue() > 13)
                {
                    count++;
                }
                p = p.GetNext();
            }
            Console.WriteLine(count);
            """,
            Array.Empty<string[]>(),
            Prints(print =>
            {
                var list = Make(20, 5, 13, 14, 30);
                int count = 0;
                var p = list;
                while (p != null)
                {
                    if (p.GetValue() > 13) count++;
                    p = p.GetNext();
                }
                print(count);
            })),

        new(6, 2, ReadKind.Choose, WhatPrints, WhatPrintsHe,
            """
            // list is [4 -> 7 -> 2 -> 5]
            int sum = 0;
            IntNode p = list;
            while (p.HasNext())
            {
                sum = sum + p.GetValue();
                p = p.GetNext();
            }
            Console.WriteLine(sum);
            """,
            new[] { L(18), L(13), L(14), L(11) },
            Prints(print =>
            {
                var list = Make(4, 7, 2, 5)!;
                int sum = 0;
                var p = list;
                while (p.HasNext())
                {
                    sum = sum + p.GetValue();
                    p = p.GetNext()!;
                }
                print(sum);
            })),

        new(7, 2, ReadKind.Choose, WhatPrints, WhatPrintsHe,
            """
            // list is [3 -> 8 -> 1]
            IntNode p = list;
            while (p.HasNext())
            {
                p = p.GetNext();
                Console.WriteLine(p.GetValue());
            }
            """,
            new[] { L(3, 8, 1), L(8, 1), L(3, 8), L(8) },
            Prints(print =>
            {
                var p = Make(3, 8, 1)!;
                while (p.HasNext())
                {
                    p = p.GetNext()!;
                    print(p.GetValue());
                }
            })),

        new(8, 2, ReadKind.TypeNumber, WhatPrints, WhatPrintsHe,
            """
            // list is [6 -> -2 -> 11 -> 4]
            int x = list.GetValue();
            IntNode p = list.GetNext();
            while (p != null)
            {
                if (p.GetValue() > x)
                {
                    x = p.GetValue();
                }
                p = p.GetNext();
            }
            Console.WriteLine(x);
            """,
            Array.Empty<string[]>(),
            Prints(print =>
            {
                var list = Make(6, -2, 11, 4)!;
                int x = list.GetValue();
                var p = list.GetNext();
                while (p != null)
                {
                    if (p.GetValue() > x) x = p.GetValue();
                    p = p.GetNext();
                }
                print(x);
            })),

        new(9, 2, ReadKind.TypeNumber, WhatPrints, WhatPrintsHe,
            """
            // list is [5 -> 9 -> 3 -> 3 -> 8 -> 2]
            int count = 0;
            IntNode p = list;
            while (p.HasNext())
            {
                if (p.GetValue() > p.GetNext().GetValue())
                {
                    count++;
                }
                p = p.GetNext();
            }
            Console.WriteLine(count);
            """,
            Array.Empty<string[]>(),
            Prints(print =>
            {
                var p = Make(5, 9, 3, 3, 8, 2)!;
                int count = 0;
                while (p.HasNext())
                {
                    if (p.GetValue() > p.GetNext()!.GetValue()) count++;
                    p = p.GetNext()!;
                }
                print(count);
            })),

        new(10, 2, ReadKind.Choose, "The list is empty. What happens?", "הרשימה ריקה. מה יקרה?",
            """
            IntNode list = null;
            int count = 0;
            while (list.HasNext())
            {
                count++;
                list = list.GetNext();
            }
            Console.WriteLine(count);
            """,
            new[] { L(0), L(1), L(Crash) },
            Prints(print =>
            {
                IntNode? list = null;
                int count = 0;
                while (list!.HasNext())
                {
                    count++;
                    list = list.GetNext();
                }
                print(count);
            })),

        // ---------- Level 3: changing a list ----------
        new(11, 3, ReadKind.TypeList, WhatList + " " + TypeListHint, WhatListHe,
            """
            // list is [1 -> 2 -> 3 -> 4]
            IntNode p = list;
            while (p.HasNext())
            {
                p.SetValue(p.GetValue() + p.GetNext().GetValue());
                p = p.GetNext();
            }
            """,
            Array.Empty<string[]>(),
            Prints(print =>
            {
                var list = Make(1, 2, 3, 4)!;
                var p = list;
                while (p.HasNext())
                {
                    p.SetValue(p.GetValue() + p.GetNext()!.GetValue());
                    p = p.GetNext()!;
                }
                print(Show(list));
            })),

        new(12, 3, ReadKind.Choose, WhatList, WhatListHe,
            """
            // list is [7 -> 8 -> 9]
            IntNode second = list.GetNext();
            list.SetNext(second.GetNext());
            second.SetNext(list);
            list = second;
            """,
            new[] { LS(8, 7, 9), LS(7, 9, 8), LS(8, 9, 7), LS(8, 7) },
            Prints(print =>
            {
                var list = Make(7, 8, 9)!;
                var second = list.GetNext()!;
                list.SetNext(second.GetNext());
                second.SetNext(list);
                list = second;
                print(Show(list));
            })),

        new(13, 3, ReadKind.Choose, WhatList, WhatListHe,
            """
            // list is [1 -> 2 -> 3 -> 4]
            IntNode first = list;
            list = list.GetNext();
            IntNode p = list;
            while (p.HasNext())
            {
                p = p.GetNext();
            }
            p.SetNext(first);
            first.SetNext(null);
            """,
            new[] { LS(2, 3, 4, 1), LS(4, 1, 2, 3), LS(2, 3, 4), LS(1, 2, 3, 4) },
            Prints(print =>
            {
                var list = Make(1, 2, 3, 4)!;
                var first = list;
                list = list.GetNext()!;
                var p = list;
                while (p.HasNext())
                {
                    p = p.GetNext()!;
                }
                p.SetNext(first);
                first.SetNext(null);
                print(Show(list));
            })),

        // ---------- Level 4: removing and inserting ----------
        new(14, 4, ReadKind.Choose, WhatList, WhatListHe,
            """
            // list is [5 -> 6 -> 7 -> 8]
            list.SetNext(list.GetNext().GetNext());
            """,
            new[] { LS(5, 7, 8), LS(5, 6, 8), LS(6, 7, 8), LS(5, 8) },
            Prints(print =>
            {
                var list = Make(5, 6, 7, 8)!;
                list.SetNext(list.GetNext()!.GetNext());
                print(Show(list));
            })),

        new(15, 4, ReadKind.Choose, WhatList, WhatListHe,
            """
            // list is [3 -> 9 -> 4 -> 9 -> 2]
            IntNode p = list;
            while (p.HasNext())
            {
                if (p.GetNext().GetValue() == 9)
                {
                    p.SetNext(p.GetNext().GetNext());
                }
                else
                {
                    p = p.GetNext();
                }
            }
            """,
            new[] { LS(3, 4, 2), LS(3, 4, 9, 2), LS(3, 9, 4, 2), LS(9, 9) },
            Prints(print =>
            {
                var list = Make(3, 9, 4, 9, 2)!;
                var p = list;
                while (p.HasNext())
                {
                    if (p.GetNext()!.GetValue() == 9) p.SetNext(p.GetNext()!.GetNext());
                    else p = p.GetNext()!;
                }
                print(Show(list));
            })),

        new(16, 4, ReadKind.Choose, WhatList, WhatListHe,
            """
            // list is [1 -> 3 -> 4]
            IntNode node = new IntNode(2);
            node.SetNext(list.GetNext());
            list.SetNext(node);
            """,
            new[] { LS(1, 2, 3, 4), LS(2, 1, 3, 4), LS(1, 3, 2, 4), LS(1, 2) },
            Prints(print =>
            {
                var list = Make(1, 3, 4)!;
                var node = new IntNode(2);
                node.SetNext(list.GetNext());
                list.SetNext(node);
                print(Show(list));
            })),

        new(17, 4, ReadKind.TypeList, WhatList + " " + TypeListHint, WhatListHe,
            """
            // list is [2 -> 5 -> 8 -> 11]
            IntNode p = list;
            while (p.HasNext() && p.GetNext().GetValue() < 9)
            {
                p = p.GetNext();
            }
            p.SetNext(new IntNode(9, p.GetNext()));
            """,
            Array.Empty<string[]>(),
            Prints(print =>
            {
                var list = Make(2, 5, 8, 11)!;
                var p = list;
                while (p.HasNext() && p.GetNext()!.GetValue() < 9)
                {
                    p = p.GetNext()!;
                }
                p.SetNext(new IntNode(9, p.GetNext()));
                print(Show(list));
            })),
    };

    public static IEnumerable<string> Validate() => Reading.Validate(All);
}
