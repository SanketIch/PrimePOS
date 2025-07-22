using System;
using Xunit;

namespace PrimePOSTestCases
{
    public class UnitTest1
    {
        [Fact]
        //[CriticalTest]
        public void ProcessPaymentWithZeroAmount()
        {
            ProcessPayment(0, 100);
        }

        [Fact]
        [CriticalTest]
        public void ProcessPaymentWithValidAmount()
        {
            ProcessPayment(100, 200);
        }

        // This is the intentional failure test
        [Fact]
        //[CriticalTest]
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

        [Fact]
        [CriticalTest]
        public void CalculateDiscountWithValidInputs()
        {
            decimal result = CalculateDiscount(200, 10); // 10% of 200 = 20
            Assert.Equal(20, result);
        }

        [Fact]
        public void CalculateDiscountWithZeroPercent()
        {
            decimal result = CalculateDiscount(150, 0); // 0% of 150 = 0
            Assert.Equal(0, result);
        }

        [Fact]
        [CriticalTest]
        public void CalculateDiscountWithHundredPercent()
        {
            decimal result = CalculateDiscount(500, 100); // 100% of 500 = 500
            Assert.Equal(500, result);
        }

        public decimal CalculateDiscount(decimal amount, decimal discountPercent)
        {
            if (discountPercent < 0 || discountPercent > 100)
                throw new ArgumentException("Discount percent must be between 0 and 100.");

            return amount * (discountPercent / 100);
        }
    }
}
