namespace Sitecore.FakeDb.Tests.AutoFixture
{
  using FluentAssertions;
  using global::AutoFixture.Xunit2;
  using Xunit;

  [Trait("Category", "RequireLicense")]
  public class AutoFixtrureTest
  {
    [Theory, AutoData]
    public void ShouldCreateDb(Db db)
    {
      // arrange
      db.Add(new DbItem("home"));

      // act & assert
      db.GetItem("/sitecore/content/home").Should().NotBeNull();
    }
  }
}