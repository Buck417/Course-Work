using SS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using SpreadsheetUtilities;
using System.Threading;
using System.Xml;

namespace SpreadsheetTester
{


    /// <summary>
    ///Tests for PS5 version of Spreadsheet class
    ///</summary>
    [TestClass()]
    public class SpreadsheetTests
    {
        /// <summary>
        /// Tests if cell name for GetCellValue is invalid
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Testgetcellvalue1()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            object value = test_spreadsheet.GetCellValue("-1");
        }

        /// <summary>
        /// Tests if cell name for GetCellValue is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void Testgetcellvalue2()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.GetCellValue(null);
        }

        /// <summary>
        /// Tests SetContentsOfCell with null content
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetContentsOfCell1()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetContentsOfCell("A1", null);
        }

        /// <summary>
        /// Tests SetContentsOfCell with null name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetContentsOfCell2()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetContentsOfCell(null, "1");
        }

        /// <summary>
        /// Tests SetContentsOfCell with null name and null content
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetContentsOfCell3()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetContentsOfCell(null, null);
        }

        /// <summary>
        /// Tests SetContentsOfCell with invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetContentsOfCell4()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetContentsOfCell("1A", "1");
        }

        /// <summary>
        /// Tests SetContentsOfCell for an circular exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestSetContentsOfCell5()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetContentsOfCell("A1", "=A1");
        }

        /// <summary>
        ///  Tests Get and Set Contents with a double
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCell6()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetContentsOfCell("A1", "1");
            object value = test_spreadsheet.GetCellValue("A1");
            Assert.AreEqual(1.0, value);
        }

        /// <summary>
        ///    Tests Get and Set contents with a formula
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCell7()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet(s => true, s => s, "default");
            test_spreadsheet.SetContentsOfCell("B1", "1.0");
            test_spreadsheet.SetContentsOfCell("C1", "5.0");
            test_spreadsheet.SetContentsOfCell("A1", "=B1 + C1");
            object value = test_spreadsheet.GetCellValue("A1");
            Assert.AreEqual(6.0, value);
        }

        // Tests the IsValid delegate
        [TestMethod()]
        public void IsValidTest()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "1");
        }

        //Another IsValid delegate test
        [TestMethod()]
        public void IsValidTest1()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetContentsOfCell("A1", "= B1 - C1");
        }



        ///<summary>
        /// Tests null filepath 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSave1()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.Save(null);
        }

        /// <summary>
        /// Tests constructor with a invalid file path
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestConstructor1()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet("C:\\Users\\Ryan\\Desktop\\", s => true, s => s, "");
        }


        // Tests the Normalize delegate
        [TestMethod()]
        public void NormalizeTest()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetContentsOfCell("A1", "1");
            Assert.AreEqual("", test_spreadsheet.GetCellContents("B1"));
        }


        //Another Normalize delegate test
        [TestMethod()]
        public void NormalizeTest1()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet(s => true, s => s.ToUpper(), "");
            test_spreadsheet.SetContentsOfCell("B1", "x");
            Assert.AreEqual("x", test_spreadsheet.GetCellContents("B1"));
        }


        // Tests empty content
        [TestMethod()]
        public void EmptySheet()
        {
            Spreadsheet test_spreadsheet = new Spreadsheet();
            test_spreadsheet.SetContentsOfCell("A1", "");
        }



    }
}