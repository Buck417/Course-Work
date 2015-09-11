using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaEvaluator
{
    /// <summary>
    /// This class has one main method and uses several helper methods to evaluate an expression given to it
    /// assuming that the expression only contains +, -, *, /, (), or variables that start with one or more characters
    /// and end with one or more integers.
    /// </summary>

    public static class Evaluator
    {
        /// <summary>
        /// Delegate that uses the Lookup library to return a value when given a String.
        /// </summary>
        /// <param name="v">The string given to delegate used to determine the value</param>
        /// <returns></returns>
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
            Stack<int> values = new Stack<int>();
            Stack<String> operators = new Stack<String>();
            String[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            int tempVal, first, second;

            for (int i = 0; i < substrings.Length; i++)                         //For loop that goes through each valid case for a specific int or string
            {
                String instance = substrings[i];
                if (instance == " " || instance == "")                          //Increments if the string is empty
                {
                    continue;
                }

                if (int.TryParse(instance, out tempVal))                        //Tries to parse a string to an int, if successful passes the int to the helper method
                {
                    performOperation(tempVal, values, operators);
                }
                else if (isVar(instance))                                       //Checks if the string is a valid variable, if true it uses the delegate to return an int value and pass it to the helper method
                {
                    int variableValue = variableEvaluator(instance);
                    performOperation(variableValue, values, operators);
                }

                else if (instance == "*" || instance == "/")                    //Checks for the multiply and divide operators in the string, pushes to stack if true
                {
                    operators.Push(instance);
                }

                else if (instance == "+" | instance == "-")                     //Checks for the add or subtract operator in the string
                {
                    if (!(operators.Count == 0))
                    {
                        if (operators.Peek() == "+" | operators.Peek() == "-")                                  //Performs the addition or subtraction if there are two valid values and one operator in the stacks
                        {
                            if (values.Count <= 1)
                                throw new ArgumentException("The expression is invalid, not enough values");

                            second = values.Pop();
                            first = values.Pop();
                            String operation = operators.Pop();

                            if (operation == "+")
                            {
                                tempVal = first + second;
                                values.Push(tempVal);
                            }
                            else
                                tempVal = first - second;
                            
                        }

                    }
                    operators.Push(instance);
                }

                else if (instance == "(")                                                   //Checks if the string contains a left parenthesis, if true pushes it to the operator stack
                {
                    operators.Push(instance);
                }

                else if (instance == ")")                                                   //Checks if the string contains a right parenthesis
                {
                    if (operators.Peek() == "+" || operators.Peek() == "-")                 //Checks if the next operator in the stack is a + or - and performs the operation to the two most current values in the value stack
                    {
                        if (values.Count <= 1)
                            throw new ArgumentException("The expression is invalid, not enough values");

                        second = values.Pop();
                        first = values.Pop();
                        String operation = operators.Pop();
                        if (operation == "+")
                        {
                            first = first + second;
                            values.Push(first); 
                        }
                        else
                        {
                            first = first - second;
                            values.Push(first); 
                        }
                    }

                    if (operators.Count == 0)                    //Checks if the operator stack is empty, throws an exception if true
                    {
                        throw new ArgumentException("There is a missing '(' in the expression");
                    }

                    if (operators.Peek() != "(")                 //Checks if the operator stacks next operator is not a left parenthesis, throws an exception if true
                    {
                        throw new ArgumentException("There is a missing '(' in the expression");
                    }

                    operators.Pop();

                    if (!(operators.Count == 0))
                    {
                        if (operators.Peek() == "*" | operators.Peek() == "/")                          //Checks if the next operator is a * or a /, performs the operation on the stacks
                        {
                            if (values.Count <= 1)
                                throw new ArgumentException("The expression is invalid, not enough values");

                            second = values.Pop();
                            first = values.Pop();
                            String operation = operators.Pop();
                            if (operation == "*")
                            {
                                first = first * second;
                                values.Push(first);
                            }
                            else
                            {

                                if (second == 0)
                                {
                                    throw new ArgumentException("Cannot divide by zero");
                                }
                                Double divisionDouble = first / second;
                                int divisionInt = (int)Math.Truncate(divisionDouble);
                                values.Push(divisionInt);
                            }
                        }
                    }
        
                }
                }

           if (values.Count == 1 && operators.Count == 0)                   //If there only exists one value on the value stack after the loop is finished, this is our answer
                {
                    return values.Pop();
                }
           else if (values.Count == 2 && operators.Count == 1)              //If there only exists two values and one operator left on the stacks after the loop is finished, perform the operation and return the value that is our answer
                {
                    second = values.Pop();
                    first = values.Pop();
                    String operation = operators.Pop();
                    if(operation == "+"){
                        return first + second;
                        
                    }
                    else{
                        return first - second;
                    }
                        
                  }
                
            else{
                  throw new ArgumentException("The expression given is likely invalid");            //If the conditions above are not met, then the formula given was not correctly input into the method

                }
}

            
        

        /// <summary>
        /// This helper method either returns true or false depending on the string given.
        /// This is used to determine if the variable given is valid according to rules given in the assignment.
        /// </summary>
        /// <param name="substringGiven">The variable that its given to see if it is valid.</param>
        /// <returns>Returns true if the variable is valid and false if it isnt.</returns>
        public static Boolean isVar(String substringGiven)
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

        /// <summary>
        /// Helper method that performs multiplication or division with the stack and a value given.
        /// Used to immediately perform the operations without having to push onto the stack if multiplication or division is required
        /// and helps maintain proper order of operations.
        /// </summary>
        /// <param name="givenValue">Value given that requires either an operation performed or a stack push</param>
        /// <param name="numbers">The stack that contains a value used with an operation</param>
        /// <param name="operators">The stack that contains an operator used with two values</param>
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
