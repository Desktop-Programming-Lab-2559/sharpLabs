using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace lab14
{
    public class Operation
    {
        private string _operationString;

        public string OperationString
        {
            get => _operationString;
        }

        public string OriginalString { get; set; }

        public delegate void TracingHandler(string text);

        public static event TracingHandler Tracing;
        
        public Operation()
        {
            
        }

        public Operation(string operationString)
        {
            OriginalString = operationString;
            ProcessOperationString();
        }

        public void ProcessOperationString()
        {
            var stack = new Stack<string>();
            var workString = new StringBuilder();
            for (int i = 0; i < OriginalString.Length; i++)
            {
                var symbol = OriginalString.Substring(i, 1);
                if (char.IsLetter(symbol[0]) || symbol == "0" || symbol == "1")
                {
                    Tracing?.Invoke($"Added symbol {symbol}");
                    workString.Append(symbol);
                }
                else
                {
                    if (symbol == "=" || symbol == "<")
                    {
                        symbol += OriginalString[i + 1];
                        i++;
                    }

                    if (!symbol.IsValidOperation())
                    {
                        throw new UnknownOperationException($"Unknown operation {symbol}");
                    }

                    if (stack.Count == 0)
                    {
                        stack.Push(symbol);
                    }
                    else if (symbol.Priority() >= stack.Peek().Priority())
                    {
                        stack.Push(symbol);
                    }
                    else
                    {
                        if (symbol == ")")
                        {
                            try
                            {
                                while (stack.Peek() != "(")
                                {
                                    Tracing?.Invoke($"Added symbol {stack.Peek()}");
                                    workString.Append(stack.Pop());
                                }

                                stack.Pop();
                                if (stack.Count != 0 && stack.Peek() == "!")
                                {
                                    Tracing?.Invoke($"Added symbol {stack.Peek()}");
                                    workString.Append(stack.Pop());
                                }
                            }
                            catch (InvalidOperationException e)
                            {
                                Console.WriteLine(e);
                                throw new WrongBracketsException($"Wrong brackets in {OriginalString}");
                            }
                        } else if (symbol.Priority() < stack.Peek().Priority())
                        {
                            while (stack.Count != 0 && symbol.Priority() < stack.Peek().Priority() && stack.Peek() != "(")
                            {
                                Tracing?.Invoke($"Added symbol {stack.Peek()}");
                                workString.Append(stack.Pop());
                            }
                            stack.Push(symbol);
                        }
                    }
                }
            }

            while (stack.Count != 0)
            {
                Tracing?.Invoke($"Added symbol {stack.Peek()}");
                if (stack.Peek() == "(") throw new WrongBracketsException($"Wrong brackets in {OriginalString}");
                workString.Append(stack.Pop());
            }

            _operationString = workString.ToString();
        }

        public string ToProcessedString()
        {
            return _operationString;
        }

        public override string ToString()
        {
            return OriginalString;
        }

        private char[] GetVariable()
        {
            return Regex.Replace(OriginalString, "[^a-z]", "", RegexOptions.IgnoreCase)// [^A-Za-z]
                .Distinct()
                .ToArray();
        }

        public int VariablesCount()
        {
            return GetVariable().Length;
        }
        
        public bool Calculate(params bool[] values)
        {
            var vCount = VariablesCount();
            if (vCount != values.Length) throw new ArgumentException("Wrong number of variables");
            if (_operationString == null) ProcessOperationString();

            var workString = _operationString;
            if (vCount != 0) workString = ReplaceWithValues(values);
            
            var result = new Stack<bool>();
            for (int i = 0; i < workString.Length; i++)
            {
                if (workString[i] == '0' || workString[i] == '1')
                {
                    result.Push(workString[i] == '1');
                }
                else
                {
                    var op = workString[i].ToString();
                    if (workString[i] == '=' || workString[i] == '<')
                    {
                        op += workString[i + 1];
                        i++;
                    }

                    if (op == "!")
                    {
                        try
                        {
                            Tracing?.Invoke($"Counting !{result.Peek()}");
                            result.Push(op.Execute(result.Peek(), result.Pop()));
                        }
                        catch (InvalidOperationException e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                    else
                        try
                        {
                            var v = result.Pop();
                            Tracing?.Invoke($"Counting {v} {op} {result.Peek()}");
                            result.Push(op.Execute(v, result.Pop()));
                        }
                        catch (InvalidOperationException e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                }
            }

            return result.Pop();
        }

        private string ReplaceWithValues(bool[] values) //         ????????????  IReadOnlyList<bool>
        {
            var s = Regex.Replace(_operationString.ToString(), "[^a-z]", "", RegexOptions.IgnoreCase);
            var uniqueChar = s.Distinct().ToArray();
            Array.Sort(uniqueChar);
            var valueOfVariables = new Dictionary<string, bool>();
            for (int i = 0; i < uniqueChar.Length; i++)
            {
                valueOfVariables.Add(uniqueChar[i].ToString(), values[i]);
            }

            var replacedString = new StringBuilder(_operationString.ToString());
            foreach (var pair in valueOfVariables)
            {
                replacedString.Replace(pair.Key, (pair.Value ? 1 : 0).ToString());
            }

            return replacedString.ToString();
        }

        public bool[][] CalculateTable()
        {
            var vCount = VariablesCount();
            var table = new bool[(int) Math.Pow(2, vCount)][];
            // var table = new bool[vCount + 1, (int)Math.Pow(2, vCount)];
            
            for (int i = 0; i < (int) Math.Pow(2, vCount); i++)
            {
                table[i] = new bool[vCount + 1];
                for (int j = 0; j < vCount ; j++)
                {
                    table[i][j] = (i >> (vCount - j -1) & 1) == 1;
                }
            }

            for (int i = 0; i < (int)Math.Pow(2, vCount); i++)
            {
                
                table[i][vCount] = Calculate(table[i].Take(vCount).ToArray());
            }
            
            return table;
        }

        public string PCNF()
        {
            Tracing?.Invoke($"Compute PCNF");
            var variables = GetVariable();
            if (variables.Length == 0)
            {
                return string.Empty;
            }
            var pcnf = new StringBuilder();
            foreach (var line in CalculateTable())
            {
                if (!line.Last())
                {
                    // Tracing?.Invoke($"");
                    var tmp = new StringBuilder("(");
                    for (int i = 0; i < line.Length - 1; i++)
                    {
                        tmp.Append(line[i] ? $"!{variables[i]}|" : $"{variables[i].ToString()}|");
                    }

                    tmp.Remove(tmp.Length - 1, 1);
                    tmp.Append(")&");
                    pcnf.Append(tmp);
                }
            }

            if (pcnf.Length > 0)
                pcnf.Remove(pcnf.Length - 1, 1);
            return pcnf.ToString();
        }

        public string PDNF()
        {
            Tracing?.Invoke($"Compute PDNF");
            var variables = GetVariable();
            if (variables.Length == 0)
            {
                return string.Empty;
            }
            var pdnf = new StringBuilder();
            foreach (var line in CalculateTable())
            {
                if (line.Last())
                {
                    var tmp = new StringBuilder("(");
                    for (int i = 0; i < line.Length - 1; i++)
                    {
                        tmp.Append(line[i] ? $"{variables[i].ToString()}&" : $"!{variables[i]}&");
                    }

                    tmp.Remove(tmp.Length - 1, 1);
                    tmp.Append(")|");
                    pdnf.Append(tmp);
                }
            }

            if (pdnf.Length > 0)
                pdnf.Remove(pdnf.Length - 1, 1);
            return pdnf.ToString();
        }
    }

    internal static class StringExtension
    {
        private static readonly Dictionary<string, int> OperationPriority = new Dictionary<string, int>
        {
            {")", 0},
            {"~", 1}, // эквивалентность
            {"=>", 2}, // импликация
            {"<=", 2}, // коимпликация
            {"+", 3}, // сложение по модулю 2
            {"|", 3}, // или
            {"&", 4}, // и
            {"!", 5}, // не
            {"(", 7}
        };

        
        private static readonly Dictionary<string, Func<bool, bool, bool>> OperationDelegate =
            new Dictionary<string, Func<bool, bool, bool>>
        {
            {"~", (l, r) => l == r}, // эквивалентность
            {"=>", (l, r) => !l | r}, // импликация
            {"<=", (l, r) => l & !r}, // коимпликация
            {"+", (l, r) => l ^ r}, // сложение по модулю 2
            {"|", (l, r) => l | r}, // или
            {"&", (l, r) => l & r}, // и
            {"!", (l, r) => !l}, // не
        };
        public static int Priority(this string s)
        {
            if (!s.IsValidOperation()) throw new UnknownOperationException($"Unknown operation {s}");
            return OperationPriority[s];
        }

        public static bool IsValidOperation(this string s)
        {
            return OperationPriority.ContainsKey(s);
        }

        public static bool Execute(this string s, bool right, bool left)
        {
            return OperationDelegate[s](left, right);
        }
    }

    [Serializable]
    public class WrongBracketsException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public WrongBracketsException()
        {
        }

        public WrongBracketsException(string message) : base(message)
        {
        }

        public WrongBracketsException(string message, Exception inner) : base(message, inner)
        {
        }

        protected WrongBracketsException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class UnknownOperationException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public UnknownOperationException()
        {
        }

        public UnknownOperationException(string message) : base(message)
        {
        }

        public UnknownOperationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected UnknownOperationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}