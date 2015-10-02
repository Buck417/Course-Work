using SS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using SpreadsheetUtilities;

namespace SpreadsheetTest
{
    
    
    /// <summary>
    ///Tests for Spreadsheet class
    ///</summary>
    [TestClass()]
    public class SpreadsheetTest
    {

        /// <summary>
        /// Empty Constructor Test
        /// </summary>
        [TestInitialize]
        public void ConstructorTest()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
        }


        /// <summary>
        /// Test Constructor
        /// </summary>
        [TestMethod]
        public void ConstructorTest2()
        {
            List<string> empty = new List<string>(new Spreadsheet().GetNamesOfAllNonemptyCells());
            Assert.AreEqual(0, empty.Count);
        }

        /// <summary>
        /// Tests GetNames of a Double cell
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCellsTest()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetCellContents("A1", 1.0);
            Assert.AreEqual("A1", new List<string>(test_spreadsheet.GetNamesOfAllNonemptyCells())[0]);
            Assert.AreEqual(1, new List<string>(test_spreadsheet.GetNamesOfAllNonemptyCells()).Count);
        }
        /// <summary>
        /// Tests GetNames of a Formula Cell
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCellsTest2()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetCellContents("A1", new Formula("x+1"));
            Assert.AreEqual("A1", new List<string>(test_spreadsheet.GetNamesOfAllNonemptyCells())[0]);
            Assert.AreEqual(1, new List<string>(test_spreadsheet.GetNamesOfAllNonemptyCells()).Count);
        }
        /// <summary>
        /// Tests GetNames of a String Cell
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCellsTest3()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetCellContents("A1", "x+1");
            Assert.AreEqual("A1", new List<string>(test_spreadsheet.GetNamesOfAllNonemptyCells())[0]);
            Assert.AreEqual(1, new List<string>(test_spreadsheet.GetNamesOfAllNonemptyCells()).Count);
        }
        /// <summary>
        /// Tests GetNames after clearing a cell, setting it to empty string
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCellsTest4()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetCellContents("A1", "x+1");
            test_spreadsheet.SetCellContents("A1", "");

            Assert.AreEqual(0, new List<string>(test_spreadsheet.GetNamesOfAllNonemptyCells()).Count);
        }

        /// <summary>
        ///Larger Test for GetNames
        ///</summary>
        [TestMethod()]
        public void GetNamesOfAllNonemptyCellsTest5()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            HashSet<String> set = new HashSet<string>();
            set.Add("A1");
            set.Add("B2");
            set.Add("C3");
            test_spreadsheet.SetCellContents("A1", new Formula("1+a1"));
            test_spreadsheet.SetCellContents("B2", new Formula("a1*a1"));
            test_spreadsheet.SetCellContents("C3", 5);
            IEnumerable<string> real_set;
            real_set = test_spreadsheet.GetNamesOfAllNonemptyCells();

            foreach (String s in real_set)
                Assert.IsTrue(set.Contains(s));
        }

        /// <summary>
        /// Tests GetCellContents
        /// </summary>
        [TestMethod]
        public void GetCellContentsTest()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetCellContents("A1", 1.2);
            Assert.AreEqual(1.2, test_spreadsheet.GetCellContents("A1"));

        }

        /// <summary>
        ///Tests GetCellContents with more cells
        ///</summary>
        [TestMethod()]
        public void GetCellContentsTest2()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetCellContents("A1", "test");
            test_spreadsheet.SetCellContents("B2", new Formula("x+1"));
            test_spreadsheet.SetCellContents("C3", 2.0);

            Assert.AreEqual("test", test_spreadsheet.GetCellContents("A1"));
            Assert.AreEqual(2.0, test_spreadsheet.GetCellContents("C3"));
            Assert.AreEqual(new Formula("x+1"), test_spreadsheet.GetCellContents("B2"));
        }

        /// <summary>
        /// Tests GetCellContents Empty String
        /// </summary>
        [TestMethod]
        public void GetCellContentsTest3()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetCellContents("A1", "");

            Assert.AreEqual("", test_spreadsheet.GetCellContents("A1"));
        }


        /// <summary>
        ///Tests SetCellContents
        ///</summary>
        [TestMethod()]
        public void SetCellContentsDoubleTest()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            HashSet<String> expected_set = new HashSet<String> {"A1"};
            ISet<string> real_set = test_spreadsheet.SetCellContents("A1", 1.0);

            foreach (String s in real_set)
                Assert.IsTrue(expected_set.Contains(s));

            test_spreadsheet.SetCellContents("B2", new Formula("1+a1"));
            real_set = test_spreadsheet.SetCellContents("A1", 2.0);
            expected_set = new HashSet<String> { "A1", "B2" };

            foreach (String s in real_set)
                Assert.IsTrue(expected_set.Contains(s));

            Assert.AreEqual(2.0, (double)test_spreadsheet.GetCellContents("A1"), .0000000001);
        }

        /// <summary>
        /// Tests SetCellContents With Empty String in name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsDoubleTest2()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetCellContents("", 1.0);
        }

        /// <summary>
        /// Tests SetCellContents with Null name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsDoubleTest3()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetCellContents(null, 1.0);

        }

        /// <summary>
        ///Tests SetCellContents Invalid name
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsDoubleTest4()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();

            test_spreadsheet.SetCellContents("1_A", 1.0);
        }

        /// <summary>
        /// Tests SetCellContents acceptable underscore
        /// </summary>
        [TestMethod]
        public void GetCellContentsDoubleTest5()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetCellContents("_A1", 1.2);
            Assert.AreEqual(1.2, test_spreadsheet.GetCellContents("_A1"));

        }


        /// <summary>
        ///Tests SetCellContentsFormula
        ///</summary>
        [TestMethod()]
        public void SetCellContentsFormulaTest()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            HashSet<String> expected_set = new HashSet<String> { "A1" };
            ISet<string> real_set = test_spreadsheet.SetCellContents("A1", new Formula("x+1.0"));

            foreach (String s in real_set)
                Assert.IsTrue(expected_set.Contains(s));

            test_spreadsheet.SetCellContents("B2", new Formula("1+a1"));
            real_set = test_spreadsheet.SetCellContents("A1", new Formula("2.0 + 2.0"));
            expected_set = new HashSet<String> { "A1", "B2" };

            foreach (String s in real_set)
                Assert.IsTrue(expected_set.Contains(s));

            Assert.AreEqual(new Formula("2.0 + 2.0"), test_spreadsheet.GetCellContents("A1"));
        }

        /// <summary>
        ///Tests SetCellContents by providing an incorrect type of content to the cell
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsFormulaTest1()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            HashSet<String> expected_set = new HashSet<String> { "A1" };
            test_spreadsheet.SetCellContents("a1", "test");
            ISet<string> real_set = test_spreadsheet.SetCellContents("A1", new Formula("5+2"));

            foreach (String s in real_set)
                Assert.IsTrue(expected_set.Contains(s));

            test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetCellContents(null, new Formula("6+2"));
        }

        /// <summary>
        ///Tests SetCellContents with an invalid name
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsFormulaTest2()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetCellContents("1A", new Formula("2+2"));
        }

        /// <summary>
        ///Tests SetCellContents with a null name
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCellContentsFormulaTest3()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            Formula test = null;
            test_spreadsheet.SetCellContents("A1", test);
        }

        /// <summary>
        ///Tests SetCellContents with a cycle
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void SetCellContentsFormulaTest4()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();

            test_spreadsheet.SetCellContents("B2", new Formula("0+A1"));
            test_spreadsheet.SetCellContents("A1", new Formula("2-1"));
            test_spreadsheet.SetCellContents("C3", new Formula("B2+1"));
            test_spreadsheet.SetCellContents("A1", new Formula("C3+1"));
        }

        /// <summary>
        ///Tests SetCellContents of String
        ///</summary>
        [TestMethod()]
        public void SetCellContentsStringTest()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            ISet<string> real_set = test_spreadsheet.SetCellContents("A1", "test");
            HashSet<String> expected_set = new HashSet<String> {"A1"};

            foreach (String s in real_set)
                Assert.IsTrue(expected_set.Contains(s));

            test_spreadsheet.SetCellContents("B2", new Formula("A1+1"));
            real_set = test_spreadsheet.SetCellContents("A1", "test2");
            expected_set = new HashSet<String> { "A1", "B2" };

            foreach (String s in real_set)
                Assert.IsTrue(expected_set.Contains(s));

            Assert.AreEqual("test2", test_spreadsheet.GetCellContents("A1"));

        }


        /// <summary>
        ///Tests SetCellContents with null value
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCellContentsStringTest1()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            String temp = null;
            test_spreadsheet.SetCellContents("A1", temp);
        }
    

		/// <summary>
		/// Tests SetCellContents
		/// </summary>
		[TestMethod]
		public void SetCellContentsStringTest2()
		{
            Spreadsheet test_spreadsheet = new Spreadsheet();
			test_spreadsheet.SetCellContents("A1", "test");
			Assert.AreEqual("test", test_spreadsheet.GetCellContents("A1"));

		}
		/// <summary>
		/// Tests SetCellContents with null name.
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void SetCellContentsStringTest3()
		{
            Spreadsheet test_spreadsheet = new Spreadsheet();
			test_spreadsheet.SetCellContents(null, "test");
		}
		/// <summary>
		/// Tests SetCellContents with invalid name
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(InvalidNameException))]
		public void SetCellContentsStringTest4()
		{
            Spreadsheet test_spreadsheet = new Spreadsheet();
			test_spreadsheet.SetCellContents("2_", "test");
		}
		/// <summary>
		/// Tests SetCellContents with valid but stressful name but null value
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetCellContentsStringTest5()
		{
            Spreadsheet test_spreadsheet = new Spreadsheet();
			string test = null;
			test_spreadsheet.SetCellContents("___a300__37weert",test);
		}
		/// <summary>
		/// More Baic GetCell Tests
		/// </summary>
		[TestMethod]
		public void TestGetCellContents()
		{
            Spreadsheet test_spreadsheet = new Spreadsheet();
			test_spreadsheet.SetCellContents("A1", new Formula("B2+C3"));
			Assert.AreEqual("B2+C3", test_spreadsheet.GetCellContents("A1").ToString());

		}
	
		/// <summary>
		/// Basic setcell string test
		/// </summary>
		[TestMethod]
		public void TestSetCellContentsString()
		{
            Spreadsheet test_spreadsheet = new Spreadsheet();
			test_spreadsheet.SetCellContents("A1", "test");
			Assert.AreEqual("test", test_spreadsheet.GetCellContents("A1"));
		}
		/// <summary>
		/// Tests double set cell contents
		/// </summary>
		[TestMethod]
		public void TestSetCellContentsdouble()
		{
            Spreadsheet test_spreadsheet = new Spreadsheet();
			test_spreadsheet.SetCellContents("A1", 1.0005);
			Assert.AreEqual(1.0005, test_spreadsheet.GetCellContents("A1"));
		}
		/// <summary>
		/// Tests SetCellContents
		/// </summary>
		[TestMethod]
		public void TestSetCellContentsFormula()
		{
            Spreadsheet test_spreadsheet = new Spreadsheet();
			test_spreadsheet.SetCellContents("A1", new Formula("2+B1"));
			Assert.AreEqual("2+B1",test_spreadsheet.GetCellContents("A1").ToString());
		}
		/// <summary>
		/// Tests Set Cell Formula with null value
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestSetCellContentsFormula6()
		{
			Formula test = null;
            Spreadsheet test_spreadsheet = new Spreadsheet();
			test_spreadsheet.SetCellContents("A1", test);
		}
		/// <summary>
		/// Tests SetCellContents Formula with a replaced formula and cycle
		/// </summary>
		[TestMethod]
        [ExpectedException(typeof(CircularException))]
		public void TestSetCellContentsFormula7()
		{
            Spreadsheet test_spreadsheet = new Spreadsheet();
			test_spreadsheet.SetCellContents("A1", new Formula("4444444444444+3333333333"));
			test_spreadsheet.SetCellContents("A1", new Formula("A1+B2"));
			Assert.AreEqual("A1+B2", test_spreadsheet.GetCellContents("A1").ToString());
		}
		/// <summary>
		/// Tests SetCellFormula stress test
		/// </summary>
		[TestMethod]
		public void TestSetCellContentsFormula8()
		{
            Spreadsheet test_spreadsheet = new Spreadsheet();
			test_spreadsheet.SetCellContents("B1", new Formula("A1*10"));
			test_spreadsheet.SetCellContents("C1", new Formula("B1-2"));
			List<string> dents = new List<string>(test_spreadsheet.SetCellContents("A1",4));
			Assert.IsTrue(dents.Contains("A1"));
			Assert.IsTrue(dents.Contains("B1"));
			Assert.IsTrue(dents.Contains("C1"));
		}
        /// <summary>
        /// Tests cycles in SetCellContents Formula
        /// </summary>
		[TestMethod]
		[ExpectedException(typeof(CircularException))]
		public void SetCellContentsFormulaTestMore()
		{
            Spreadsheet test_spreadsheet = new Spreadsheet();
			test_spreadsheet.SetCellContents("B2", new Formula("A1"));
			test_spreadsheet.SetCellContents("A1", new Formula("B2"));
			Assert.IsTrue(new List<string>(test_spreadsheet.GetNamesOfAllNonemptyCells())[0] == "B1");
			Assert.IsTrue(new List<string>(test_spreadsheet.GetNamesOfAllNonemptyCells()).Count == 1);
		}


	}
}