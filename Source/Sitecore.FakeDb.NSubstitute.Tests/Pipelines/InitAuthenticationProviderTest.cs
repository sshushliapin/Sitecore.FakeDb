namespace Sitecore.FakeDb.NSubstitute.Tests.Pipelines
{
  using FluentAssertions;
  using Sitecore.FakeDb.NSubstitute.Pipelines.InitFakeDb;
  using Sitecore.Pipelines;
  using Sitecore.Security.Authentication;
  using Xunit;

  // TODO:[Med] Test it is successfully merged with the main config
  public class InitAuthenticationProviderTest
  {
    [Fact]
    public void ShouldGetActiveUser()
    {
      // arrange
      var processor = new InitAuthenticationProvider();

      // act
      processor.Process(new PipelineArgs());

      // assert
      AuthenticationManager.Provider.GetActiveUser().Name.Should().Be(@"sitecore\Anonymous");
    }
  }
}