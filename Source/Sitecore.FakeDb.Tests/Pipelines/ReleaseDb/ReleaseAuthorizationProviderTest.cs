namespace Sitecore.FakeDb.Tests.Pipelines.ReleaseDb
{
  using FluentAssertions;
  using Sitecore.FakeDb.Pipelines.ReleaseFakeDb;
  using Sitecore.FakeDb.Security.AccessControl;
  using Sitecore.Pipelines;
  using Sitecore.Security.AccessControl;
  using System.Collections.Generic;
  using Xunit;

  public class ReleaseAuthorizationProviderTest
  {
    [Fact]
    public virtual void ShouldResetAuthorizationrovider()
    {
      // arrange
      var processor = new ReleaseAuthorizationProvider();

      ((FakeAuthorizationProvider)AuthorizationManager.Provider).AccessRulesStorage.Value =
        new Dictionary<ISecurable, AccessRuleCollection>();

      // act
      processor.Process(new PipelineArgs());

      // assert
      ((FakeAuthorizationProvider)AuthorizationManager.Provider).AccessRulesStorage.Value.Should().BeNull();
    }
  }
}
