namespace Sitecore.FakeDb.Tests.Links
{
    using FluentAssertions;
    using Sitecore.Links;
    using Xunit;

    [Trait("Category", "RequireLicense")]
    public class LinkManagerTest
    {
        [Fact]
        public void ShouldGetItemUrl()
        {
            // arrange
            using (var db = new Db {new DbItem("home")})
            {
                var item = db.GetItem("/sitecore/content/home");

                // act & assert
                LinkManager.GetItemUrl(item).Should().Be("/en/sitecore/content/home.aspx");
            }
        }
    }
}