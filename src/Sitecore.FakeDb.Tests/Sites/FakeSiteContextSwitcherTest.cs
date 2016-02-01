namespace Sitecore.FakeDb.Tests.Sites
{
    using FluentAssertions;
    using Sitecore.Configuration;
    using Sitecore.FakeDb.Sites;
    using Sitecore.Sites;
    using Xunit;

    public class FakeSiteContextSwitcherTest
    {
        [Fact]
        public void ShouldSwitchContextSite()
        {
            // arrange
            var site = new FakeSiteContext("mywebsite");

            // act
            using (new FakeSiteContextSwitcher(site))
            {
                // assert
                Context.Site.Name.Should().Be("mywebsite");
                Factory.GetSite("mywebsite").Name.Should().Be("mywebsite");
            }

            Context.Site.Should().BeNull();
            Factory.GetSite("mywebsite").Should().BeNull();
        }

        [Fact]
        public void ShouldPreserveExistingSites()
        {
            // arrange
            var existingSite = new FakeSiteContext("existing");
            SiteContextFactory.Sites.Add(existingSite.SiteInfo);

            var site = new FakeSiteContext("mywebsite");

            // act
            using (new FakeSiteContextSwitcher(site))
            {
                // assert
                Context.Site.Name.Should().Be("mywebsite");
                Factory.GetSite("existing").Should().NotBeNull();
            }

            Context.Site.Should().BeNull();
            Factory.GetSite("mywebsite").Should().BeNull();
            Factory.GetSite("existing").Should().NotBeNull();
        }
    }
}
