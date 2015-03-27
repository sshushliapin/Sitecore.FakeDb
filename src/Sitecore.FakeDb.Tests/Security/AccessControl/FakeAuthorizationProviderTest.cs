namespace Sitecore.FakeDb.Tests.Security.AccessControl
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture;
  using Sitecore.Configuration;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.FakeDb.Security.AccessControl;
  using Sitecore.Security.AccessControl;
  using Sitecore.Security.Accounts;
  using Xunit;

  public class FakeAuthorizationProviderTest : IDisposable
  {
    private readonly AuthorizationProvider localProvider;

    private readonly ItemAuthorizationHelper helper;

    private readonly ISecurable entity;

    private readonly AccessRuleCollection rules;

    private readonly Item item;

    private FakeAuthorizationProvider provider;

    public FakeAuthorizationProviderTest()
    {
      this.provider = new FakeAuthorizationProvider();

      this.localProvider = Substitute.For<AuthorizationProvider>();
      this.helper = Substitute.For<ItemAuthorizationHelper>();

      this.entity = Substitute.For<ISecurable>();
      this.item = ItemHelper.CreateInstance();
      this.rules = new AccessRuleCollection();
    }

    [Fact]
    public void ShouldGetEmptyCollectionIfNoEntityFound()
    {
      // arrange
      var missingEntity = Substitute.For<ISecurable>();

      // act & assert
      this.provider.GetAccessRules(missingEntity).Should().BeEmpty();
    }

    [Fact]
    public void ShouldNotFailWhenSetAccessRules()
    {
      // act & assert
      this.provider.SetAccessRules(this.entity, this.rules);
    }

    [Fact]
    public void ShouldGetItemAccessRules()
    {
      // arrange   
      this.helper.GetAccessRules(this.item).Returns(this.rules);

      this.provider = new FakeAuthorizationProvider(this.helper);

      // act & assert
      this.provider.GetAccessRules(this.item).Should().BeSameAs(this.rules);
    }

    [Fact]
    public void ShouldSetItemAccessRules()
    {
      // arrange
      this.provider = new FakeAuthorizationProvider(this.helper);

      // act
      this.provider.SetAccessRules(this.item, this.rules);

      // assert
      this.helper.Received().SetAccessRules(this.item, this.rules);
    }

    [Fact]
    public void ShouldGetAccessPermissionAllowByDefault()
    {
      // arrange
      var fixture = new Fixture();
      var account = fixture.Create<User>();
      var accessRight = fixture.Create<AccessRight>();

      // act & assert
      this.provider
          .GetAccess(this.entity, account, accessRight)
          .ShouldBeEquivalentTo(new AccessResult(AccessPermission.Allow, new AccessExplanation("Allow")));
    }

    [Fact]
    public void ShouldBeThreadLocalProvider()
    {
      // act & assert
      this.provider.Should().BeAssignableTo<IThreadLocalProvider<AuthorizationProvider>>();
    }

    [Fact]
    public void ShouldCallGetAccess()
    {
      // arrange
      var fixture = new Fixture();
      var account = fixture.Create<User>();
      var accessRight = fixture.Create<AccessRight>();
      var accessResult = fixture.Create<AccessResult>();

      this.provider.LocalProvider.Value = this.localProvider;
      this.localProvider.GetAccess(this.entity, account, accessRight).Returns(accessResult);

      // act & assert
      this.provider.GetAccess(this.entity, account, accessRight).Should().BeSameAs(accessResult);
    }

    [Fact]
    public void ShouldCallGetAccessRules()
    {
      // arrange
      this.provider.LocalProvider.Value = this.localProvider;
      this.localProvider.GetAccessRules(this.entity).Returns(this.rules);

      // act & assert
      this.provider.GetAccessRules(this.entity).Should().BeSameAs(this.rules);
    }

    [Fact]
    public void ShouldCallSetAccessRules()
    {
      // arrange
      this.provider.LocalProvider.Value = this.localProvider;

      // act 
      this.provider.SetAccessRules(this.entity, this.rules);

      // assert
      this.localProvider.Received().SetAccessRules(this.entity, this.rules);
    }

    public void Dispose()
    {
      this.provider.LocalProvider.Value = null;
      Factory.Reset();
    }
  }
}