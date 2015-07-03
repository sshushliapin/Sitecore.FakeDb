namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using FluentAssertions;
  using Ploeh.AutoFixture;
  using Xunit;

  public class ContentItemCustomizationTest
  {
    [Fact(Skip = "To be fixed.")]
    public void ShouldAddBuilder()
    {
      // arrange
      var fixture = new Fixture();

      var sut = new ContentItemCustomization();

      // act
      sut.Customize(fixture);

      // assert
      fixture.Customizations.Should().Contain(x => x.GetType() == typeof(ContentItemGenerator));
    }
  }
}