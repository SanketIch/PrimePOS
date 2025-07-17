using System;
using Xunit;

namespace PrimePOSTestCases
{
    public class UnitTest1
    {
        [Fact]
        public void ProcessPaymentWithZeroAmount()
        {
            ProcessPayment(0, 100);
        }

        [Fact]
        public void ProcessPaymentWithValidAmount()
        {
            ProcessPayment(100, 200);
        }

        // This is the intentional failure test
        [Fact]
        [CriticalTest]
        public void IntentionalFailureTest()
        {
            int expected = 100;
            int actual = 40;
            Assert.Equal(expected, actual);  // This will always fail
        }

        public bool ProcessPayment(decimal amount, decimal balance)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");

            if (balance >= amount)
                return true;

            return false;
        }
    }
}
