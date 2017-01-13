namespace Sitecore.FakeDb.Tests.Data.DataProviders
{
    using FluentAssertions;
    using Ploeh.AutoFixture.Xunit2;
    using Sitecore.Collections;
    using Sitecore.Common;
    using Sitecore.Data.DataProviders;
    using Sitecore.FakeDb.Data.DataProviders;
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
          CallContext context,
          LanguageCollection languages)
        {
            var contextLanguages = new DbLanguages(languages);
            using (new Switcher<DbLanguages>(contextLanguages))
            {
                sut.GetLanguages(context)
                  .Should().BeSameAs(languages);
            }
        }
    }
}