namespace Sitecore.FakeDb.NSubstitute.Tests
{
  using System;
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
      const string Type = "Sitecore.Security.AccessControl.AuthorizationProvider, Sitecore.Kernel";

      // act
      var obj = this.factory.GetObject(Type);

      // assert
      obj.Should().BeAssignableTo<AuthorizationProvider>();
    }

    [Fact]
    public void ShouldSetProviderNameIfTypeIsProvider()
    {
      // arrange
      const string Type = "Sitecore.Security.AccessControl.AuthorizationProvider, Sitecore.Kernel";

      // act
      var providerMock = (ProviderBase)this.factory.GetObject(Type);

      // assert
      providerMock.Name.Should().Be("AuthorizationProvider");
    }

    [Fact]
    public void ShouldThrowExceptionIfNoTypeFound()
    {
      // arrange
      const string Type = "wrong-type-name";

      // act
      Action action = () => this.factory.GetObject(Type);

      // assert
      action.ShouldThrow<TypeLoadException>();
    }
  }
}