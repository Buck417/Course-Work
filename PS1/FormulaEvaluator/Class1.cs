using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaEvaluator{
    

    public static class Evaluator{

        public delegate int Lookup(String v);
       

        
    
/// <summary>
/// A method that uses stacks to separate a string formula and perform operations on the values according to
/// standard order of operations in arithmatic and return the value of the formula if the formula is valid.
/// Throws an Argument Exception if the formula is invalid.
/// </summary>
/// <param name="exp"> A formula used to evaluate. </param>
/// <param name="variableEvaluator"> A delegate that takes a string and returns the string value, used for variables in a formula. </param>
/// <returns>Returns an int value of the solved formula. </returns>
      public static int Evaluate(String exp, Lookup variableEvaluator){
        Stack<int> numbers = new Stack<int>();
        Stack<char> operators = new Stack<char>();
        String[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
        int x = 0;
        return x;
      }
}

}
