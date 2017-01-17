namespace Sitecore.FakeDb.Tests
{
    using System;
    using FluentAssertions;
    using Sitecore.Collections;
    using Sitecore.Globalization;
    using Xunit;

    public class DbLanguagesTest
    {
        [Fact]
        public void InstantiateWithNullLanguagesThrows()
        {
            Action action = () => new DbLanguages(null);
            action.ShouldThrow<ArgumentNullException>()
                .And.ParamName.Should().Be("languages");
        }

        [Fact]
        public void GetLanguagesReturnsValidLanguageCollection()
        {
            var en = Language.Parse("en");
            var da = Language.Parse("da");
            var expected = new LanguageCollection { en, da };
            var sut = new DbLanguages(en, da);

            var actual = sut.GetLanguages();

            actual.ShouldAllBeEquivalentTo(expected);
        }
    }
}