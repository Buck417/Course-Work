// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax; variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        private String validFormula;
        private List<String> tokenFormula;

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            if (formula.Length < 1)
            {
                throw new FormulaFormatException("The formula given is empty");
            }

           // tokenFormula = GetTokens(formula);
            validFormula = normalize(formula);
        }


        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            Stack<Double> values = new Stack<Double>();
            Stack<String> operators = new Stack<String>();
            String[] substrings = Regex.Split(validFormula, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            double tempVal, firstStackValue, secondStackValue;

            for (int i = 0; i < substrings.Length; i++)                         //For loop that goes through each valid case for a specific int or string
            {
                String instance = substrings[i];
                if (instance == " " || instance == "")                          //Increments if the string is empty
                {
                    continue;
                }

                if (Double.TryParse(instance, out tempVal))                        //Tries to parse a string to an int, if successful passes the int to the helper method
                {
                    if (operators.Count != 0)                                       //Checks if the operator is divides and if the divisor is 0, returns an error if true
                    {
                        if (operators.Peek() == "/")
                        {
                            if (tempVal == 0)
                            {
                                return new FormulaError("Cannot divide by zero");
                            }
                        }
                    }

                    performOperation(tempVal, values, operators);
                }
                else if (isVar(instance))                                       //Checks if the string is a valid variable, if true it uses the delegate to return an int value and pass it to the helper method
                {
                    Double variableValue = lookup(instance);
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
                                return new FormulaError("The expression is invalid, not enough values");

                            secondStackValue = values.Pop();
                            firstStackValue = values.Pop();
                            String operation = operators.Pop();

                            if (operation == "+")
                            {
                                tempVal = firstStackValue + secondStackValue;
                                values.Push(tempVal);
                            }
                            else
                                tempVal = firstStackValue - secondStackValue;

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
                            return new FormulaError("The expression is invalid, not enough values");

                        secondStackValue = values.Pop();
                        firstStackValue = values.Pop();
                        String operation = operators.Pop();
                        if (operation == "+")
                        {
                            firstStackValue = firstStackValue + secondStackValue;
                            values.Push(firstStackValue);
                        }
                        else
                        {
                            firstStackValue = firstStackValue - secondStackValue;
                            values.Push(firstStackValue);
                        }
                    }

                    if (operators.Count == 0)                    //Checks if the operator stack is empty, throws an exception if true
                    {
                        return new FormulaError("There is a missing '(' in the expression");
                    }

                    if (operators.Peek() != "(")                 //Checks if the operator stacks next operator is not a left parenthesis, throws an exception if true
                    {
                        return new FormulaError("There is a missing '(' in the expression");
                    }

                    operators.Pop();

                    if (!(operators.Count == 0))
                    {
                        if (operators.Peek() == "*" | operators.Peek() == "/")                          //Checks if the next operator is a * or a /, performs the operation on the stacks
                        {
                            if (values.Count <= 1)
                                return new FormulaError("The expression is invalid, not enough values");

                            secondStackValue = values.Pop();
                            firstStackValue = values.Pop();
                            String operation = operators.Pop();
                            if (operation == "*")
                            {
                                firstStackValue = firstStackValue * secondStackValue;
                                values.Push(firstStackValue * secondStackValue);
                            }
                            else
                            {

                                if (secondStackValue == 0)
                                {
                                    return new FormulaError("Cannot divide by zero");
                                }
                                Double divisionDouble = firstStackValue / secondStackValue;
                                values.Push(divisionDouble);
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
                secondStackValue = values.Pop();
                firstStackValue = values.Pop();
                String operation = operators.Pop();
                if (operation == "+")
                {
                    return firstStackValue + secondStackValue;

                }
                else
                {
                    return firstStackValue - secondStackValue;
                }

            }

            else
            {
                return new FormulaError("The expression given is likely invalid");            //If the conditions above are not met, then the formula given was not correctly input into the method

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
            Double val;
            if (Regex.IsMatch(substringGiven, "A"))
            {
                if ((Regex.IsMatch(varSubString[1], @"^[a-zA-Z]+$")) && (Double.TryParse(varSubString[varSubString.Length - 2], out val)))
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
        public static void performOperation(Double givenValue, Stack<Double> numbers, Stack<string> operators)
        {
            Double stackValue;

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



        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            return null;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return validFormula;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens, which are compared as doubles, and variable tokens,
        /// whose normalized forms are compared as strings.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            return false;
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            return false;
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return false;
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return 0;
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}

