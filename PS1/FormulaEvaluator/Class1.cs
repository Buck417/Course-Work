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
        Stack<String> operators = new Stack<String>();
        String[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
        int val;

        for (int i = 0; i < substrings.Length; i++)
        {
            String instance = substrings[i];
            int length = substrings[i].Length;
            if (substrings[i] == " ")
            {
                
                i++;
            }
            if(instance == "*" || instance == "+" || instance == "(" || instance == "-" || instance == "/"){
                operators.Push(instance);
            }
            if (int.TryParse(instance, out val))
            {
                numbers.Push(val);
            }
            if(length > 1){

            }
        }
        int x = 0;
        return x;
      }

      public static Boolean Variable(String substringGiven)
      {
          String[] varSubString;
          string[] stringSeparators = new string[] {""};
          varSubString = substringGiven.Split(stringSeparators, StringSplitOptions.None);
          int val;
          if (Regex.IsMatch(substringGiven, @"^[a-zA-Z0-9]+$")){
              if((Regex.IsMatch(varSubString[0], @"^[a-zA-Z]+$") && (int.TryParse(varSubString[varSubString.Length], out val)){
                  return true;
              }
          }
          

              return false;
      }

}

}
