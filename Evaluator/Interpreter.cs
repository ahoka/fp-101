using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evaluator
{
    public class Lisp
    {
        public IList<string> Tokenize(string text)
        {
            return text.Replace("(", " ( ")
                .Replace(")", " ) ")
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

        public object Read(IList<string> tokens)
        {
            if (tokens.Count == 0)
            {
                throw new InvalidProgramException("Unexpected EOF");
            }

            var token = tokens.First();
            tokens.RemoveAt(0);
            if (token == "(")
            {
                var exp = new List<object>();
                while (tokens.First() != ")")
                {
                    exp.Add(Read(tokens));
                }

                tokens.RemoveAt(0);

                return exp;
            }
            else if (token == ")")
            {
                throw new InvalidProgramException("Unexpected ')'");
            }
            else
            {
                return Atom(token);
            }
        }

        private object Atom(string token)
        {
            if (decimal.TryParse(token, out var number))
            {
                return number;
            }
            else
            {
                return token;
            }
        }

        private IList<object> List(params object[] xs)
        {
            return xs.Skip(1).ToList();
        }

        Dictionary<string, object> GlobalEnv = new Dictionary<string, object>()
        {
            { "nil", new List<object>() },
            { "car", new Func<IList<object>, object>(x => x.First()) },
            { "cdr", new Func<IList<object>, object>(x => x.Skip(1)) },
            { "+", new Func<decimal, decimal, decimal>((x, y) => x + y ) },
            { "-", new Func<decimal, decimal, decimal>((x, y) => x - y ) },
            { "/", new Func<decimal, decimal, decimal>((x, y) => x / y ) },
            { "*", new Func<decimal, decimal, decimal>((x, y) => x * y ) }
        };

        public object Eval(object exp, Dictionary<string, object> env = null)
        {
            if (env == null)
            {
                env = GlobalEnv;
            }

            switch (exp)
            {
                case string x:
                    try
                    {
                        return env[x];
                    }
                    catch (Exception)
                    {
                        throw new InvalidProgramException($"Variable not set: {x}");
                    }
                case decimal x:
                    return x;
                case IList<object> x:
                    switch (x.First())
                    {
                        case "if":
                            var (_, cond, t, f) = x;
                            if (Eval(cond, env).IsTrue())
                            {
                                return Eval(t, env);
                            }
                            else
                            {
                                return Eval(f, env);
                            }
                        case "list":
                            return x.Skip(1).ToList();
                        case "define":
                            var (_, sym, expr) = x;
                            env[sym as string] = Eval(expr, env);
                            return null;
                        default:
                            var fun = Eval(x.First(), env) as Delegate;
                            if (fun == null)
                            {
                                throw new InvalidProgramException($"Not a function: {x.First()}");
                            }
                            return fun.DynamicInvoke(x.Skip(1).Select(o => Eval(o, env)).ToArray());
                    }
                default:
                    throw new InvalidProgramException($"At: {exp}");
            }
        }
    }
}
