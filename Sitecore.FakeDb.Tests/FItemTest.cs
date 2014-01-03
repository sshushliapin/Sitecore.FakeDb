namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Xunit;

  public class FItemTest
  {
    [Fact]
    public void ShouldGenerateNewIdsIfNotSet()
    {
      // arrange
      var item = new FItem("my item");

      // act & assert
      item.ID.Should().NotBeNull();
      item.TemplateID.Should().NotBeNull();
    }
  }
}