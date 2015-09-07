using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaEvaluator{
    

    Public Static Class Evaluator{

    public delegate int Lookup(String v);
    String[] substrings = Regex.Split(s, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

        
    

      public static int Evaluate(String exp, Lookup variableEvaluator)
      {
        int x;
      }
}

}
