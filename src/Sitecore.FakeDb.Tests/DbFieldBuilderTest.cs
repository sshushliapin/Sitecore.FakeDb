namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Xunit;

  public class DbFieldBuilderTest
  {
    [Theory]
    [InlineAutoData("__Renderings", true)]
    [InlineAutoData("__Final Renderings", false)]
    public void ShouldSetFieldShared(string fieldName, bool shared)
    {
      new DbField(fieldName).Shared.Should().Be(shared);
    }

    [Fact]
    public void ShouldSetFieldType()
    {
      new DbField("__Renderings").Type.Should().Be("layout");
    }

    [Theory, DefaultSubstituteAutoData]
    public void ShouldSetInnerBuilder([Frozen]IDbFieldBuilder builder, DbFieldBuilder sut)
    {
      sut.Builder.Should().BeSameAs(builder);
    }
  }
}