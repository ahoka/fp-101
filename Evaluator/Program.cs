using System;
using System.Collections.Generic;

namespace Evaluator
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = "(if nil (* (- 10 4) (+ 1 2)) 99)";

            var l = new Lisp();
            var p = l.Read(l.Tokenize(program));
            var r = l.Eval(p);

            Console.WriteLine(r);

            while (true)
            {
                try
                {
                    var input = Console.ReadLine();
                    Console.WriteLine(l.Eval(l.Read(l.Tokenize(input))));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                }
            }
        }
    }
}
