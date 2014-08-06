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

    private readonly ISecurable entity;

    private readonly AccessRuleCollection rules;

    private FakeAuthorizationProvider provider;

    public FakeAuthorizationProviderTest()
    {
      this.provider = new FakeAuthorizationProvider();
      this.fixture = new Fixture();
      this.database = Database.GetDatabase("master");

      this.entity = Substitute.For<ISecurable>();
      this.rules = new AccessRuleCollection();
    }

    [Fact]
    public void ShouldGetNullIfNoEntityFound()
    {
      // arrange
      var missingEntity = Substitute.For<ISecurable>();

      // act & assert
      this.provider.GetAccessRules(missingEntity).Should().BeNull();
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
      var rulesStorage = new Dictionary<ISecurable, AccessRuleCollection> { { this.entity, this.rules } };

      this.provider = new FakeAuthorizationProvider(rulesStorage);

      // act & assert
      this.provider.GetAccessRules(this.entity).Should().BeSameAs(this.rules);
    }

    [Fact]
    public void ShouldSetAccessRules()
    {
      // arrange    
      var rulesStorage = new Dictionary<ISecurable, AccessRuleCollection>();

      this.provider = new FakeAuthorizationProvider(rulesStorage);

      // act
      this.provider.SetAccessRules(this.entity, this.rules);

      // assert
      rulesStorage[this.entity].Should().BeSameAs(rules);
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

    [Theory]
    [InlineData("CanRead", WellknownRights.ItemRead)]
    [InlineData("CanWrite", WellknownRights.ItemWrite)]
    [InlineData("CanRename", WellknownRights.ItemRename)]
    [InlineData("CanCreate", WellknownRights.ItemCreate)]
    [InlineData("CanDelete", WellknownRights.ItemDelete)]
    [InlineData("CanAdmin", WellknownRights.ItemAdmin)]
    public void ShouldGetAccessPermissionIfConfigured(string propertyName, string accessRightName)
    {
      // arrange
      var itemId = ID.NewID;

      entity.GetUniqueId().Returns(ItemHelper.CreateInstance(itemId, this.database).GetUniqueId());

      var item = new DbItem("propertyName");
      ReflectionUtil.SetProperty(item.Access, propertyName, false);

      var dataStorage = Substitute.For<DataStorage>(this.database);
      dataStorage.GetFakeItem(itemId).Returns(item);

      this.provider.SetDataStorage(dataStorage);

      // act
      var accessResult = this.provider.GetAccess(this.entity, User.Current, AccessRight.FromName(accessRightName));

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
          this.provider.AccessRulesStorage.Value = new Dictionary<ISecurable, AccessRuleCollection>();
          this.provider.SetAccessRules(this.entity, rules1);

          Thread.Sleep(100);

          this.provider.GetAccessRules(this.entity).Should().BeSameAs(rules1);
        });

      var t2 = Task.Factory.StartNew(() =>
        {
          this.provider.AccessRulesStorage.Value = new Dictionary<ISecurable, AccessRuleCollection>();
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