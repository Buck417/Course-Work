// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// Version 1.2 (9/24/2015 11:59 p.m.)

//Change log:
// (Version 1.21) Implemented the constructors and methods left undone
// Author: Ryan Fletcher

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
        private HashSet<String> variables = new HashSet<String>();

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
            //variables used to keep track of parenthesis
            int openParenthesis = 0;
            int closedParenthesis = 0;
           
            //variable used to determine if a token is a number
            double tempVal;

            //variable used to normalize a formula variable
            String tempVariable;

            //String used to determine what came before the current token
            String previous = null;
         
            //Stringbuilder used to create the valid formula
            StringBuilder formulaBuilder = new StringBuilder();

            //Checks if the formula is not an empty string
            if (formula.Length < 1)
            {
                throw new FormulaFormatException("The formula given is empty");
            }

            //Used to see if the beginning and end of the formula are correct
            string[] formulaTokens = GetTokens(formula).ToArray();

            //At this point, this is called when a formula given was just white spaces and the tokens returned would be 0
            if (formulaTokens.Length < 1)
                throw new FormulaFormatException("The formula given is empty");

            //Checks if the beginning of the formula starts with a "(", a number, or a variable
            if (formulaTokens[0] != "(" && !(isVar(formulaTokens[0])) && !(Double.TryParse(formulaTokens[0], out tempVal)))
                throw new FormulaFormatException("The formula given is in an incorrect format");

            //Checks if the ending of the formula ends with a ")", a number, or a variable
            if (formulaTokens[formulaTokens.Length - 1] != ")" && !(isVar(formulaTokens[formulaTokens.Length-1])) && !(Double.TryParse(formulaTokens[formulaTokens.Length - 1], out tempVal)))
                throw new FormulaFormatException("The formula given is in an incorrect format");

            //Foreach loop used to loop through the string to create the formula
            foreach (String s in GetTokens(formula))
            {


                //Must initially check the first token because previous is null and you cannot check the value of previous
                if(previous == null)                        
                {
                    if(Double.TryParse(s, out tempVal)) 
                    {
                        formulaBuilder.Append(s);
                        previous = s;
                    }
                    else if(isVar(s))
                    {
                        formulaBuilder.Append(s);
                        previous = s;
                        variables.Add(s);
                    }
                    else if(s == "(")
                    {
                        formulaBuilder.Append(s);
                        previous = s;
                        openParenthesis++;
                    }
                    else
                        throw new FormulaFormatException("The formula given is in an incorrect format");
                    continue;
                }


                //This if block checks for an open parenthesis in the formula string
                if (s == "(")
                {
                    openParenthesis++;
                    if(previous == "(" || isOperator(previous)){
                        formulaBuilder.Append(s);
                        previous = s;
                    
                    }
                    else{
                        throw new FormulaFormatException("An open parenthesis was not preceded by an operator or another open parenthesis");
                    }
                }

                //This if block checks for a closed parenthesis in the formula string
                if (s == ")")
                {
                    closedParenthesis++;
                    if(isVar(previous) || previous == ")" || Double.TryParse(previous, out tempVal)){
                        formulaBuilder.Append(s);
                        previous = s;
                    }
                    else{
                        throw new FormulaFormatException("An closed parenthesis was not preceded by a number, variable, or another closed parenthesis");
                    }
                }

                //This if block checks for a number in the formula string
                if(Double.TryParse(s, out tempVal))
                {
                    if(previous == "(" || isOperator(previous)){
                        formulaBuilder.Append(s);
                        previous = s;
                    }
                    else{
                        throw new FormulaFormatException("A number was not preceded by an open parenthesis or an operator");
                    }
                }

                //This if block checks for a number in the formula string and normalizes and validates the variable
                if(isVar(s))
                {
                    if(previous == "(" || isOperator(previous)){
                        tempVariable = normalize(s);
                        if(!isValid(tempVariable))
                            throw new FormulaFormatException("The normalized version of a variable is not valid");
                        formulaBuilder.Append(tempVariable);
                        variables.Add(tempVariable);
                        previous = s;
                        
                    }
                    else{
                        throw new FormulaFormatException("A number was not preceded by an open parenthesis or an operator");
                    }
                }

                //This if block checks for an operator in the formula string
                if(isOperator(s))
                {
                    if(previous == ")" || Double.TryParse(previous, out tempVal)|| isVar(previous)){
                        formulaBuilder.Append(s);
                        previous = s;
                    }
                    else{
                        throw new FormulaFormatException("An operator was not preceded by a closed parenthesis, a number, or a variable");
                    }
                }


            }

            //Checks to see equal parenthesis existed in the formula provided
            if(openParenthesis != closedParenthesis)
                throw new FormulaFormatException("There were not an equal amount of open to closed parenthesis");
           
            //Takes every token appended to formulaBuilder and forms a formula for the class
            validFormula = formulaBuilder.ToString();
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

            //For loop that goes through each valid case for a specific int or string
            for (int i = 0; i < substrings.Length; i++)                         
            {
                String instance = substrings[i];
                //Increments if the string is empty
                if (instance == " " || instance == "")                          
                {
                    continue;
                }

                //Tries to parse a string to an int, if successful passes the int to the helper method
                if (Double.TryParse(instance, out tempVal))                        
                {
                    //Checks if the operator is divides and if the divisor is 0, returns an error if true
                    if (operators.Count != 0)                                       
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
                //Checks if the string is a valid variable, if true it uses the delegate to return an int value and pass it to the helper method
                else if (isVar(instance))                                       
                {
                    Double variableValue = lookup(instance);

                    //Checks if the operator is divides and if the divisor is 0, returns an error if true
                    if (operators.Count != 0)                                       
                    {
                        if (operators.Peek() == "/")
                        {
                            if (variableValue == 0)
                            {
                                return new FormulaError("Cannot divide by zero");
                            }
                        }
                    }
                    performOperation(variableValue, values, operators);
                }

                //Checks for the multiply and divide operators in the string, pushes to stack if true
                else if (instance == "*" || instance == "/")                    
                {
                    operators.Push(instance);
                }

                //Checks for the add or subtract operator in the string
                else if (instance == "+" | instance == "-")                     
                {
                    if (!(operators.Count == 0))
                    {
                        //Performs the addition or subtraction if there are two valid values and one operator in the stacks
                        if (operators.Peek() == "+" | operators.Peek() == "-")                                  
                        {
                            if (values.Count <= 1)
                            {
                                return new FormulaError("The expression is invalid, not enough values");
                            }
                                
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

                //This else if checks if the string contains a left parenthesis, if true pushes it to the operator stack
                else if (instance == "(")                                                   
                {
                    operators.Push(instance);
                }

                //This else if block checks if the string contains a right parenthesis
                else if (instance == ")")                                                   
                {
                    //Checks if the next operator in the stack is a + or - and performs the operation to the two most current values in the value stack
                    if (operators.Peek() == "+" || operators.Peek() == "-")                 
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

                    //Checks if the operator stack is empty, throws an exception if true
                    if (operators.Count == 0)                    
                    {
                        return new FormulaError("There is a missing '(' in the expression");
                    }

                    //Checks if the operator stacks next operator is not a left parenthesis, throws an exception if true
                    if (operators.Peek() != "(")                 
                    {
                        return new FormulaError("There is a missing '(' in the expression");
                    }

                    operators.Pop();

                    if (!(operators.Count == 0))
                    {
                        //Checks if the next operator is a * or a /, performs the operation on the stacks
                        if (operators.Peek() == "*" | operators.Peek() == "/")                          
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

                else
                {
                    return new FormulaError("The expression was given an incorrect variable format");
                }
            }

            //If there only exists one value on the value stack after the loop is finished, this is our answer
            if (values.Count == 1 && operators.Count == 0)                   
            {
                return values.Pop();
            }

            //If there only exists two values and one operator left on the stacks after the loop is finished, perform the operation and return the value that is our answer
            else if (values.Count == 2 && operators.Count == 1)             
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

            //If the conditions above are not met in the entire Evaluate class, then the formula given was not correctly input into the method
            else
            {
                return new FormulaError("The expression given is likely invalid");            
            }
        }

        /// <summary>
        /// This helper method either returns true or false depending on the string given.
        /// This is used to determine if the variable given is valid according to rules given in the assignment.
        /// </summary>
        /// <param name="substringGiven">The variable that its given to see if it is valid.</param>
        /// <returns>Returns true if the variable is valid and false if it isnt.</returns>
        private static Boolean isVar(String substringGiven)
        {
            string[] varSubString = Regex.Split(substringGiven, string.Empty);
            if (!(Regex.IsMatch(varSubString[1], @"^[a-zA-Z_]+$")))
                return false;
            if (Regex.IsMatch(substringGiven, @"^\w+$"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Helper method that returns a boolean depending on if the string given is an operator or not
        /// </summary>
        /// <param name="substringGiven">String needed to determine if it is an operator or not</param>
        /// <returns></returns>
        private static Boolean isOperator(String substringGiven)
        {
            if (substringGiven == "+" || substringGiven == "-" || substringGiven == "*" || substringGiven == "/")
                return true;
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
        private static void performOperation(Double givenValue, Stack<Double> numbers, Stack<string> operators)
        {
            //Used as a place holder for the answer of the evaluation
            Double stackValue;

            if (operators.Count != 0)
            {
                //If an operator * is on the stack, multiply the two values and return the answer to the stack
                if (operators.Peek() == "*")
                {
                    operators.Pop();
                    stackValue = numbers.Pop();
                    stackValue = stackValue * givenValue;
                    numbers.Push(stackValue);
                    return;
                }
                //If an operator / is on the stack, divide the two values and return the answer to the stack. Note that the check for dividing by 0 already happened.
                if (operators.Peek() == "/")
                {
                    operators.Pop();
                    stackValue = numbers.Pop();
                    Double divisionDouble = stackValue / givenValue;
                    numbers.Push(divisionDouble);
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
            //This will loop through a hashset of variables created when the formula object was created, returning a variable that exists in the formula
            //each time the loop is called.
            foreach (String s in variables)
            {
                yield return s;
            }
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
            if (obj == null)
                return false;
            if (obj.GetType() != this.GetType())
                return false;
            Formula temp = (Formula)obj;
            return this.ToString().Equals(temp.ToString());
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            if (Object.ReferenceEquals(f1, null) && Object.ReferenceEquals(f2, null))
                return true;

            if (!(Object.ReferenceEquals(f1, null)) && Object.ReferenceEquals(f2, null))
                return false;

            if ((Object.ReferenceEquals(f1, null)) && !(Object.ReferenceEquals(f2, null)))
                return false;

            return f1.ToString().Equals(f2.ToString());
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            if (Object.ReferenceEquals(f1, null) && Object.ReferenceEquals(f2, null))
                return false;

            if (!(Object.ReferenceEquals(f1, null)) && Object.ReferenceEquals(f2, null))
                return true;

            if ((Object.ReferenceEquals(f1, null)) && !(Object.ReferenceEquals(f2, null)))
                return true;

            return !(f1.ToString().Equals(f2.ToString()));
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            int hashCode = 0;
            int counter = 1;
            String[] substrings = Regex.Split(validFormula, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            foreach (String s in substrings)
            {
                //This hashCode formula computers the hashcode for each character in the string and adds it to the total sum of hashcodes.
                //It then multiplies sum by the number of times the loop has iterated, ensuring an almost unique hashcode so the a/b != b/a.
                hashCode = (hashCode + s.GetHashCode()) * counter;
                counter++;
            }
            return hashCode;
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

