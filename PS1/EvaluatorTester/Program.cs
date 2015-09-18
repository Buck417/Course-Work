using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FormulaEvaluator;


namespace EvaluatorTester
{
    class Program
    {
        
        static void Main(string[] args)
        {

            String x = " (10-2)/4 - 1*500";
             int answer =  Evaluator.Evaluate(x, s => 2);
             int answer2 = Evaluator.Evaluate("2+3*5+(3+4*8)*5+2", s => 0);

        }
    }
}
