﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Concepts;
using System.Concepts.Enumerable;
using static System.Concepts.Enumerable.Instances;
using System.Concepts.Showable;
using TinyLinq.SpecialisedInstances;

namespace TinyLinq
{   
    public static class DesugaredTests
    {
        public static void Run()
        {
            int[] sample = { 1, 0, 0, 9, 7, 3, 2, 5, 3, 3, 7, 6, 5, 2, 0, 1, 3, 5, 8, 6, 3, 4, 6, 7, 3, 5, 4, 8, 7, 6, 8, 0, 9, 5, 9, 0, 9, 1, 1, 7, 3, 9, 2, 9, 2, 7, 4, 9, 4, 5, 3, 7, 5, 4, 2, 0, 4, 8, 0, 5, 6, 4, 8, 9, 4, 7, 4, 2, 9, 6, 2, 4, 8, 0, 5, 2, 4, 0, 3, 7, 2, 0, 6, 3, 6, 1, 0, 4, 0, 2, 0, 0, 8, 2, 2, 9, 1, 6, 6 };

            var f = sample.CSelect((int x) => x + 5);
            while (CEnumerator<ArraySelect<int, int>, int>.MoveNext(ref f))
            {
                Console.WriteLine(CEnumerator<ArraySelect<int, int>, int>.Current(ref f));
            }

            Console.WriteLine("oOo");

            var f2 = sample.CSelect((int x) => x + 5).CSelect((int y) => y * 10);
            while (CEnumerator<ArraySelect<int, int>, int>.MoveNext(ref f2))
            {
                Console.WriteLine(CEnumerator<ArraySelect<int, int>, int>.Current(ref f2));
            }

            Console.WriteLine("oOo");

            var selsel = sample.CSelect((int x) => x * 10).CSelect((int y) => y + 5);
            Console.WriteLine(Helpers.String(selsel.CToArray()));

            var goo = sample.CWhere((int x) => x % 3 == 0);
            var bar = goo.CSelect((int y) => y * 6);
            Console.WriteLine(Helpers.String(bar.CToArray()));

            var ary = sample.CSelect((int x) => x + 5).CWhere((int z) => z % 3 == 0);
            Console.WriteLine(Helpers.String(ary.CToArray()));

            Console.WriteLine("oOo");

            var goop = new List<int>(sample).CWhere((int x) => x % 3 == 0);

            var f3 = new List<int>(sample).CSelect((int x) => x + 5).CSelect((int y) => y * 10);
            while (CEnumerator<Select<List<int>.Enumerator, int, int>>.MoveNext(ref f3))
            {
                Console.WriteLine(CEnumerator<Select<List<int>.Enumerator, int, int>>.Current(ref f3));
            }

            Console.WriteLine("oOo");


            var g = ((IEnumerable<int>)sample).Select(x => x + 5).GetEnumerator();
            while (g.MoveNext())
            {
                Console.WriteLine(g.Current);
            }
            var g2 = ((IEnumerable<int>)sample).Select(x => x + 5).Select(y => y * 10).GetEnumerator();
            while (g2.MoveNext())
            {
                Console.WriteLine(g2.Current);
            }
        }
    }
}
