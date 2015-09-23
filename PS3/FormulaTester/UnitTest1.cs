using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

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
            Assert.AreEqual("4.0+x", testFormula.ToString());
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
        public void TestConstructorFailStandardWrongVariableFormat()
        {
            Formula testFormula = new Formula("2x");
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
    }
}
