namespace Sitecore.FakeDb.Tests.Security.AccessControl
{
  using System;
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.FakeDb.Security.AccessControl;
  using Sitecore.Reflection;
  using Sitecore.Security.AccessControl;
  using Sitecore.Security.Accounts;
  using Xunit;
  using Xunit.Extensions;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using System.Threading;

  public class FakeAuthorizationProviderTest : IDisposable
  {
    private readonly Fixture fixture;

    private readonly Database database;

    private readonly DataStorage dataStorage;

    private readonly ISecurable entity;

    private readonly AccessRuleCollection rules;

    private FakeAuthorizationProvider provider;

    public FakeAuthorizationProviderTest()
    {
      this.fixture = new Fixture();
      this.database = Database.GetDatabase("master");
      this.dataStorage = new DataStorage(this.database);

      this.provider = new FakeAuthorizationProvider();
      this.provider.SetDataStorage(this.dataStorage);

      this.entity = Substitute.For<ISecurable>();
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
      Assert.DoesNotThrow(() => this.provider.SetAccessRules(this.entity, this.rules));
    }

    [Fact]
    public void ShouldGetAccessRules()
    {
      // arrange   
      this.entity.GetUniqueId().Returns("1");
      this.dataStorage.AccessRules.Add("1", this.rules);

      // act & assert
      this.provider.GetAccessRules(this.entity).Should().BeSameAs(this.rules);
    }

    [Fact]
    public void ShouldSetAccessRules()
    {
      // arrange
      this.entity.GetUniqueId().Returns("1");

      // act
      this.provider.SetAccessRules(this.entity, this.rules);

      // assert
      this.dataStorage.AccessRules["1"].Should().BeSameAs(this.rules);
    }

    [Fact]
    public void ShouldGetAccessPermissionAllowByDefault()
    {
      // arrange
      var account = this.fixture.Create<User>();
      var accessRight = this.fixture.Create<AccessRight>();

      // act & assert
      this.provider.GetAccess(this.entity, account, accessRight).ShouldBeEquivalentTo(new AccessResult(AccessPermission.Allow, new AccessExplanation("Allow")));
    }

    [Fact]
    public void ShouldGetAccessPermission()
    {
      // arrange
      this.entity.GetUniqueId().Returns("1");
      var user = Context.User;

      var rules = new AccessRuleCollection 
        {
          AccessRule.Create(user, AccessRight.ItemWrite, PropagationType.Entity, AccessPermission.Deny),
          AccessRule.Create(user, AccessRight.ItemWrite, PropagationType.Descendants, AccessPermission.Deny)
        };

      this.dataStorage.AccessRules.Add("1", rules);

      // act
      var accessResult = this.provider.GetAccess(this.entity, user, AccessRight.ItemWrite);

      // assert
      accessResult.Permission.Should().Be(AccessPermission.Deny);
    }

    [Fact]
    public void ShouldBeThreadSafe()
    {
      // arrange
      var rules1 = new AccessRuleCollection();
      var rules2 = new AccessRuleCollection();

      var t1 = Task.Factory.StartNew(() =>
        {
          this.provider.SetDataStorage(new DataStorage(this.database));

          this.provider.SetAccessRules(this.entity, rules1);

          Thread.Sleep(100);

          this.provider.GetAccessRules(this.entity).Should().BeSameAs(rules1);
        });

      var t2 = Task.Factory.StartNew(() =>
        {
          this.provider.SetDataStorage(new DataStorage(this.database));

          this.provider.SetAccessRules(this.entity, rules2);
          this.provider.GetAccessRules(this.entity).Should().BeSameAs(rules2);
        });

      t1.Wait();
      t2.Wait();
    }

    public void Dispose()
    {
      Factory.Reset();
    }
  }
}