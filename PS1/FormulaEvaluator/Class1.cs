using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaEvaluator
{


    public static class Evaluator
    {

        public delegate int Lookup(String v);




        /// <summary>
        /// A method that uses stacks to separate a string formula and perform operations on the values according to
        /// standard order of operations in arithmatic and return the value of the formula if the formula is valid.
        /// Throws an Argument Exception if the formula is invalid.
        /// </summary>
        /// <param name="exp"> A formula used to evaluate. </param>
        /// <param name="variableEvaluator"> A delegate that takes a string and returns the string value, used for variables in a formula. </param>
        /// <returns>Returns an int value of the solved formula. </returns>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            Stack<int> numbers = new Stack<int>();
            Stack<String> operators = new Stack<String>();
            String[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            int val, first, second;

            for (int i = 0; i < substrings.Length; i++)
            {
                String instance = substrings[i];
                int variableValue;
                if (instance == " " || instance == "")
                {
                    continue;
                }

                if (int.TryParse(instance, out val))
                {
                    performOperation(val, numbers, operators);
                }
                else if (Variable(instance))
                {
                    variableValue = variableEvaluator(instance);
                    numbers.Push(variableValue);
                }

                else if (instance == "*" || instance == "/")
                {
                    operators.Push(instance);
                }

                else if (instance == "+" | instance == "-")
                {
                    if (!(operators.Count == 0))
                    {
                        if (operators.Peek() == "+" | operators.Peek() == "-")
                        {
                            if (numbers.Count <= 1)
                                throw new ArgumentException("The expression is invalid, not enough values");

                            second = numbers.Pop();
                            first = numbers.Pop();
                            String operation = operators.Pop();

                            if (operation == "+")
                            {
                                val = first + second;
                                numbers.Push(val);
                            }
                            else
                                val = first - second;
                            
                        }

                    }
                    operators.Push(instance);
                }

                else if (instance == "(")
                {
                    operators.Push(instance);
                }

                else if (instance == ")")
                {
                    if (operators.Peek() == "+" || operators.Peek() == "-")
                    {
                        if (numbers.Count <= 1)
                            throw new ArgumentException("The expression is invalid, not enough values");

                        second = numbers.Pop();
                        first = numbers.Pop();
                        String operation = operators.Pop();
                        if (operation == "+")
                        {
                            first = first + second;
                            numbers.Push(first); 
                        }
                        else
                        {
                            first = first - second;
                            numbers.Push(first); 
                        }
                    }

                    if (operators.Count == 0 || operators.Peek() != "(")
                    {
                        throw new ArgumentException("There is a missing '(' in the expression");
                    }

                    operators.Pop();

                    if (operators.Count > 0)
                    {
                        if (operators.Peek() == "*" | operators.Peek() == "/")
                        {
                            if (numbers.Count <= 1)
                                throw new ArgumentException("The expression is invalid, not enough values");

                            second = numbers.Pop();
                            first = numbers.Pop();
                            String operation = operators.Pop();
                            if (operation == "*")
                            {
                                first = first * second;
                                numbers.Push(first);
                            }
                            else
                            {

                                if (second == 0)
                                {
                                    throw new ArgumentException("Cannot divide by zero");
                                }
                                Double divisionDouble = first / second;
                                int divisionInt = (int)Math.Truncate(divisionDouble);
                                numbers.Push(divisionInt);
                            }
                        }
                    }
        
                }
                }

           if (numbers.Count == 1 && operators.Count == 0)
                {
                    return numbers.Pop();
                }
           else if (numbers.Count == 2 && operators.Count == 1) 
                {
                    second = numbers.Pop();
                    first = numbers.Pop();
                    String operation = operators.Pop();
                    if(operation == "+"){
                        return first + second;
                        
                    }
                    else{
                        return first - second;
                    }
                        
                  }
                
            else{
                  throw new ArgumentException("The expression given is likely invalid");

                }
}

            
        

        /// <summary>
        /// This helper method either returns true or false depending on the string given.
        /// This is used to determine if the variable given is valid according to rules given in the assignment.
        /// </summary>
        /// <param name="substringGiven">The variable that its given to see if it is valid.</param>
        /// <returns>Returns true if the variable is valid and false if it isnt.</returns>
        public static Boolean Variable(String substringGiven)
        {
            string[] varSubString = Regex.Split(substringGiven, string.Empty);
            int val;
            if (Regex.IsMatch(substringGiven, @"^[a-zA-Z0-9]+$"))
            {
                if ((Regex.IsMatch(varSubString[1], @"^[a-zA-Z]+$")) && (int.TryParse(varSubString[varSubString.Length - 2], out val)))
                {
                    return true;
                }
            }
            return false;
        }

        public static void performOperation(int givenValue, Stack<int> numbers, Stack<string> operators)
        {
            int stackValue;

            if (operators.Count != 0)
            {


                if (operators.Peek() == "*")
                {
                    operators.Pop();
                    stackValue = numbers.Pop();
                    stackValue = stackValue * givenValue;
                    numbers.Push(stackValue);
                    return;
                }

                if (operators.Peek() == "/")
                {
                    if (givenValue == 0)
                    {
                        throw new ArgumentException("Cannot divide by zero");
                    }
                    operators.Pop();
                    stackValue = numbers.Pop();
                    Double divisionDouble = stackValue / givenValue;
                    int divisionInt = (int)Math.Truncate(divisionDouble);
                    numbers.Push(divisionInt);
                    return;
                }
            }
           
            numbers.Push(givenValue);
        }

    }

}
