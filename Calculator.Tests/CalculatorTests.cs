using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator;
using NUnit.Framework;

namespace Calculator.Tests
{
    [TestFixture]
    public class CalculatorTests
    {
        [Test]
        public void Add_AddsTwoPositiveNumbers_Calculated()
        {
            var calc = new Calculator();
            int sum = calc.Add(2, 2);
            Assert.AreEqual(4, sum);
        }
        [Test]
        public void Add_AddsPositiveNumberToNegativeNumber_Calculated()
        {
            var calc = new Calculator();
            int sum = calc.Add(2, -4);
            Assert.AreEqual(-2, sum);
        }
        [Test]
        public void Add_AddsNegativeNumberToPositiveNumber_Calculated()
        {
            var calc = new Calculator();
            int sum = calc.Add(-10, 2);
            Assert.AreEqual(-8, sum);
        }
        [Test]
        public void Add_AddsTwoNegativeNumbers_Calculated()
        {
            var calc = new Calculator();
            int sum = calc.Add(-7, -2);
            Assert.AreEqual(-9, sum);
        }

        [TestCase(4, 2, 2.0f)]
        [TestCase(-4, 2, -2.0f)]
        [TestCase(4, -2, -2.0f)]
        [TestCase(0, 3, 0.0f)]
        [TestCase(5, 2, 2.5f)]
        [TestCase(1, 3, 0.333333343f)]
        public void Divide_ReturnsProperValue(int dividend, int divisor, float expectedQuotient)
        {
            var calc = new Calculator();
            var quotient = calc.Divide(dividend, divisor);
            Assert.AreEqual(expectedQuotient, quotient);
        }

        [Test]
        public void Divide_DivisionByZero_ThrowsException()
        {
            var calc = new Calculator();
            Assert.Throws<DivideByZeroException>(() => calc.Divide(2, 0));
        }

        [Test]
        public void Divide_OnCalculatedEventIsCalled()
        {
            var calc = new Calculator();

            bool wasEventCalled = false;
            calc.CalculatedEvent += (sender, args) => wasEventCalled = true;

            calc.Divide(1, 2);

            Assert.IsTrue(wasEventCalled);
        }
        
        [Test]
        public void Divide_DividendIsZero_ReturnsQuotientEqualToZero(
            [Values(-2, -1, 1, 2)] int divisor)
        {
            var calc = new Calculator();
            float quotient = calc.Divide(0, divisor);

            Assert.AreEqual(0, quotient);
        }

        [Test]
        public void Divide_DividendAndDivisorAreRandomPositiveNumbers_ReturnsPositiveQuotient(
            [Random(min: 1, max: 100, count: 1)] int dividend,
            [Random(min: 1, max: 100, count: 10)] int divisor)
        {
            var calc = new Calculator();
            float quotient = calc.Divide(dividend, divisor);
            Assert.That(quotient > 0);
        }

        [Test]
        [Combinatorial]
        public void Divide_DividendIsPositiveAndDivisorIsNegative_ReturnsNegativeQuotient(
            [Values(1, 2, 3, 4)] int dividend,
            [Values(-1, -2, -3)] int divisor)
        {
            var calc = new Calculator();
            float quotient = calc.Divide(dividend, divisor);

            Assert.That(quotient < 0);
        }

        [Test]
        [Sequential]
        public void Divide_DivisorIsNegativeOfDividend_ReturnsMinusOne(
            [Values(1, 2, 30)] int dividend,
            [Values(-1, -2, -30)] int divisor)
        {
            var calc = new Calculator();
            float quotient = calc.Divide(dividend, divisor);

            Assert.That(quotient == -1);
        }
    }
}


[TestFixture(typeof (int))]
[TestFixture(typeof (float))]
[TestFixture(typeof (double))]
[TestFixture(typeof (decimal))]
public class GenericCalculatorTests<T>
{
    [Test]
    public void AdditionTest()
    {
        var calculator = new GenericCalculator<T>();

        dynamic result = calculator.Add((dynamic) 2, (dynamic) 3);

        Assert.That(result, Is.EqualTo(5));
    }
}

[TestFixture(typeof(ArrayList))]
[TestFixture(typeof(List<int>))]
[TestFixture(typeof(Collection<int>))]
public class ListsTests<T> where T : IList, new()
{
    [Test]
    public void CountTest()
    {
        var list = new T { 2, 3 };

        Assert.That(list, Has.Count.EqualTo(2));
    }
}

public class Theory
{
    [Datapoint]
    public int Negative = -1;

    [Datapoint]
    public int Positive = 1;

    [Theory]
    public void WhenDividendIsPositiveAndDivisorIsNegative_TheQuotientIsNegative(int dividend,
        int divisor)
    {
        Assume.That(dividend > 0);
        Assume.That(divisor < 0);

        var calculator = new Calculator.Calculator();

        float quotient = calculator.Divide(dividend, divisor);

        Assert.That(quotient < 0);
    }
}