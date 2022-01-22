namespace Marketplace.Tests
{
    public class MoneyTest
    {
        private static readonly ICurrencyLookup CurrencyLookup = new FakeCurrencyLookup();

        [Fact]
        public void MoneyObjectsWithSameAmount_ShouldBeEqual()
        {
            var firstAmount = Money.FromDecimal(5, "EUR", CurrencyLookup);
            var secondAmount = Money.FromDecimal(5, "EUR", CurrencyLookup);
            Assert.Equal(firstAmount, secondAmount);
        }

        [Fact]
        public void TwoObjectWithDifferentCurrencies_ShouldNotBeEqual()
        {
            var firstAmount = Money.FromDecimal(5, "EUR", CurrencyLookup);
            var secondAmount = Money.FromDecimal(5, "USD", CurrencyLookup);
            Assert.NotEqual(firstAmount, secondAmount);
        }

        [Fact]
        public void FromString_and_FromDecimal_should_be_equal()
        {
            var firstAmount = Money.FromDecimal(5, "EUR", CurrencyLookup);
            var secondAmount = Money.FromString("5.00", "EUR", CurrencyLookup);

            Assert.Equal(firstAmount, secondAmount);
        }

        [Fact]
        public void UnusedCurrency_ShouldThrowException()
        {
            Assert.ThrowsAny<ArgumentException>(() => Money.FromDecimal(100, "DEM", CurrencyLookup));
        }

        [Fact]
        public void UnknownCurrency_ShouldThrowException()
        {
            Assert.ThrowsAny<ArgumentException>(() => Money.FromDecimal(100, "WHAAAT?", CurrencyLookup));
        }

        [Fact]
        public void TooManyDecimalPlaces_ShouldThrowException()
        {
            Assert.ThrowsAny<ArgumentException>(() => Money.FromDecimal(100.123m, "WHAAAT?", CurrencyLookup));
        }

        [Fact]
        public void SumOfMoney_EqualsFullAmount()
        {
            var coin1 = Money.FromDecimal(1, "EUR", CurrencyLookup);
            var coin2 = Money.FromDecimal(2, "EUR", CurrencyLookup);
            var coin3 = Money.FromDecimal(2, "EUR", CurrencyLookup);
            var banknote = Money.FromDecimal(5, "EUR", CurrencyLookup);

            Assert.Equal(banknote, coin1 + coin2 + coin3);
        }

        [Fact]
        public void AddDifferentCurrencies_ShouldThrowException()
        {
            var firstAmount = Money.FromDecimal(5, "USD", CurrencyLookup);
            var secondAmount = Money.FromDecimal(5, "EUR", CurrencyLookup);

            Assert.ThrowsAny<CurrencyMismatchException>(() => firstAmount + secondAmount);
        }

        [Fact]
        public void SubtractDifferentCurrencies_ShouldThrowException()
        {
            var firstAmount = Money.FromDecimal(5, "USD", CurrencyLookup);
            var secondAmount = Money.FromDecimal(5, "EUR", CurrencyLookup);

            Assert.ThrowsAny<CurrencyMismatchException>(() => firstAmount + secondAmount);
        }
    }
}