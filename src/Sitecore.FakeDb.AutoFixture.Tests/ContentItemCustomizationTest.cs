namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using System;
  using FluentAssertions;
  using Xunit;

  public class ContentItemCustomizationTest
  {
    [Fact]
    public void ShouldThrowIfFeatureIsNull()
    {
      // arrange
      var sut = new ContentItemCustomization();

      // act
      Action action = () => sut.Customize(null);

      // assert
      action.ShouldThrow<ArgumentNullException>()
            .WithMessage("Value cannot be null.\r\nParameter name: fixture");
    }
  }
}