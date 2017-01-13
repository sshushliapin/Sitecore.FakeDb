namespace Sitecore.FakeDb.Tests
{
    using FluentAssertions;
    using Ploeh.AutoFixture.Xunit2;
    using Sitecore.Collections;
    using Xunit;

    public class DbLanguagesTest
    {
        [Theory, AutoData]
        public void GetLanguagesReturnsValidLanguages(
          LanguageCollection languages)
        {
            var sut = new DbLanguages(languages);
            sut.GetLanguages()
              .ShouldAllBeEquivalentTo(languages);
        }
    }
}