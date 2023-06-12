using System.Collections.Generic;
using UnityEngine;
using System;

public class IncorrectFormulaException : Exception
{
    public IncorrectFormulaException()
    {
    }

    public IncorrectFormulaException(string message)
        : base(message)
    {
    }
}

public class Token
{
}

public class Number : Token
{
    public float value;
    public Number(float v)
    {
        value = v;
    }
}

public class Variable : Token
{
    public char name;
    public Variable(char n)
    {
        name = n;
    }
}

public class OpenParenthesis : Token { }
public class CloseParenthesis : Token { }  

public class Operator : Token {
}
public class Plus : Operator {
}
public class Minus : Operator {
}
public class Times : Operator {
}
public class DividedBy : Operator {
}
public class Power : Operator {
}


public static class KeyboardReader
{

    public static Vector3 g = new Vector3(0f, -9.8f, 0f);

    public static float A = 3;
    public static float m = 2;
    public static float k = 8;

    /*public static Queue<Token> Shunting_yard_algorithm(List<Token> input)
    {
        if (input.Count == 0)
            throw new IncorrectFormulaException("The formula is empty");
        else if (input[0] is Operator) //todo maybe add negative numbers
            throw new IncorrectFormulaException("The formula cannot start with an operator");
        else if (input[input.Count - 1] is Operator)
            throw new IncorrectFormulaException("The formula cannot end with an operator");

        Queue<Token> output = new Queue<Token>();
        Stack<Token> operators = new Stack<Token>();
        Token previous = null;

        foreach (Token t in input)
        {
            if (t is Number || t is Variable)
            {
                //todo if both numbers merge (and output now is a queue of strings)
                if (previous is Number || previous is Variable)
                    throw new IncorrectFormulaException("There are two numbers or vectors in a row in the formula, there should be an operator between them");
                output.Enqueue(t);
            }
            else if (t is Operator)
            {
                if (previous is Operator || previous is OpenParenthesis)
                    throw new IncorrectFormulaException("There cannot be two operators in a row in the expression");
                while (operators.Count != 0 && !(operators.Peek() is OpenParenthesis) && (GetPrecedence(operators.Peek()) > GetPrecedence(t)
                    || (GetPrecedence(operators.Peek()) == GetPrecedence(t) && !(t is Power))))
                {
                    output.Enqueue(operators.Pop());
                }
                operators.Push(t);
            }
            else if (t is OpenParenthesis) operators.Push(t);
            else if (t is CloseParenthesis)
            {
                if (operators.Count == 0)
                    throw new IncorrectFormulaException("There are more close parentheses than open ones in the formula");
                while (operators.Peek() != '(')
                {
                    output.Enqueue(operators.Pop().ToString());
                    if (operators.Count == 0)
                        throw new IncorrectFormulaException("There are more close parentheses than open ones in the formula");
                }
                operators.Pop();
            }
            else
                throw new IncorrectFormulaException(c + " is not a supported character");
            previous = c;
        }

        while (operators.Count != 0)
        {
            char o = operators.Pop();
            if (o == '(')
                throw new IncorrectFormulaException("There are more open parentheses than close ones in the formula");
            output.Enqueue(o.ToString());
        }

        //throw additional exceptions that wouldn't be detected otherwise
        getValueFromFormula(output, new Vector3(), new Vector3(), 0);

        return output;
    }*/

    
    public static Queue<string> Shunting_yard_algorithm(string input, bool vectorExpected, bool numberExpected)
    {
        if (input.Length == 0)
            throw new IncorrectFormulaException("The formula is empty");
        else if (IsOperator(input[0])) //todo maybe add negative numbers
            throw new IncorrectFormulaException("The formula cannot start with an operator");
        else if (IsOperator(input[input.Length - 1]))
            throw new IncorrectFormulaException("The formula cannot end with an operator");

        Queue<string> output = new Queue<string>();
        Stack<char> operators = new Stack<char>();
        char previous = ' ';

        while (input.Length != 0)
        {
            char c = input[0];

            input = input.Substring(1);

            if (char.IsNumber(c))
            {
                if (IsVariable(previous) || IsFunction(previous))
                {
                    throw new IncorrectFormulaException("There are two numbers or vectors in a row in the formula, there should be an operator between them");
                }
                string number = "" + c;
                while (input.Length > 0 && char.IsNumber(input[0]))
                {
                    number = number + input[0];
                    input = input.Substring(1);
                }
                if (input.Length>0 && input[0] == '.')
                {
                    number = number + '.';
                    input = input.Substring(1);
                    if (input.Length == 0 || !char.IsNumber(input[0]))
                    {
                        throw new IncorrectFormulaException("A digit is expected after " + number);
                    }
                    while (input.Length > 0 && char.IsNumber(input[0]))
                    {
                        number = number + input[0];
                        input = input.Substring(1);
                    }
                }
                output.Enqueue(number);
            }
            else if (IsVariable(c))
            {
                if (char.IsNumber(previous) || IsVariable(previous) || IsFunction(previous))
                    throw new IncorrectFormulaException("There are two numbers or vectors in a row in the formula, there should be an operator between them");
                output.Enqueue(c.ToString());
            }
            else if (IsFunction(c))
            {
                if (char.IsNumber(previous) || IsVariable(previous) || IsFunction(previous))
                    throw new IncorrectFormulaException("There are two numbers or vectors in a row in the formula, there should be an operator between them");
                operators.Push(c);
            }
            else if (IsOperator(c))
            {
                if (IsOperator(previous) || previous=='(')
                    throw new IncorrectFormulaException("There cannot be two operators in a row " + previous + " and " + c + " in the expression");
                while (operators.Count != 0 && operators.Peek() != '(' && (GetPrecedence(operators.Peek()) > GetPrecedence(c)
                    || (GetPrecedence(operators.Peek()) == GetPrecedence(c) && c != '^')))
                {
                    output.Enqueue(operators.Pop().ToString());
                }
                operators.Push(c);
            }
            else if (c == '(') operators.Push(c);
            else if (c == ')')
            {
                if (operators.Count == 0)
                    throw new IncorrectFormulaException("There are more close parentheses than open ones in the formula");
                while (operators.Peek() != '(')
                {
                    output.Enqueue(operators.Pop().ToString());
                    if (operators.Count == 0)
                        throw new IncorrectFormulaException("There are more close parentheses than open ones in the formula");
                }
                operators.Pop();
                if (IsFunction(operators.Peek()))
                {
                    output.Enqueue(operators.Pop().ToString());
                }
            }
            else if (c=='.')
            {
                throw new IncorrectFormulaException("There needs to be a digit before a .");
            }
            else
                throw new IncorrectFormulaException(c + " is not a supported character");
            previous = c;
        }

        while (operators.Count != 0)
        {
            char o = operators.Pop();
            if (o == '(')
                throw new IncorrectFormulaException("There are more open parentheses than close ones in the formula");
            output.Enqueue(o.ToString());
        }

        //throw additional exceptions that wouldn't be detected otherwise
        if (vectorExpected)
        {
            getVectorFromFormula(output, new Vector3(), new Vector3(), 0);
        }
        else if (numberExpected)
        {
            getFloatFromFormula(output, 0);
        }
        else
        {
            getValueFromFormula(output, new Vector3(), new Vector3(), 0);

        }

        return output;
    }

    public static float getFloatFromFormula(Queue<string> q, float t)
    {
        (bool, float, Vector3) tuple = getValueFromFormula(q, new Vector3(), new Vector3(), t);

        if (tuple.Item1)
            throw new IncorrectFormulaException("the formula written results in a vector, it should be a number");
        return tuple.Item2;

    }

    public static Vector3 getVectorFromFormula(Queue<string> q, Vector3 p, Vector3 v, float t)
    {
        (bool, float, Vector3) tuple = getValueFromFormula(q, p, v, t);
        if (!tuple.Item1)
            throw new IncorrectFormulaException("the formula written results in a number, it should be a vector");
        return tuple.Item3;
    }

    public static (bool,float,Vector3) getValueFromFormula(Queue<string> q, Vector3 p, Vector3 v, float t)
    {
        Stack<Vector3> sv = new Stack<Vector3>();
        Stack<float> sf = new Stack<float>();
        Stack<bool> isVector = new Stack<bool>();
        Queue<string> f = new Queue<string>(q);

        while (f.Count != 0)
        {

            string token = f.Dequeue();
            if (float.TryParse(token, out float n))
            {
                sf.Push(n);
                isVector.Push(false);
            }
            else if (token == "t")
            {
                sf.Push(t);
                isVector.Push(false);
            }
            else if (token == "v")
            {
                sv.Push(v);
                isVector.Push(true);
            }
            else if (token == "p")
            {
                sv.Push(p);
                isVector.Push(true);
            }
            else if (token == "g")
            { //todo write a variable system instead
                sv.Push(g);
                isVector.Push(true);
            }
            else if (token == "A")
            {
                sf.Push(A);
                isVector.Push(false);
            }
            else if (token == "k")
            {
                sf.Push(k);
                isVector.Push(false);
            }
            else if (token == "m")
            {
                sf.Push(m);
                isVector.Push(false);
            }
            else if (token == "s")
            {
                bool b = isVector.Pop();
                if (b)
                    throw new IncorrectFormulaException("sin cannot be used on a vector");
                sf.Push(Mathf.Sin(sf.Pop()));
                isVector.Push(false);
            }
            else if (token == "√")
            {
                bool b = isVector.Pop();
                if (b)
                    throw new IncorrectFormulaException("square root cannot be used on a vector");
                sf.Push(Mathf.Sqrt(sf.Pop()));
                isVector.Push(false);
            }
            else
            {
                bool b2 = isVector.Pop();
                bool b1 = isVector.Pop();
                if (token == "+")
                {
                    if (b1 ^ b2)
                        throw new IncorrectFormulaException("+ cannot be used between a number and a vector");
                    if (b1)
                    {
                        sv.Push(sv.Pop() + sv.Pop());
                    }
                    else
                    {
                        sf.Push(sf.Pop() + sf.Pop());
                    }
                    isVector.Push(b1);
                }
                else if (token == "-")
                {
                    if (b1 ^ b2)
                        throw new IncorrectFormulaException("- cannot be used between a number and a vector");
                    if (b1)
                    {
                        Vector3 subtrahend = sv.Pop();
                        sv.Push(sv.Pop() - subtrahend);
                    }
                    else
                    {
                        float subtrahend = sf.Pop();
                        sf.Push(sf.Pop() - subtrahend);
                    }
                    isVector.Push(b1);
                }
                else if (token == "×")
                {
                    if (b1 && b2)
                        throw new IncorrectFormulaException("× cannot be used between two vectors");
                    if (b1 || b2)
                    {
                        sv.Push(sf.Pop() * sv.Pop());
                    }
                    else
                    {
                        sf.Push(sf.Pop() * sf.Pop());
                    }
                    isVector.Push(b1 || b2);
                }
                else if (token == "/")
                {
                    if (b2)
                        throw new IncorrectFormulaException("/ cannot be used with a vector as divisor");
                    if (b1)
                    {
                        sv.Push(sv.Pop() / sf.Pop());
                    }
                    else
                    {
                        float divisor = sf.Pop();
                        sf.Push(sf.Pop() / divisor);
                    }
                    isVector.Push(b1);
                }
                else if (token == "^")
                {
                    if (b1 || b2)
                        throw new IncorrectFormulaException("^ cannot be used with vectors");
                    float exponent = sf.Pop();
                    sf.Push(Mathf.Pow(sf.Pop(), exponent));
                    isVector.Push(false);
                }
            }
        }

        if (sv.Count == 0)
            return (false, sf.Pop(), Vector3.zero);
        else
            return (true, 0, sv.Pop());
    }

    public static bool IsVariable(char c)
    {
        return c == 'g' || c == 'A' || c == 'k' || c == 'm' || c == 'v' || c == 'p' || c == 't'; //todo write a variable system instead for g and other variables
    }

    public static bool IsOperator(char c)
    {
        return c == '+' || c == '-' || c == '×' || c == '/' || c == '^';
    }

    public static bool IsFunction(char c)
    {
        return c == 's' || c == '√';
    }

    public static int GetPrecedence(char c)
    {
        switch (c)
        {
            case '^':
                return 4;
            case '×':
                return 3;
            case '/':
                return 3;
            case '+':
                return 2;
            case '-':
                return 2;
            default:
                throw new System.Exception(); //todo exception
               
        }
    }
}

