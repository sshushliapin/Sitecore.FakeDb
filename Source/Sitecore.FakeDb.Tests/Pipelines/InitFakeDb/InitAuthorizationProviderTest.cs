namespace Sitecore.FakeDb.Tests.Pipelines.InitFakeDb
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Pipelines.InitFakeDb;
  using Sitecore.FakeDb.Security.AccessControl;
  using Sitecore.Pipelines;
  using Sitecore.Security.AccessControl;
  using Xunit;

  public class InitAuthorizationProviderTest
  {
    [Fact]
    public void ShouldSetAccessRulesStorage()
    {
      // arrange
      var provider = new InitAuthorizationProvider();

      var database = Database.GetDatabase("master");
      var args = new InitDbArgs(database, new DataStorage(database));

      ((FakeAuthorizationProvider)AuthorizationManager.Provider).AccessRulesStorage.Value = null;

      // act
      provider.Process(args);

      // assert
      ((FakeAuthorizationProvider)AuthorizationManager.Provider).AccessRulesStorage.Value.Should().BeEmpty();
    }
  }
}
