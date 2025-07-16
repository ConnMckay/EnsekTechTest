using EnsekTechTest.Abstractions.Errors;
using EnsekTechTest.TestData;
using FluentAssertions;
using System.Diagnostics;
using Xunit;

namespace EnsekTechTest.Tests
{
    public class CalculatorTests
    {
        [Theory]
        [ClassData(typeof(CalculatorMultiplierData))]
        public void Test_multipliers(int first, int second, int result)
        {

            int multiplied = first * second;
            multiplied.Should().Be(result);
        }
        [Theory]
        [ClassData(typeof(CalculatorDivisionData))]
        public void Test_divisions(int first, int second, int result)
        {
            try
            {
                int multiplied = first / second;
                multiplied.Should().Be(result);
            }
            catch (DivideByZeroException)
            {
                Debug.WriteLine(CalculatorErrors.DivideByZero);
                //throw;
            }
        }
    }
}
