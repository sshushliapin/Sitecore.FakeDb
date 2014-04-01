namespace Sitecore.FakeDb.NSubstitute.Tests
{
  using System.Configuration.Provider;
  using FluentAssertions;
  using Sitecore.Security.AccessControl;
  using Xunit;

  public class NSubstituteFactoryTest
  {
    private readonly NSubstituteFactory factory;

    public NSubstituteFactoryTest()
    {
      this.factory = new NSubstituteFactory();
    }

    [Fact]
    public void ShouldCreateSubstituteForType()
    {
      // arrange
      var type = "Sitecore.Security.AccessControl.AuthorizationProvider, Sitecore.Kernel";

      // act
      var obj = this.factory.GetObject(type);

      // assert
      obj.Should().BeAssignableTo<AuthorizationProvider>();
    }

    [Fact]
    public void ShouldSetProviderNameIfTypeIsProvider()
    {
      // arrange
      var providerType = "System.Configuration.Provider.ProviderBase, System.Configuration";

      // act
      var providerMock = (ProviderBase)this.factory.GetObject(providerType);

      // assert
      providerMock.Name.Should().Be("ProviderBase");
    }
  }
}