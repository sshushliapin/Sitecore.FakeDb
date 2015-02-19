namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit;
  using Sitecore.Data;
  using Xunit.Extensions;

  public class FieldNamingHelperTest
  {
    [Theory]
    [AutoData]
    public void ShouldReturnSuggestedIdNamePair(ID id, string name)
    {
      // arrange
      var namingHelper = new FieldNamingHelper();

      // act
      var pair = namingHelper.GetFieldIdNamePair(id, name);

      // assert
      pair.Key.Should().BeSameAs(id);
      pair.Value.Should().Be(name);
    }
  }
}