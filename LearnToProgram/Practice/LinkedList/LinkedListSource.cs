namespace LearnToProgram.Practice.LinkedList;

// C# source that is compiled in the browser together with the students' methods.
// It is plain text here, not part of this app: the IntNode class students use,
// a guard that stops endless loops, and the tests from the IntNode worksheet.
static class LinkedListSource
{
    public const string Header = """
        using System;
        using System.Collections.Generic;

        public class IntNode
        {
            private int value;
            private IntNode next;

            public IntNode(int value)
            {
                this.value = value;
                this.next = null;
            }

            public IntNode(int value, IntNode next)
            {
                this.value = value;
                this.next = next;
            }

            public int GetValue() { return this.value; }
            public IntNode GetNext() { return this.next; }
            public void SetValue(int value) { this.value = value; }
            public void SetNext(IntNode next) { this.next = next; }
            public bool HasNext() { return this.next != null; }
            public override string ToString() { return this.value.ToString(); }
        }

        // Calls to these are added to every loop and method in student code, so a loop that
        // never ends stops with a message instead of freezing the browser.
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

    public const string Harness = """

        public static class __Harness
        {
            static List<string[]> rows;

            public static string[][] Run(string id)
            {
                rows = new List<string[]>();
                int[] empty = new int[0];
                switch (id)
                {
                    case "1":
                        TestCount(new[] { 3, 5, 8, 2, 7 }, 5);
                        TestCount(new[] { 4 }, 1);
                        TestCount(new[] { 12, 10, 8, 6, 4, 2 }, 6);
                        TestCount(empty, 0);
                        break;
                    case "2":
                        TestSumEven(new[] { 3, 5, 8, 2, 7 }, 10);
                        TestSumEven(new[] { 4 }, 4);
                        TestSumEven(new[] { 1, 3, 5 }, 0);
                        TestSumEven(new[] { 12, 10, 8, 6, 4, 2 }, 42);
                        TestSumEven(new[] { -4, 7, 0, -2, -3 }, -6);
                        TestSumEven(empty, 0);
                        break;
                    case "3":
                        TestHasValue(new[] { 3, 5, 8, 2, 7 }, 2, true);
                        TestHasValue(new[] { 3, 5, 8, 2, 7 }, 9, false);
                        TestHasValue(new[] { 4 }, 4, true);
                        TestHasValue(new[] { 1, 3, 5 }, 1, true);
                        TestHasValue(new[] { 1, 3, 5 }, 5, true);
                        TestHasValue(new[] { -4, 7, 0, -2, -3 }, 0, true);
                        TestHasValue(empty, 3, false);
                        break;
                    case "4":
                        TestMax(new[] { 3, 5, 8, 2, 7 }, 8);
                        TestMax(new[] { 4 }, 4);
                        TestMax(new[] { 1, 3, 5 }, 5);
                        TestMax(new[] { 12, 10, 8, 6, 4, 2 }, 12);
                        TestMax(new[] { -5, -1, -9 }, -1);
                        break;
                    case "5":
                        TestIncrement(new[] { 3, 5, 8 }, new[] { 4, 6, 9 });
                        TestIncrement(new[] { 4 }, new[] { 5 });
                        TestIncrement(new[] { -1, 0, -7 }, new[] { 0, 1, -6 });
                        break;
                    case "6":
                        TestAddLast(new[] { 3, 5, 8 }, 1, new[] { 3, 5, 8, 1 });
                        TestAddLast(new[] { 4 }, 9, new[] { 4, 9 });
                        TestAddLast(new[] { 2, 2 }, 2, new[] { 2, 2, 2 });
                        break;
                    case "7":
                        TestRemoveFirst(new[] { 3, 5, 8, 2 }, 8, new[] { 3, 5, 2 });
                        TestRemoveFirst(new[] { 3, 5, 8, 2 }, 3, new[] { 5, 8, 2 });
                        TestRemoveFirst(new[] { 3, 5, 8, 2 }, 2, new[] { 3, 5, 8 });
                        TestRemoveFirst(new[] { 3, 5, 3, 5 }, 5, new[] { 3, 3, 5 });
                        TestRemoveFirst(new[] { 3, 5, 8, 2 }, 9, new[] { 3, 5, 8, 2 });
                        TestRemoveFirst(new[] { 4 }, 4, empty);
                        TestRemoveFirst(empty, 4, empty);
                        break;
                    case "8":
                        TestRemoveEvens(new[] { 3, 5, 8, 2, 7 }, new[] { 3, 5, 7 });
                        TestRemoveEvens(new[] { 2, 4, 1, 6 }, new[] { 1 });
                        TestRemoveEvens(new[] { 1, 3, 5 }, new[] { 1, 3, 5 });
                        TestRemoveEvens(new[] { 12, 10, 8 }, empty);
                        TestRemoveEvens(new[] { -4, 7, 0, -2, -3 }, new[] { 7, -3 });
                        TestRemoveEvens(empty, empty);
                        break;
                    case "9":
                        TestIsSorted(new[] { 1, 2, 3 }, true);
                        TestIsSorted(new[] { -3, 0, 4, 10 }, true);
                        TestIsSorted(new[] { 1, 3, 2 }, false);
                        TestIsSorted(new[] { 2, 1 }, false);
                        TestIsSorted(new[] { 1, 2, 2 }, false);
                        TestIsSorted(new[] { 5 }, true);
                        TestIsSorted(empty, true);
                        break;
                    case "10":
                        TestMerge(new[] { 1, 3, 5 }, new[] { 2, 4, 6 }, new[] { 1, 2, 3, 4, 5, 6 });
                        TestMerge(new[] { 5, 6 }, new[] { 1, 2 }, new[] { 1, 2, 5, 6 });
                        TestMerge(new[] { 1, 1, 4 }, new[] { 1, 3 }, new[] { 1, 1, 1, 3, 4 });
                        TestMerge(empty, new[] { 1, 2 }, new[] { 1, 2 });
                        TestMerge(new[] { 1, 2 }, empty, new[] { 1, 2 });
                        TestMerge(empty, empty, empty);
                        break;
                    case "11a":
                        TestReverseCopy(new[] { 1, 2, 3 }, new[] { 3, 2, 1 });
                        TestReverseCopy(new[] { 1, 2 }, new[] { 2, 1 });
                        TestReverseCopy(new[] { 7 }, new[] { 7 });
                        TestReverseCopy(empty, empty);
                        break;
                    case "11b":
                        TestReverse(new[] { 1, 2, 3 }, new[] { 3, 2, 1 });
                        TestReverse(new[] { 1, 2 }, new[] { 2, 1 });
                        TestReverse(new[] { 7 }, new[] { 7 });
                        TestReverse(empty, empty);
                        break;
                    case "12":
                        TestLongestRun(new[] { 1, 2, 2, 2, 3, 3 }, 3);
                        TestLongestRun(new[] { 1, 2, 3 }, 1);
                        TestLongestRun(new[] { 5, 5, 5 }, 3);
                        TestLongestRun(new[] { 4, 4, 1, 2 }, 2);
                        TestLongestRun(new[] { 1, 2, 2 }, 2);
                        TestLongestRun(new[] { 3, 1, 3 }, 1);
                        TestLongestRun(new[] { 7 }, 1);
                        TestLongestRun(empty, 0);
                        break;
                }
                return rows.ToArray();
            }

            static IntNode L(params int[] values)
            {
                IntNode lst = null;
                for (int i = values.Length - 1; i >= 0; i--)
                    lst = new IntNode(values[i], lst);
                return lst;
            }

            static string Show(IntNode lst)
            {
                string s = "[";
                int count = 0;
                while (lst != null)
                {
                    if (count == 20)
                        return s + "... never ends]";
                    s += lst.GetValue();
                    if (lst.HasNext()) s += " -> ";
                    lst = lst.GetNext();
                    count++;
                }
                return s + "]";
            }

            static string Text(object x)
            {
                if (x == null) return "[]";
                if (x is IntNode) return Show((IntNode)x);
                if (x is int[]) return Show(L((int[])x));
                if (x is bool) return (bool)x ? "true" : "false";
                return x.ToString();
            }

            // Each row: the call, what the student's code gave, the expected answer, "1" if they match.
            static void Check(string call, Func<object> run, object expected)
            {
                string exp = Text(expected);
                string result;
                bool ok;
                __Guard.Reset();
                try
                {
                    result = Text(run());
                    ok = result == exp;
                }
                catch (__LoopNeverEnds)
                {
                    ok = false;
                    result = "never ends: a loop went around " + __Guard.MaxLoops.ToString("N0") + " times";
                }
                catch (__TooManyCalls)
                {
                    ok = false;
                    result = "never ends: a method called itself " + __Guard.MaxCalls + " times";
                }
                catch (Exception e)
                {
                    ok = false;
                    result = "CRASHED: " + e.GetType().Name;
                }
                rows.Add(new[] { call, result, exp, ok ? "1" : "0" });
            }

            static void TestCount(int[] v, int expected)
            {
                Check("CountList(" + Text(v) + ")", () => Student.CountList(L(v)), expected);
            }

            static void TestSumEven(int[] v, int expected)
            {
                Check("SumEvenElements(" + Text(v) + ")", () => Student.SumEvenElements(L(v)), expected);
            }

            static void TestHasValue(int[] v, int x, bool expected)
            {
                Check("HasValue(" + Text(v) + ", " + x + ")", () => Student.HasValue(L(v), x), expected);
            }

            static void TestMax(int[] v, int expected)
            {
                Check("FindMaximum(" + Text(v) + ")", () => Student.FindMaximum(L(v)), expected);
            }

            static void TestIncrement(int[] v, int[] expected)
            {
                Check("IncrementAll(" + Text(v) + ")", () =>
                {
                    IntNode lst = L(v);
                    Student.IncrementAll(lst);
                    return lst;
                }, expected);
            }

            static void TestAddLast(int[] v, int val, int[] expected)
            {
                Check("AddLast(" + Text(v) + ", " + val + ")", () =>
                {
                    IntNode lst = L(v);
                    Student.AddLast(lst, val);
                    return lst;
                }, expected);
            }

            static void TestRemoveFirst(int[] v, int x, int[] expected)
            {
                Check("RemoveFirst(" + Text(v) + ", " + x + ")", () => Student.RemoveFirst(L(v), x), expected);
            }

            static void TestRemoveEvens(int[] v, int[] expected)
            {
                Check("RemoveEvens(" + Text(v) + ")", () => Student.RemoveEvens(L(v)), expected);
            }

            static void TestIsSorted(int[] v, bool expected)
            {
                Check("IsSorted(" + Text(v) + ")", () => Student.IsSorted(L(v)), expected);
            }

            static void TestMerge(int[] a, int[] b, int[] expected)
            {
                Check("Merge(" + Text(a) + ", " + Text(b) + ")", () =>
                {
                    IntNode chain1 = L(a);
                    IntNode chain2 = L(b);
                    IntNode result = Student.Merge(chain1, chain2);
                    if (Show(chain1) != Text(a)) return "changed chain1 to " + Show(chain1);
                    if (Show(chain2) != Text(b)) return "changed chain2 to " + Show(chain2);
                    return result;
                }, expected);
            }

            static void TestReverseCopy(int[] v, int[] expected)
            {
                Check("ReverseCopy(" + Text(v) + ")", () =>
                {
                    IntNode lst = L(v);
                    IntNode result = Student.ReverseCopy(lst);
                    if (Show(lst) != Text(v)) return "changed the original list to " + Show(lst);
                    return result;
                }, expected);
            }

            static void TestReverse(int[] v, int[] expected)
            {
                Check("Reverse(" + Text(v) + ")", () =>
                {
                    IntNode lst = L(v);
                    List<IntNode> nodes = new List<IntNode>();
                    for (IntNode p = lst; p != null; p = p.GetNext())
                        nodes.Add(p);
                    IntNode result = Student.Reverse(lst);
                    if (Show(result) != Text(expected)) return result;
                    IntNode q = result;
                    for (int i = nodes.Count - 1; i >= 0; i--)
                    {
                        if (q != nodes[i]) return Show(result) + " but made new nodes";
                        q = q.GetNext();
                    }
                    return result;
                }, expected);
            }

            static void TestLongestRun(int[] v, int expected)
            {
                Check("LongestRun(" + Text(v) + ")", () => Student.LongestRun(L(v)), expected);
            }
        }
        """;
}
