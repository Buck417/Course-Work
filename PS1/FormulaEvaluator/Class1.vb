namespace FormulaEvaluator{

    Public Static Class Evaluator

        public delegate int Lookup(String v);

      public static int Evaluate(String exp, Lookup variableEvaluator)
      {
          String[] substrings = Regex.Split(s, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
             ...;
        int x;
      }

    End Class
    }