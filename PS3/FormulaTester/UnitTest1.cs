using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace PS3Tests
{
    /// <summary>
    /// Unit Test class for the Formula class in the PS3 solution
    /// </summary>
    [TestClass]
    public class ForumulaTest
    {
        /*
         * Constructor without Normalizer or Validator Tests
         */

        /// <summary>
        /// Test with common formula w/no vars
        /// </summary>
        [TestMethod]
        public void TestConstructorStandard()
        {
            Formula testFormula = new Formula("4.0 + 2.0");
            Assert.AreEqual("4.0+2.0", testFormula.ToString());
        }

        /// <summary>
        /// Test with common formula with parenthesis
        /// </summary>
        [TestMethod]
        public void TestConstructorStandardParenthesis1()
        {
            Formula testFormula = new Formula("4.0 + (2.0)");
            Assert.AreEqual("4.0+(2.0)", testFormula.ToString());
        }

        /// <summary>
        /// Another common test for parenthesis
        /// </summary>
        [TestMethod]
        public void TestConstructorStandardParenthesis2()
        {
            Formula testFormula = new Formula("(4.0 + 2.0)");
            Assert.AreEqual("(4.0+2.0)", testFormula.ToString());
        }

        /// <summary>
        /// Formula test with a variable
        /// </summary>
        [TestMethod]
        public void TestConstructorStandardVariable()
        {
            Formula testFormula = new Formula("4.0 + x");
            Assert.AreEqual("4.0+x", testFormula.ToString());
        }

        /// <summary>
        /// Formula test with a variable in parenthesis
        /// </summary>
        [TestMethod]
        public void TestConstructorStandardVariable2()
        {
            Formula testFormula = new Formula("4.0 + (x)");
            Assert.AreEqual("4.0+(x)", testFormula.ToString());
        }
        
        /// <summary>
        /// Test for empty string
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorFailStandardEmptyString()
        {
            Formula testFormula = new Formula("");
        }

        /// <summary>
        /// Test for white space string
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorFailStandardWhiteSpace()
        {
            Formula testFormula = new Formula(" ");
        }

        /// <summary>
        /// Test for wrong var format
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorFailStandardWrongVariableFormat1()
        {
            Formula testFormula = new Formula("2x");
        }

        /// <summary>
        /// Common test for construction with incorrect variable
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorFailStandardWrongVariableFormat2()
        {
            Func<string, double> lookup = s => 0;
            Formula testFormula = new Formula("3x+2");
        }

        /// <summary>
        /// Test for not enough operands
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorFailStandardImproperOperator()
        {
            Formula testFormula = new Formula("+x");
        }

        /// <summary>
        /// Improper parenthesis test
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorFailStandardImproperParenthesis1()
        {
            Formula testFormula = new Formula("(x))");
        }

        /// <summary>
        /// Improper parenthesis test
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorFailStandardImproperParenthesis2()
        {
            Formula testFormula = new Formula("((x)");
        }
        
        /// <summary>
        /// Improper parenthesis test
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorFailStandardImproperParenthesis3()
        {
            Formula testFormula = new Formula("()");
        }

        /// <summary>
        /// Improper parenthesis test
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorFailStandardImproperParenthesis4()
        {
            Formula testFormula = new Formula("x)");
        }

        /// <summary>
        /// Test for no operator
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorFailStandardNoOperator()
        {
            Formula testFormula = new Formula("x 23");
        }

        /// <summary>
        /// Improper variable test
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorFailStandardWrongFormat()
        {
            Formula testFormula = new Formula("$");
        }

        /// <summary>
        /// No operands test
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorFailStandardNoOperands()
        {
            Formula testFormula = new Formula("+");
        }



        /*
         * Constructor with! Normalizer and Validator Tests
         */


        /// <summary>
        /// Common test using Formula constructor with Func's provided
        /// </summary>
        [TestMethod]
        public void TestConstructorFunc()
        {
            Formula testFormula = new Formula("3 + x", s => s.ToUpper(), s => (s == "X") ? true : false);
            Assert.AreEqual("3+X", testFormula.ToString());
        }

        /// <summary>
        /// Common test using Formula constructor with Func's provided
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorFailFunc()
        {
            Formula testFormula = new Formula("3 + x", s => s.ToLower(), s => (s == "X") ? true : false);
            Assert.AreEqual("3+X", testFormula.ToString());
        }


        /*
         * Tests for the Evaluate Method
         */


        /// <summary>
        /// Common test for evaluate method
        /// </summary>
        [TestMethod]
        public void TestEvaluate1()
        {
            Func<string, double> lookup = s => 0;
            Assert.AreEqual(4.0, new Formula("2+2").Evaluate(lookup));
        }

        /// <summary>
        /// Common test for evaluate method with variable
        /// </summary>
        [TestMethod]
        public void TestEvaluateVar1()
        {
            Func<string, double> lookup = s => 0;
            Assert.AreEqual(2.0, new Formula("x3+2").Evaluate(lookup));
        }


        /// <summary>
        /// Common test for evaluate method with variable
        /// </summary>
        [TestMethod]
        public void TestEvaluateVar2()
        {
            Func<string, double> lookup = s => 0;
            Assert.AreEqual(16.0, new Formula("(2+x)*8").Evaluate(lookup));
        }

        /// <summary>
        /// Common test for evaluate method with variable
        /// </summary>
        [TestMethod]
        public void TestEvaluateVar3()
        {
            Func<string, double> lookup = s => 0;
            Assert.AreEqual(0.0, new Formula("2*(x2*5)").Evaluate(lookup));
        }

        /// <summary>
        /// Common test for evaluate method with variable
        /// </summary>
        [TestMethod]
        public void TestEvaluateVar4()
        {
            Func<string, double> lookup = s => 4;
            Assert.AreEqual(0.5, new Formula("2/x").Evaluate(lookup));
        }

        /// <summary>
        /// Common test for evaluate method with variable
        /// </summary>
        [TestMethod]
        public void TestEvaluateVar5()
        {
            Func<string, double> lookup = s => 4;
            Assert.AreEqual(0.5, new Formula("2/_x").Evaluate(lookup));
        }

        /// <summary>
        /// Common test for evaluate method with extra long variable
        /// </summary>
        [TestMethod]
        public void TestEvaluateVar6()
        {
            Func<string, double> lookup = s => 4;
            Assert.AreEqual(0.5, new Formula("2/_x5r_9_4rly_").Evaluate(lookup));
        }

        /// <summary>
        /// Testing divide by 0, since it returns an object, check the object toString() and see if the error is correct.
        /// </summary>
        [TestMethod]
        public void TestEvaluateDivideByZero1()
        {
            Func<string, double> lookup = s => 0;
            Formula testFormula = new Formula("2/x");
            Object c = testFormula.Evaluate(lookup);
            Assert.AreEqual("SpreadsheetUtilities.FormulaError", c.ToString());
        }

        /// <summary>
        /// Testing divide by 0, since it returns an object, check the object toString() and see if the error is correct.
        /// </summary>
        [TestMethod]
        public void TestEvaluateDivideByZero2()
        {
            Func<string, double> lookup = s => -2;
            Formula testFormula = new Formula("6/(x+2)");
            Object c = testFormula.Evaluate(lookup);
            Assert.AreEqual("SpreadsheetUtilities.FormulaError", c.ToString());
        }

        /// <summary>
        /// Common test using Formula constructor with Func's provided using multiple variables
        /// and then evaluating
        /// </summary>
        [TestMethod]
        public void StressTestMultipleVariables()
        {
            Formula testFormula = new Formula("(3+x)*y", s => s.ToUpper(), s => (s == "X" || s == "Y") ? true : false);
            Func<string, double> lookup = s => (s == "X") ? 1 : 4;
            Assert.AreEqual(16.0, testFormula.Evaluate(lookup));
        }

        /*
         * GetVariables Tests
         */

        /// <summary>
        /// Common test for multiple variables
        /// </summary>
        [TestMethod]
        public void GetVariables1()
        {
            Formula testFormula = new Formula("(3+x)*y");
            foreach (string s in testFormula.GetVariables())
                Assert.IsTrue(Regex.IsMatch(s, @"[xy]"));
        }

        /// <summary>
        /// Common test for multiple variables, some multiple instances
        /// </summary>
        [TestMethod]
        public void GetVariables2()
        {
            Formula testFormula = new Formula("(3+x)*y+x");
            int i = 0;
            foreach (string s in testFormula.GetVariables()){
                i++;
                Assert.IsTrue(Regex.IsMatch(s, @"[xy]"));
            }
            Assert.AreEqual(2, i);

        }

        /// <summary>
        /// Common test for multiple variables with lambdas
        /// </summary>
        [TestMethod]
        public void GetVariables3()
        {
            Formula testFormula = new Formula("(3+x)*y", s => s.ToUpper(), s => (Regex.IsMatch(s, @"[A-Z]")));
            foreach (string s in testFormula.GetVariables())
                Assert.IsTrue(Regex.IsMatch(s, @"[XY]"));
        }

        /// <summary>
        /// Larger test for GetVariables method
        /// </summary>
        [TestMethod()]
        public void GetVariables4()
        {
            Formula testFormula = new Formula("a1+b2*c3+d4/A2-Z7+a1/y8");
            List<string> variables = new List<string>(testFormula.GetVariables());
            HashSet<string> expected = new HashSet<string>() { "a1", "b2", "c3", "d4", "A2", "Z7", "y8" };
            Assert.AreEqual(variables.Count, 7);
            Assert.IsTrue(expected.SetEquals(variables));
        }


        /*
         * ToString Tests
         */


        /// <summary>
        /// Simple test for the ToString method
        /// </summary>
        [TestMethod()]
        public void ToStringTest()
        {
            Formula testFormula = new Formula("4+35");
            Assert.IsTrue(testFormula.Equals(new Formula(testFormula.ToString())));
        }


        
        /*
         * Equals Tests
         */

        /// <summary>
        /// Common test for equal objects
        /// </summary>
        [TestMethod]
        public void TestEqualsSameObjectType()
        {
            Formula testFormula1 = new Formula("a/b");
            Formula testFormula2 = new Formula("a/b");
            Assert.IsTrue(testFormula1.Equals(testFormula2));
        }

        /// <summary>
        /// Common test for not equal objects
        /// </summary>
        [TestMethod]
        public void TestEqualsNotSameObjectType()
        {
            String testString = "test";
            Formula testFormula2 = new Formula("a/b");
            Assert.IsFalse(testString.Equals(testFormula2));
        }


        /*
         * == and != Operator Tests
         */


        /// <summary>
        /// Common test for equal objects using ==
        /// </summary>
        [TestMethod]
        public void TestOperator1()
        {
            Formula testFormula1 = new Formula("a/b");
            Formula testFormula2 = new Formula("a/b");
            Assert.IsTrue(testFormula2 == testFormula1);
        }

        /// <summary>
        /// Common test for not equal objects using ==
        /// </summary>
        [TestMethod]
        public void TestOperator2()
        {
            Formula testFormula1 = new Formula("a/b");
            Formula testFormula2 = null;
            Assert.IsFalse(testFormula2 == testFormula1);
        }

        /// <summary>
        /// Common test for equal null objects using ==
        /// </summary>
        [TestMethod]
        public void TestOperator3()
        {
            Formula testFormula1 = null;
            Formula testFormula2 = null;
            Assert.IsTrue(testFormula2 == testFormula1);
        }

        /// <summary>
        /// Common test for equal objects using !=
        /// </summary>
        [TestMethod]
        public void TestOperator4()
        {
            Formula testFormula1 = new Formula("a/b");
            Formula testFormula2 = new Formula("a/b");
            Assert.IsFalse(testFormula2 != testFormula1);
        }

        /// <summary>
        /// Common test for equal objects using !=
        /// </summary>
        [TestMethod]
        public void TestOperator5()
        {
            Formula testFormula1 = null;
            Formula testFormula2 = new Formula("a/b");
            Assert.IsTrue(testFormula2 != testFormula1);
        }

        /// <summary>
        /// Common test for equal objects using !=
        /// </summary>
        [TestMethod]
        public void TestOperator6()
        {
            Formula testFormula1 = null;
            Formula testFormula2 = null;
            Assert.IsFalse(testFormula2 != testFormula1);
        }



        /*
         * GetHashCode Tests
         */


        /// <summary>
        /// Common test for what should be equal hashcodes
        /// </summary>
        [TestMethod]
        public void TestGetHashCode1()
        {
            Formula testFormula1 = new Formula("a/b");
            Formula testFormula2 = new Formula("a/b");
            Assert.IsTrue(testFormula1.GetHashCode() == testFormula2.GetHashCode());
        }

        /// <summary>
        /// Common test for what should be not equal hashcodes
        /// </summary>
        [TestMethod]
        public void TestGetHashCode2()
        {
            Formula testFormula1 = new Formula("a/b");
            Formula testFormula2 = new Formula("b/a");
            Assert.IsFalse(testFormula1.GetHashCode() == testFormula2.GetHashCode());
        }
    }
}
