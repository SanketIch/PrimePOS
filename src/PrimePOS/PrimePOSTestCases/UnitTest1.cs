using System;
using Xunit;

namespace PrimePOSTestCases
{
    public class UnitTest1
    {
        [Fact]
        public void Addition_ReturnsCorrectResult()
        {
            int a = 5;
            int b = 3;
            int sum = a + b;

            Assert.Equal(8, sum);
        }

        [Fact]
        public void StringContainsSubstring()
        {
            string message = "Welcome to PrimePOS System";
            Assert.Contains("PrimePOS", message);
        }
    }
}
