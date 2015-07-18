namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data;
  using Xunit;

  public class FieldNamingHelperTest
  {
    [Theory, AutoData]
    public void ShouldReturnSuggestedIdNamePair(FieldNamingHelper sut, ID id, string name)
    {
      // act
      var result = sut.GetFieldIdNamePair(id, name);

      // assert
      result.Key.Should().BeSameAs(id);
      result.Value.Should().Be(name);
    }

    [Theory, AutoData]
    public void ShouldTryToSetFieldIdFromParsedNameIfItIsId(FieldNamingHelper sut, ID id)
    {
      // arrange
      var name = id.ToString();

      // act
      var result = sut.GetFieldIdNamePair(ID.Null, name);

      // assert
      result.Key.Should().Be(id);
      result.Value.Should().Be(name);
    }
  }
}