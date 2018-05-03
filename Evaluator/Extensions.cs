using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Evaluator
{
    public static class Extensions
    {
        public static bool IsTrue(this object o)
        {
            switch (o)
            {
                case IEnumerable e:
                    return e.Cast<object>().Any();
                default:
                    return true;
            }
        }

        public static void Deconstruct<T>(this IList<T> list, out T a)
        {
            a = list[0];
        }

        public static void Deconstruct<T>(this IList<T> list, out T a, out T b)
        {
            a = list[0];
            b = list[1];
        }

        public static void Deconstruct<T>(this IList<T> list, out T a, out T b, out T c)
        {
            a = list[0];
            b = list[1];
            c = list[2];
        }

        public static void Deconstruct<T>(this IList<T> list, out T a, out T b, out T c, out T d)
        {
            a = list[0];
            b = list[1];
            c = list[2];
            d = list[3];
        }
    }
}
