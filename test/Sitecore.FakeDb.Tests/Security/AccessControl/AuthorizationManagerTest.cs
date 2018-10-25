namespace Sitecore.FakeDb.Tests.Security.AccessControl
{
  using System;
  using FluentAssertions;
  using Sitecore.FakeDb.Security.AccessControl;
  using Sitecore.Security.AccessControl;
  using Xunit;

  public class AuthorizationManagerTest
  {
    [Obsolete]
    [Fact]
    public void ShouldResolveDefaultAuthorizationProvider()
    {
      // act & assert
      AuthorizationManager.Provider.Should().BeOfType<FakeAuthorizationProvider>();
    }
  }
}