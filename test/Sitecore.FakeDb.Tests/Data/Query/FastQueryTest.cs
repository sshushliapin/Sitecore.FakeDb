namespace Sitecore.FakeDb.Tests.Data.Query
{
    using FluentAssertions;
    using Xunit;

    [Trait("Category", "RequireLicense")]
    public class FastQueryTest
    {
        [Fact]
        public void ShouldSelectSingleItem()
        {
            // arrange
            using (var db = new Db {new DbItem("home")})
            {
                // act
                var result = db.Database.SelectSingleItem("fast:/sitecore/content/home");

                // assert 
                result.Should().NotBeNull();
                result.Name.Should().Be("home");
            }
        }

        [Fact]
        public void ShouldSelectItems()
        {
            // arrange
            using (var db = new Db
                {
                    new DbItem("home")
                        {
                            new DbItem("child 1"),
                            new DbItem("child 2")
                        }
                })
            {
                // act
                var result = db.Database.SelectItems("fast:/sitecore/content/home/*");

                // assert 
                result.Should().HaveCount(2);
                result[0].Name.Should().Be("child 1");
                result[1].Name.Should().Be("child 2");
            }
        }

        [Fact]
        public void ShouldReturnNullIfNoSingleItemFound()
        {
            // arrange
            using (var db = new Db())
            {
                // act
                var result = db.Database.SelectSingleItem("fast:/sitecore/content/home");

                // assert 
                result.Should().BeNull();
            }
        }

        [Fact]
        public void ShouldReturnEmptyArrayIfNoItemsFound()
        {
            // arrange
            using (var db = new Db())
            {
                // act
                var result = db.Database.SelectItems("fast:/sitecore/content/*");

                // assert 
                result.Should().BeEmpty();
            }
        }
    }
}