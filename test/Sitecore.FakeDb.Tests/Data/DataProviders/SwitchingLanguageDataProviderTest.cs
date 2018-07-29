namespace Sitecore.FakeDb.Tests.Data.DataProviders
{
    using FluentAssertions;
    using global::AutoFixture.Xunit2;
    using Sitecore.Common;
    using Sitecore.Data.DataProviders;
    using Sitecore.FakeDb.Data.DataProviders;
    using Sitecore.Globalization;
    using Xunit;
    using CallContext = Sitecore.Data.DataProviders.CallContext;

    public class SwitchingLanguageDataProviderTest
    {
        [Theory, AutoData]
        public void SutIsDataProvider(SwitchingLanguageDataProvider sut)
        {
            sut.Should().BeAssignableTo<DataProvider>();
        }

        [Theory, DefaultAutoData]
        public void GetLanguagesReturnsEmptyCollectionIfNoLanguagesSwitched(
            SwitchingLanguageDataProvider sut,
            CallContext context)
        {
            sut.GetLanguages(context)
                .Should().BeEmpty();
        }

        [Theory, DefaultAutoData]
        public void GetLanguagesReturnsLanguagesIfSwitched(
            SwitchingLanguageDataProvider sut,
            CallContext context)
        {
            var en = Language.Parse("en");
            var da = Language.Parse("da");
            var contextLanguages = new DbLanguages(en, da);
            using (new Switcher<DbLanguages>(contextLanguages))
            {
                sut.GetLanguages(context)
                    .ShouldAllBeEquivalentTo(new[] {en, da});
            }
        }
    }
}