namespace Sitecore.FakeDb.Tests.Pipelines.GetTranslation
{
  using FluentAssertions;
  using Sitecore.Pipelines.GetTranslation;
  using Xunit;

  public class GetFakeTranslationTest
  {
    [Fact]
    public void ShouldReturnTextWithAsterisk()
    {
      // arrange
      var processor = new GetFakeTranslation();
      var args = new GetTranslationArgs { Key = "My phrase" };

      // act
      processor.Process(args);

      // assert
      args.Result.Should().Be("My phrase*");
    }
  }
}