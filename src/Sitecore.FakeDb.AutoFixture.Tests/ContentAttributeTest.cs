namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Xunit;

  public class ContentAttributeTest
  {
    [Fact]
    public void ShouldBeCustomizeAttribute()
    {
      // arrange & act
      var sut = new ContentAttribute();

      // assert
      sut.Should().BeAssignableTo<CustomizeAttribute>();
    }

    [Fact]
    public void ShouldGetContentCustomization()
    {
      // arrange
      var sut = new ContentAttribute();

      // act
      var customization = sut.GetCustomization(null);

      // assert
      customization.Should().BeAssignableTo<ContentItemCustomization>();
    }
  }
}