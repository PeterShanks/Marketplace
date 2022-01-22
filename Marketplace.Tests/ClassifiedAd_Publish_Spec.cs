namespace Marketplace.Tests
{
    public class ClassifiedAd_Publish_Spec
    {
        private readonly ClassifiedAd _classifiedAd;

        public ClassifiedAd_Publish_Spec()
        {
            _classifiedAd = new ClassifiedAd(
                new ClassifiedAdId(Guid.NewGuid()),
                new UserId(Guid.NewGuid()));
        }

        [Fact]
        public void ValidAd_CanPublish()
        {
            _classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Test ad"));
            _classifiedAd.UpdateText(ClassifiedAdText.FromString("Please buy my stuff"));
            _classifiedAd.UpdatePrice(Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup()));

            _classifiedAd.RequestToPublish();

            Assert.Equal(ClassifiedAdState.PendingReview, _classifiedAd.State);
        }

        [Fact]
        public void ClassifiedAdWithoutTitle_OnPublish_ShouldThrowException()
        {
            _classifiedAd.UpdateText(ClassifiedAdText.FromString("Please buy my stuff"));
            _classifiedAd.UpdatePrice(
                Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup()));

            Assert.Throws<InvalidEntityStateException>(() => _classifiedAd.RequestToPublish());
        }

        [Fact]
        public void ClassifiedAdWithoutText_OnPublish_ShouldThrowException()
        {
            _classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Test ad"));
            _classifiedAd.UpdatePrice(
                Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup()));

            Assert.Throws<InvalidEntityStateException>(() => _classifiedAd.RequestToPublish());
        }

        [Fact]
        public void ClassifiedAdWithoutPrice_OnPublish_ShouldThrowException()
        {
            _classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Test ad"));
            _classifiedAd.UpdateText(ClassifiedAdText.FromString("Please buy my stuff"));

            Assert.Throws<InvalidEntityStateException>(() => _classifiedAd.RequestToPublish());
        }

        [Fact]
        public void ClassifiedAdWithZeroPrice_OnPublish_ShouldThrowException()
        {
            _classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Test ad"));
            _classifiedAd.UpdateText(ClassifiedAdText.FromString("Please buy my stuff"));
            _classifiedAd.UpdatePrice(
                Price.FromDecimal(0.0m, "EUR", new FakeCurrencyLookup()));

            Assert.Throws<InvalidEntityStateException>(() => _classifiedAd.RequestToPublish());
        }
    }
}