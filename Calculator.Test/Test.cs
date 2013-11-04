using System;
using System.Reflection.Emit;
using Calculator.Application;
using NUnit.Framework;


namespace Calculator.Test
{
    [TestFixture]
    public class Test
    {
        private ICalculator _calculator;

        [SetUp]
        public void Initialize()
        {
            _calculator = new Calculator.Application.Calculator();
        }
        [Test]
        public void AnEmptyStringRetunsZero()
        {
            Assert.That(_calculator.Add(""),Is.EqualTo(0));
        }
        [TestCase ("0",0)]
        [TestCase ("1",1)]
        [TestCase ("2",2)]
        [TestCase ("3",3)]
        [TestCase ("4",4)]
        public void OneNumberIsReturnsTheSameValue(string input,int expected)
        {
            Assert.That(_calculator.Add(input),Is.EqualTo(expected));
        }
        [TestCase ("1,2",3)]
        [TestCase ("1,2,3",6)]
        public void TwoNumbersSeparetedByCommaRetunsTheSum(string input,int expected)
        {
            Assert.That(_calculator.Add(input),Is.EqualTo(expected));
        }
        [TestCase ("1\n2",3)]
        [TestCase ("3\n4",7)]
        [TestCase ("7\n8",15)]
        public void TwoNumbersSeparatedByNewLineReturnsTheSum(string input, int expected)
        {
            Assert.That(_calculator.Add(input),Is.EqualTo(expected));
        }
        [TestCase("//;\n1;2;3", 6)]
        [TestCase("//;\n6;7;8", 21)]
        [TestCase("//;\n21;22;23", 66)]
        public void NumbersSepartedByCustomDelimeterReturnsTheSum(string input, int expected)
        {
            Assert.That(_calculator.Add(input),Is.EqualTo(expected));
        }
        [TestCase("//[***]\n1***2***3", 6)]
        public void CanHandleLongLegnthDelimiters(string input, int expected)
        {
            Assert.That(_calculator.Add(input),Is.EqualTo(expected));
        }
        [TestCase("1001,2", 2)]
        [TestCase("10,1002\n3", 13)]
        [TestCase("//;\n1;1005;2", 3)]
        [TestCase("//[**]\n10**1020**10", 20)]
        public void NumbersGreaterThatnAThousandIsIgnored(string input, int expected)
        {
            Assert.That(_calculator.Add(input),Is.EqualTo(expected));
        }
        [TestCase("//[%][#][@]\n1%2#3@4", 10)]
        [TestCase("//[*][&][^]\n1*2&3^4", 10)]
        [TestCase("//[%$][#!][@^]\n1%$2#!3@^4", 10)]
        public void CanHandleMultipleDelimiters(string input, int expected)
        {
            Assert.That(_calculator.Add(input),Is.EqualTo(expected));
        }
        [TestCase("-1,2", "Negative numbers not allowed: -1")]
        [TestCase("//;\n-1;2;-5", "Negative numbers not allowed: -1,-5")]
        [TestCase("//[***]\n-1***10", "Negative numbers not allowed: -1")]
        [TestCase("//[***]\n-1***10***-5", "Negative numbers not allowed: -1,-5")]
        public void NegativeValuesReturnsExceptionMessage(string input, string expected)
        {
            Assert.Throws(
                Is.TypeOf<NegativeNumberException>()
                    .And.Message.EqualTo(expected), () => _calculator.Add(input));
        }
         
    }
}