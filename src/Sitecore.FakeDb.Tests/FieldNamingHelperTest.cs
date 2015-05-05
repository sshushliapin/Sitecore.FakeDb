namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Ploeh.AutoFixture;
  using Sitecore.Data;
  using Xunit;

  public class FieldNamingHelperTest
  {
    [Fact]
    public void ShouldReturnSuggestedIdNamePair()
    {
      // arrange
      var fixture = new Fixture();
      var id = fixture.Create<ID>();
      var name = fixture.Create<string>();

      var namingHelper = new FieldNamingHelper();

      // act
      var pair = namingHelper.GetFieldIdNamePair(id, name);

      // assert
      pair.Key.Should().BeSameAs(id);
      pair.Value.Should().Be(name);
    }
  }
}