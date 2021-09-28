using FluentAssertions;
using Math;
using System;
using System.Collections.Generic;
using Xunit;

namespace MathTests
{
    public class CalculatorTests
    {
        Calculator calculator;

        public CalculatorTests()
        {
            calculator = new Calculator();
        }

        [Fact]
        public void Multiply_ShallReturn_MultiplicationResult()
        {
            //Arrange
            int[] values = { 10, 5 };
            var expected = 50;

            //Act
            var actual = calculator.Multiply(values);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(5, 5, 10)]
        [InlineData(10, 5, 15)]
        [InlineData(50, 12, 62)]
        [InlineData(1, 0, 1)]
        public void Sum_ShallReturn_SumResult(int v1, int v2, int result)
        {
            //Arrange
            int n1 = v1;
            int n2 = v2;
            int expected = result;

            //Act
            var actual = calculator.Sum(new[] { n1, n2 });

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Divide_WhenDividingByNonZero_ShallReturn_DivisionResult()
        {
            //Arrange
            int v1 = 10;
            int v2 = 2;
            decimal expected = 5;

            //Act
            var actual = calculator.Divide(v1, v2);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Divide_WhenDividingByZero_ShallThrowException()
        {
            //Arrange
            int v1 = 10;
            int v2 = 0;

            //Act
            var actual = Assert.Throws<DivideByZeroException>(
                () => calculator.Divide(v1, v2));

            //Assert
            Assert.NotNull(actual);
            Assert.IsType<DivideByZeroException>(actual);
        }
    }
}

