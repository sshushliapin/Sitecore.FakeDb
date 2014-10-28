namespace Sitecore.FakeDb.Tests.Pipelines.InitFakeDb
{
  using FluentAssertions;
  using Sitecore.FakeDb.Pipelines.InitFakeDb;
  using Sitecore.Pipelines;
  using Xunit;

  public class SetContextIsUnitTestingTest
  {
    [Fact]
    public void ShouldSetContextIsUnitTesting()
    {
      // arrange
      var processor = new SetContextIsUnitTesting();

      // act
      processor.Process(new PipelineArgs());

      // assert
      Context.IsUnitTesting.Should().BeTrue();
    }
  }
}