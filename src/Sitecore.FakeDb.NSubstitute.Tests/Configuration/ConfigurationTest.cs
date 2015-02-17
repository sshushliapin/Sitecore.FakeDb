namespace Sitecore.FakeDb.NSubstitute.Tests.Configuration
{
  using FluentAssertions;
  using Sitecore.Configuration;
  using Xunit;

  public class ConfigurationTest
  {
    [Fact]
    public void ShouldResolveNSubstituteFactory()
    {
      // act & assert
      Factory.CreateObject("factories/factory", true).Should().BeOfType<NSubstituteFactory>();
    }
  }
}