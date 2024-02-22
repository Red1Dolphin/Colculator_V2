using System;
using System.Collections;
using System.Globalization;

namespace Colculator_V2
{
    internal class Program
    {
        /// <summary>
        /// Метод розбиває рядок (string) на токени 
        /// </summary>
        /// <param name="Line"></param>
        /// <returns></returns>
        static ArrayList Tok(string Line)
        {
            var tokens = new ArrayList();
            string Space = "";
            foreach (char c in Line)
            {
                if (char.IsDigit(c) || c == '.')
                {
                    Space += c;
                }
                else if (c == ' ')
                {
                    if (!string.IsNullOrEmpty(Space))
                    {
                        tokens.Add(double.Parse(Space, CultureInfo.InvariantCulture));
                        Space = "";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Space))
                    {
                        tokens.Add(double.Parse(Space, CultureInfo.InvariantCulture));
                        Space = "";
                    }
                    tokens.Add(c.ToString());
                }

            }
            if (!string.IsNullOrEmpty(Space))
            {
                tokens.Add(double.Parse(Space, CultureInfo.InvariantCulture));
            }

            return tokens;
        }
        /// <summary>
        /// Визначає приорітетність виконання дії операндів
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        static int Priority(string A)
        {
            switch (A)
            {
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
                case "^":
                    return 3;
                default: return 0;
            }
        }
        /// <summary>
        /// Метод ковертує токени для польської нотаціїш
        /// </summary>
        /// <param name="Tokens"></param>
        /// <returns></returns>
        static ArrayList Postfix(ArrayList Tokens)
        {
            ArrayList Tokens2 = new ArrayList();
            var stack = new Stack();
            foreach (var t in Tokens)
            {
                if (t is double)
                {
                    Tokens2.Add(t);
                }
                else if (t.ToString() == "(")
                {
                    stack.Push(t.ToString());
                }
                else if (t.ToString() == ")")
                {
                    while (stack.Count > 0 && stack.Peek().ToString() != "(")
                    {
                        Tokens2.Add(stack.Pop());
                    }
                    stack.Pop();
                }
                else
                {
                    while (stack.Count > 0 && Priority(stack.Peek().ToString()) >= Priority(t.ToString()))
                    {
                        Tokens2.Add(stack.Pop());
                    }
                    stack.Push(t.ToString());
                }
            }
            while (stack.Count > 0)
            {
                Tokens2.Add(stack.Pop());
            }
            return Tokens2;
        }
        /// <summary>
        /// Метод проробляє математичні обчислення взалежності від операнда.
        /// </summary>
        /// <param name="Numeric1"></param>
        /// <param name="Numeric2"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        static double Math(double Numeric1, double Numeric2, string op)
        {
            switch (op)
            {
                case "+":
                    return Numeric1 + Numeric2;

                case "-":
                    return Numeric1 - Numeric2;

                case "*":
                    return Numeric1 * Numeric2;

                case "/":
                    return Numeric1 / Numeric2;
                case "^":
                    if (Numeric2 < 0)
                    {
                        if (Numeric1 == 0)
                        {
                            throw new ArgumentException("0 не може бути зведений до від'ємного степіня");
                        }
                        double result = 1;
                        for (int i = 0; i > Numeric2; i--)
                        {
                            result /= Numeric1;
                        }
                        return result;
                    }
                    else if (Numeric2 == 0)
                    {
                        return 1;
                    }
                    else if (Numeric2 == 1)
                    {
                        return Numeric1;
                    }
                    else
                    {
                        double result = 1;
                        for (int i = 0; i < Numeric2; i++)
                        {
                            result *= Numeric1;
                        }
                        return result;
                    }
                default:
                    throw new ArgumentException("Невідомий операнд" + op);
            }
        }

        static double Calculation(ArrayList Tokens)
        {
            var stack = new Stack();
            foreach (var t in Tokens)
            {
                if (t is double)
                {
                    stack.Push(t.ToString());
                }
                else
                {
                    double Numeric_2 = double.Parse(stack.Pop().ToString());
                    double Numeric_1 = double.Parse(stack.Pop().ToString());
                    double result_0 = Math(Numeric_1, Numeric_2, t.ToString());
                    stack.Push(result_0.ToString());
                }
            }
            double result_1 = double.Parse(stack.Peek().ToString());
            if (result_1 % 1 == 0)
            {
                int result_2 = (int)result_1;
                return result_2;
            }
            else
            {
                return result_1;
            }


        }
        static void Main(string[] args)
        {

            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Red;

            Console.WriteLine("\t КАЛЬКУЛЯТОР");

            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Red;
                  
            try
            {
                for (int i = 0; ; i++)
                {
                    Console.Write("Введіть вираз для обрахуваання \n < ");
                    string exspression = Console.ReadLine();
                    var Tokens = Tok(exspression);
                    var Polski_Tokens = Postfix(Tokens);
                    var Result = Calculation(Polski_Tokens);
                    Console.WriteLine("Результат \n < " + Result);
                    
                }
            }
            catch
            {
                Console.WriteLine("Ведено неправельний вираз");
            }

        }
    }
}
